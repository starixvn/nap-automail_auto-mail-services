namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.BA
{
    using Fanex.NAPReport.BADailyWinLoss.Business.Service;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyWinLoss : ScheduleMailBase
    {
        private IMailingService service;

        public DailyWinLoss()
        {
            service = new MailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailToVIPGroup();

            return new JobMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            service.SendMailToNormalGroup();
        }
    }
}