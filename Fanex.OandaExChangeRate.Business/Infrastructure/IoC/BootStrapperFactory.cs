namespace Fanex.OandaExChangeRate.Business.Infrastructure.IoC
{
    using System;
    using System.Configuration;

    using Fanex.OandaExChangeRate.Business.Infrastructure.Configurations.BootStrappers;
    using Fanex.OandaExChangeRate.Business.Services.BootStrappers;

    public static class BootStrapperFactory
    {
        /// <summary>
        /// By default the factory create Unity Bootstrapper instance
        /// 
        /// </summary>
        /// <returns></returns>
        public static CommonBootStrapper CreateInstance()
        {
            try
            {
                var settings = ConfigurationManager.GetSection
                    (BootStrapperMappingConstants.ConfigurationSectionName);


                if (settings != null)
                {
                    var bootStrapperType = Type.GetType(((BootStrapperSettings)settings).DefaultBootStrapperFullTypeName);
                    var bootStrapper = (CommonBootStrapper)Activator.CreateInstance(bootStrapperType);

                    return bootStrapper;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return new UnityBootStrapper();
        }


    }
}
