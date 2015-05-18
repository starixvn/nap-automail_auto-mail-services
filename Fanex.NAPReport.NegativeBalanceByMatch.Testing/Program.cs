using System;
using Fanex.Data.Configuration;
using Fanex.NegativeBalanceByMatch.Business;
using Fanex.NegativeBalanceByMatch.Business.Business;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Helper;

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

				NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService);
				INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService =
					new NegativeBalanceByMatchExcelService(negativeBalanceByMatchService);

				MailService mailService = new MailService(licenseeService, negativeBalanceByMatchService, negativeBalanceByMatchExcelService);
				mailService.SendMails();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}
		}
	}
}
