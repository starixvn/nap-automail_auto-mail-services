using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Models;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Mail mail = new Mail
            {
                Subject = "Mail Station Test (Function Mail).",
                Title = "Mail Station Test (Function Mail).",
                Body = "Email Body.",
                MailingListId = AppSettings.GetFunctionId("Negative Balance By Match: Haifa")
            };
         
            MailClientHandler.Start();
            MailClientHandler.SendMail(mail);
            MailClientHandler.Stop();
        }
    }
}
