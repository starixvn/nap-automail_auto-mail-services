namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.ServiceFactory
{
    using System;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.MailService.ServiceFactory;

    public class NegativeBalanceByMatchServiceFactory : ITimerServiceFactory<IStandaloneTimerFunctionMailService>
    {
        public IStandaloneTimerFunctionMailService GetService()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            ConfigurationParser configurationParser = new ConfigurationParser(baseDir + @"SiteConfig\NegativeBalanceByMatch\Settings\NegativeBalanceByMatch.Settings.xml");
            configurationParser.Parse<AppSettings>();

            LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(baseDir + @"SiteConfig\NegativeBalanceByMatch\Settings\LicenseeFunctionMap.xml");
            ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

            return new NegativeBalanceByMatchService(licenseeService, baseDir);
        }
    }
}