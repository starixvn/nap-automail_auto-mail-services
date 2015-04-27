namespace Fanex.OandaExChangeRate.Business.Infrastructure.Configurations.BootStrappers
{
    using System.Configuration;

    public class BootStrapperSettings : ConfigurationSection
    {
        [ConfigurationProperty(BootStrapperMappingConstants.DefaulFullTypeNameAttributeName, IsRequired = true)]
        public string DefaultBootStrapperFullTypeName
        {
            get
            {
                if (this.Properties.Contains(BootStrapperMappingConstants.DefaulFullTypeNameAttributeName))
                {
                    return base[BootStrapperMappingConstants.DefaulFullTypeNameAttributeName].ToString();
                }

                return BootStrapperMappingConstants.DefaultBootStrapperFullTypeName;
            }
        }

        [ConfigurationProperty("CurrencyConvertorClient", IsRequired = true)]
        public string CurrencyConvertorClient
        {
            get { return base["CurrencyConvertorClient"].ToString(); }
        }
    }
}
