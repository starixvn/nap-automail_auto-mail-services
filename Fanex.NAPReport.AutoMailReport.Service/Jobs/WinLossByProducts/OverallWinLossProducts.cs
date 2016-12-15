namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.WinLossByProducts
{
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    public class OverallWinLossProducts : ScheduleMailBase
    {
        private IMailingServiceBase _serivce;

        public OverallWinLossProducts()
        {
            _serivce = new OverallWinLossProductsMailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = _serivce.SendMailToVIPGroup();

            return new JobMessage()
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