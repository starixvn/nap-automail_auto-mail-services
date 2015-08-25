// -----------------------------------------------------------------------
// <copyright file="GameWinLimitControl.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.GameWinLimitControl
{
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.GameWinLimitControl.Business.Services;
    using Fanex.Service.Core.MailService;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimitControl : ScheduleMailBase
    {
        private IMailingService service;

        public GameWinLimitControl()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mailResult = service.SendMailToVIPGroup();
            
            return new JobResultMessage()
            {
                JobStatus = mailResult.JobStatus,
                Mail = mailResult.Mail,
                Schedule = schedule,
            };
        }
    }
}
