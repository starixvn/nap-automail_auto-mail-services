// -----------------------------------------------------------------------
// <copyright file="DirectBalance.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.BalanceMonthlyReport
{
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DirectBalance : ScheduleMailBase
    {
        private IMailingService service;

        public DirectBalance()
        {
            service = new MailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailDirectBalanceToVIPGroup();

            return new JobMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            service.SendMailToDirectBalanceNormalGroup();
        }
    }
}