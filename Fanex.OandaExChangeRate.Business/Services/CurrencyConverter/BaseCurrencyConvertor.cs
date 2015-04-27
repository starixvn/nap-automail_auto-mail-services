namespace Fanex.OandaExChangeRate.Business.Services.CurrencyConverter
{
    using System;
    using System.Configuration;

    using Fanex.OandaExChangeRate.Business.Infrastructure.Configurations.BootStrappers;

    public abstract class BaseCurrencyConvertor
    {
        public static BaseCurrencyConvertor Instance
        {
            get
            {
                var settings = ConfigurationManager.GetSection
                    (BootStrapperMappingConstants.ConfigurationSectionName);
                var currencyClientType = Type.GetType(((BootStrapperSettings) settings).CurrencyConvertorClient);
                var currencyConvertorClient = (BaseCurrencyConvertor) Activator.CreateInstance(currencyClientType);
                return currencyConvertorClient;
            }
        }
        public abstract string GetExchageRate(string fromCurrency, string toCurrency, string rateDate);

        public abstract double GetExchageRate_Old(string fromCurrency, string toCurrency, string rateDate);

        public abstract double GetExchageRate_Simulate(string fromCurrency, string toCurrency, string rateDate);
    }
}
