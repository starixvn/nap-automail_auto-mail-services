namespace Fanex.NAPReport.GameWinLimitControl.Business.ServiceFactory
{
    using System;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.GameWinLimitControl.Business.Services;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;

    public class MailingServiceFactory : TimerFunctionMailServiceBase, IStandaloneTimerFunctionMailService
    {
        public override bool IsActive()
        {
            return AppSettings.IsActive;
        }

        public override bool NotifyMailVipGroup()
        {
            bool result = false;
            try
            {
                IMailingService mailingService = new MailingService();
                var mailResult = mailingService.SendMailToVIPGroup();
                if (mailResult.JobStatus == JobStatus.Success)
                {
                    mailResult.Mail.MailingListId = AppSettings.FunctionId;
                    MailClientHandler.SendMail(mailResult.Mail);
                    result = true;
                }

                mailingService = null;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result = false;
            }

            return result;
        }
    }
}
