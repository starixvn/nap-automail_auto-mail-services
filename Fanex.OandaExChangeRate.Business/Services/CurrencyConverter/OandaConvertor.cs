using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.OandaExChangeRate.Business.Services.CurrencyConverter
{
    using System.IO;
    using System.Net;

    using Fanex.OandaExChangeRate.Business.Infrastructure.Configurations;

    /*
     * Send a request to FXCSV:
 
       - Connect to http://www.oanda.com/cgi-bin/fxcsv/fxcsv and create your request with
        client_id=*********
        expr=list of currencies (or one currency). This is home currency
        exch=list of currencies (or leave empty to get all currencies). This is the quote currency.
        amount=amount for the exchange. Optional. Default value=1
        date=MM/DD/YYYY (starting date for the conversion). Optional. Default value=today
        nprices=number of conversions you wish to receive. Optional. Default value=1
 
        http://www.oanda.com/cgi-bin/fxcsv/fxcsv?client_id=[customerid]&expr=[list_home_currencies] &exch=[list_of_quote_currencies]
 

     */

    public class OandaConvertor : BaseCurrencyConvertor
    {
        public override double GetExchageRate(string fromCurrency, string toCurrency, string rateDate)
        {
            if (fromCurrency.Equals(toCurrency))
            {
                return 1.00;
            }


            var url = AppSettings.OandaUrl;
            url = string.Format(url, fromCurrency.Trim(), toCurrency.Trim(), rateDate);
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            //Response data:            
            /* Client_id oneworksOanda
              base,quote,amount,date,ask,bid
              SGD,EUR,1,05-06-2012,0.6148,0.6137
            */

            //rate = (ask + bid)/2

            var reader = new StreamReader(response.GetResponseStream());
            var responseString = reader.ReadToEnd();
            var lines = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

            if (lines.Length < 3) return 0.0;

            int askColumnIndex = -1, bidColumnIndex = -1;
            var arr = lines[1].Split(new string[] { "," }, StringSplitOptions.None);

            var i = 0;
            foreach (var col in arr)
            {
                if (col.ToLower() == "ask") askColumnIndex = i;
                if (col.ToLower() == "bid") bidColumnIndex = i;
                ++i;
            }

            if (askColumnIndex == -1 || bidColumnIndex == -1) return 0.0;

            if (askColumnIndex > bidColumnIndex) i = askColumnIndex;
            else i = bidColumnIndex;

            double ask, bid;
            arr = lines[2].Split(new string[] { "," }, StringSplitOptions.None);
            if (arr.Length < i) return 0.0;

            if (!double.TryParse(arr[askColumnIndex], out ask)) return 0.0;
            if (!double.TryParse(arr[bidColumnIndex], out bid)) return 0.0;

            return ask;
        }
    }
}
