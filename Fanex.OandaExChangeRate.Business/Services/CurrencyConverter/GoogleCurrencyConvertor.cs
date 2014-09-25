namespace Fanex.OandaExChangeRate.Business.Services.CurrencyConverter
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using Fanex.OandaExChangeRate.Business.Model;
    using Newtonsoft.Json;
    using Fanex.Common;

    public class GoogleCurrencyConvertor: BaseCurrencyConvertor
    {
        public override double GetExchageRate(string fromCurrency, string toCurrency, string rateDate)
        {
            if (fromCurrency.Equals(toCurrency))
            {
                return 1.00;
            }
            var currency = new GoogleCurrency();
            var url = AppSettings.GoogleApiUrl;
            url = string.Format(url, fromCurrency.Trim(), toCurrency.Trim());
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var responseString = reader.ReadToEnd();
            //var test = "{lhs: \"1 U.S. dollar\",rhs: \"19�607.8431 Vietnamese dong\",error: \"\",icc: true}";
            responseString = responseString.Replace("\\x3csup\\x3e", "\x3csup\x3e");
            responseString = responseString.Replace("\\x3c/sup\\x3e", "\x3c/sup\x3e");
            responseString = responseString.Replace("\\x26#215;", "\x26#215;");
            var regString = @"[-+]?[0-9]*\.?[0-9]* \x26#215; 10\x3csup\x3e([0-9-]+)\x3c\/sup\x3e";

            var rate = 0.0;

            var reg = new Regex(regString);

            if (!reg.IsMatch(responseString))
            {
                currency = JsonConvert.DeserializeObject<GoogleCurrency>(responseString);
                if(!string.IsNullOrEmpty(currency.error))
                {
                    //send mail
                    return 0.0;
                    //throw new Exception();
                }
                rate = Convert.ToDouble(currency.rhs.Split(' ')[0]);
                
            }
            else
            {
                var match = reg.Match(responseString);
                var result = match.Value;
                rate = DeGoogleLargeNumbers(result);
            }
            return rate;
        }

        private static double DeGoogleLargeNumbers(string googleNumberString)
        {
           // string format: x * pow(10, y)
            const string regX = @"[-+]?[0-9]*\.?[0-9]* ";
            var reg = new Regex(regX);
            var xValue = Converter.To<double>(reg.Match(googleNumberString).Value);

            const string regNumber = "([0-9-]+)";
            const string regSupString = @"<sup>([0-9-]+)</sup>";
            reg = new Regex(regSupString);
            var supString = reg.Match(googleNumberString).Value;
            reg = new Regex(regNumber);
            var supValue = Converter.To<double>(reg.Match(supString).Value);
            var yValue = Math.Pow(10, supValue);

            var value = xValue * yValue;

            return value;
        }

       
    }
}
