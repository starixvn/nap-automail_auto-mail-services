using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NegativeBalanceByMatch.Business.Business
{
    public class MailService
    {
	    private readonly ILicenseeService _licenseeService;
	    private INegativeBalanceByMatchExcelService _negativeBalanceByMatchExcelService ;

		public MailService(ILicenseeService licenseeService, INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService)
		{
			_licenseeService = licenseeService;
			_negativeBalanceByMatchExcelService = negativeBalanceByMatchExcelService;
		}

	    public void SendMails()
		{
		    foreach (LicenseeFunction licenseeFunction in _licenseeService.GetLicenseeFunctions())
		    {
				Mail mail = new Mail
				{
					Subject = "Mail Station Test (Function Mail).",
					Title = "Mail Station Test (Function Mail).",
					Body = "Email Body.",
					MailingListId = licenseeFunction.Function.Id
				};

				//mail.Attachments.Add();

				MailClientHandler.Start();
				MailClientHandler.SendMail(mail);
				MailClientHandler.Stop();
		    }
	    }
    }
}
