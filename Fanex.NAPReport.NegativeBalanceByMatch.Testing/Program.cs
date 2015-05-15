using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NegativeBalanceByMatch.Business.Helper;
using Fanex.NegativeBalanceByMatch.Business;
using Fanex.NegativeBalanceByMatch.Business.Business;
using Fanex.NegativeBalanceByMatch.Business.Business.ImportExport;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(AppSettings.LicenseeFunctionMap);
			ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);
	        NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService);
			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService(negativeBalanceByMatchService);



            Mail mail = new Mail
            {
                Subject = "Mail Station Test (Function Mail).",
                Title = "Mail Station Test (Function Mail).",
                Body = "Email Body.",
                MailingListId = 33 //AppSettings.GetFunctionId("Negative Balance By Match: Haifa")
            };

        

            MailClientHandler.Start();
            MailClientHandler.SendMail(mail);
            MailClientHandler.Stop();
        }
    }
}
