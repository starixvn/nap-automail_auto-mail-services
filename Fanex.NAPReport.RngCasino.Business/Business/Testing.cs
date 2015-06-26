using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Service.Core.MailService;
using Fanex.MailDelivery.Client.Models;
using Fanex.MailDelivery.Client.Handlers;

namespace Fanex.NAPReport.RngCasino.Business.Business
{
    public class Testing : TimerFunctionMailServiceBase, IStandaloneTimerFunctionMailService
    {
        public override bool IsActive()
        {
            return true;
        }

        public override bool NotifyMailVipGroup()
        {
            Mail mail = new Mail()
            {
                Subject = "Subject",
                Title = "Title",
                Body = "bodyContent - RNG Casino",
                MailingListId = 91
            };

            MailClientHandler.SendMail(mail);

            return true;
        }
    }
}
