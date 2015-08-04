namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.WinLossByProducts
{
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;
    using Quartz;

    public class OverallWinLossProducts : ScheduleMailBase
    {
        private IMailingServiceBase _serivce;

        public OverallWinLossProducts()
        {
            _serivce = new OverallWinLossProductsMailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = _serivce.SendMailToVIPGroup();

            return new JobResultMessage()
            {
                JobStatus = mail.JobStatus,
                Mail = mail.Mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            _serivce.SendMailToNormalGroup();
        }
    }
}
