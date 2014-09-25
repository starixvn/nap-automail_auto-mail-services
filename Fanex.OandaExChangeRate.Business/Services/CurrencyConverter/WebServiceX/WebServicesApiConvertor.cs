[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://www.webserviceX.NET/", ClrNamespace = "www.webservicex.net")]
namespace Fanex.OandaExChangeRate.Business.Services.CurrencyConverter.WebServiceX
{
    using System;

    public class WebServicesApiConvertor: BaseCurrencyConvertor
    {
        public override double GetExchageRate(string fromCurrency, string toCurrency, string shortDateTime)
        {
            double dResult = 0;
            var wsCurrencyConvertor = new CurrencyConvertorSoapClient();
            dResult = wsCurrencyConvertor.ConversionRate((Currency) Enum.Parse(typeof (Currency), fromCurrency),
                                                         (Currency) Enum.Parse(typeof (Currency), toCurrency));
            return dResult;
        }
    }
}
