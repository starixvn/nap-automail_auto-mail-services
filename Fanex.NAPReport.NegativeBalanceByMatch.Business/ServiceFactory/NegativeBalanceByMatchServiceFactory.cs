using System;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;
using Fanex.Service.Core.Configuration;
using Fanex.Service.Core.MailService;
using Fanex.Service.Core.MailService.ServiceFactory;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.ServiceFactory
{
	public class NegativeBalanceByMatchServiceFactory : ITimerServiceFactory<ICommonTimerFunctionMailService>
	{
		public ICommonTimerFunctionMailService GetService()
		{
			ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\NegativeBalanceByMatch\NegativeBalanceByMatch.Settings.xml");
			configurationParser.Parse<NegativeBalanceByMatchSettings>();

			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(@"SiteConfig\NegativeBalanceByMatch\LicenseeFunctionMap.xml");
			ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

			MailService mailService = new MailService(negativeBalanceByMatchExcelService);

			return new NegativeBalanceByMatchService(licenseeService, mailService,
				AppDomain.CurrentDomain.BaseDirectory + @"SiteConfig\NegativeBalanceByMatch");
		}
	}
}
