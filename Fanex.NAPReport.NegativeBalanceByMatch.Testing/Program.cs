using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;
using Fanex.NegativeBalanceByMatch.Business.Business;
using Fanex.NegativeBalanceByMatch.Business.Business.ImportExport;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            DbSettingProviderManager.Current.Start();

            NegativeBalanceByMatchService service = new NegativeBalanceByMatchService();

            INegativeBalanceByMatchExcelService excelService = new NegativeBalanceByMatchExcelService(service);
            excelService.GenerateReportForLicensees();

            //Mail mail = new Mail
            //{
            //    Subject = "Mail Station Test (Function Mail).",
            //    Title = "Mail Station Test (Function Mail).",
            //    Body = "Email Body.",
            //    MailingListId = AppSettings.GetFunctionId("Negative Balance By Match: Haifa")
            //};

            //MailClientHandler.SendMail(mail);


           
        }
    }
}
