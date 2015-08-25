namespace Fanex.NAPReport.RngCasino.Business.Business
{
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.MailService;

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
                MailingListId = AppSettings.FunctionIdVipGroup
            };

            MailClientHandler.SendMail(mail);

            return true;
        }
    }
}