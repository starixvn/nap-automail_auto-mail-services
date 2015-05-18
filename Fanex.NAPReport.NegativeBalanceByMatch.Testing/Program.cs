using Fanex.Data.Configuration;
using Fanex.NegativeBalanceByMatch.Business;
using Fanex.NegativeBalanceByMatch.Business.Business;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Helper;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
	class Program
	{
		static void Main(string[] args)
		{
			DbSettingProviderManager.Current.Start();

			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(AppSettings.LicenseeFunctionMap);
			ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);
			NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService);
			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = 
				new NegativeBalanceByMatchExcelService(licenseeService, negativeBalanceByMatchService);


			negativeBalanceByMatchExcelService.GenerateReportForLicensees();

			MailService mailService = new MailService(licenseeService, negativeBalanceByMatchExcelService);
			mailService.SendMails();


			//Mail mail = new Mail
			//{
			//    Subject = "Mail Station Test (Function Mail).",
			//    Title = "Mail Station Test (Function Mail).",
			//    Body = "Email Body.",
			//    MailingListId = 33 //AppSettings.GetFunctionId("Negative Balance By Match: Haifa")
			//};



			//MailClientHandler.Start();
			//MailClientHandler.SendMail(mail);
			//MailClientHandler.Stop();
		}
	}
}
