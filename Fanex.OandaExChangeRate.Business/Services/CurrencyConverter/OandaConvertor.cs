namespace Fanex.OandaExChangeRate.Business.Services.CurrencyConverter
{
    using System;
    using System.IO;
    using System.Net;
    using System.Xml;

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
        public override string GetExchageRate(string fromCurrency, string toCurrency, string rateDate)
        {
            string ask = "0", bid = "0";
            string date = Convert.ToDateTime(rateDate).ToString("yyyy-MM-dd");
            try
            {
                if (fromCurrency.Equals(toCurrency))
                {
                    return "1";
                }

                if (fromCurrency.ToLower() == "c" || toCurrency.ToLower() == "c"
                    || fromCurrency.ToLower() == "h" || toCurrency.ToLower() == "h"
                    || fromCurrency.ToLower() == "uus" || toCurrency.ToLower() == "uus")
                {
                    return ask;
                }

                var url = AppSettings.OandaUrl;
                url = string.Format(url, fromCurrency.Trim(), toCurrency.Trim(), date);
                var request = WebRequest.Create(url);
                var response = request.GetResponse();

                XmlDocument xml = new XmlDocument();
                xml.Load(response.GetResponseStream());
                XmlNodeList xnlist = xml.SelectNodes("response/quotes/quote");

                foreach (XmlNode node in xnlist)
                {
                    var askTag = node.SelectNodes("ask");
                    if (askTag.Count > 0)
                    {
                        ask = askTag[0].InnerText;
                    }

                    var bidTag = node.SelectNodes("bid");
                    if (bidTag.Count > 0)
                    {
                        bid = bidTag[0].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "fromCurrency: " + fromCurrency + ", toCurrency: " + toCurrency + ", date: " + date + ", Exception: " + ex.Message;
                throw new Exception(msg);
            }

            return ask;
        }

        public override double GetExchageRate_Old(string fromCurrency, string toCurrency, string rateDate)
        {
            if (fromCurrency.Equals(toCurrency))
            {
                return 1.00;
            }

            var url = AppSettings.OandaUrl;
            url = string.Format(url, fromCurrency.Trim(), toCurrency.Trim(), Convert.ToDateTime(rateDate).ToString("yyyy-MM-dd"));
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            
            /*Response data:            
            Client_id oneworksOanda
              base,quote,amount,date,ask,bid
              SGD,EUR,1,05-06-2012,0.6148,0.6137
            */

            var reader = new StreamReader(response.GetResponseStream());
            var responseString = reader.ReadToEnd();
            var lines = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

            if (lines.Length < 3)
            {
                return 0.0;
            }

            int askColumnIndex = -1, bidColumnIndex = -1;
            var arr = lines[1].Split(new string[] { "," }, StringSplitOptions.None);

            var i = 0;
            foreach (var col in arr)
            {
                if (col.ToLower() == "ask")
                {
                    askColumnIndex = i;
                }

                if (col.ToLower() == "bid")
                {
                    bidColumnIndex = i;
                }

                ++i;
            }

            if (askColumnIndex == -1 || bidColumnIndex == -1)
            {
                return 0.0;
            }

            if (askColumnIndex > bidColumnIndex)
            {
                i = askColumnIndex;
            }
            else
            {
                i = bidColumnIndex;
            }

            double ask, bid;
            arr = lines[2].Split(new string[] { "," }, StringSplitOptions.None);
            if (arr.Length < i) 
            { 
                return 0.0; 
            }

            if (!double.TryParse(arr[askColumnIndex], out ask))
            {
                return 0.0;
            }

            if (!double.TryParse(arr[bidColumnIndex], out bid))
            {
                return 0.0;
            }

            return ask;
        }

        public override double GetExchageRate_Simulate(string fromCurrency, string toCurrency, string rateDate)
        {
            Random random = new Random();
            double ask = random.NextDouble() / 1000;

            return ask;
        }
    }
}
