// -----------------------------------------------------------------------
// <copyright file="MailingServiceFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business.ServiceFactory
{
    using System;
    using Fanex.NAPReport.TurnoverWatcher.Business.Service;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingServiceFactory : TimerFunctionMailServiceBase, IStandaloneTimerFunctionMailService
    {
        public override bool IsActive()
        {
            return true;
        }

        public override bool NotifyMailVipGroup()
        {
            bool result = false;
            try
            {
                IMailingServiceBase mailingService = new MailingService();
                mailingService.SendMailToVIPGroup();
                result = true;                
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
