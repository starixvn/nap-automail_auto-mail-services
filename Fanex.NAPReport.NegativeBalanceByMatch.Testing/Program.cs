using System;
using Fanex.Data.Configuration;
using Fanex.NAPReport.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				DbSettingProviderManager.Current.Start();

				LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(AppSettings.LicenseeFunctionMap);
				ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

				INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

				MailService mailService = new MailService(negativeBalanceByMatchExcelService);

				NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService, mailService);

				negativeBalanceByMatchService.ProcessBusiness();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}
		}
	}
}
