namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.VirtualSportWinLoss
{
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.Service.Core.MailService;
    using Quartz;

    public class VirtualSportWinLoss : ScheduleMailBase
    {
        private IMailingServiceBase service;

        public VirtualSportWinLoss()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailToVIPGroup();

            return new JobResultMessage()
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
