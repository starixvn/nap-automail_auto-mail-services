namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.VirtualSportWinLoss
{
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    public class VirtualSportWinLoss : ScheduleMailBase
    {
        private IMailingServiceBase service;

        public VirtualSportWinLoss()
        {
            service = new MailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailToVIPGroup();

            return new JobMessage()
            {
                JobStatus = mail.JobStatus,
                Mail = mail.Mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            service.SendMailToNormalGroup();
        }
    }
}