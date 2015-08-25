// -----------------------------------------------------------------------
// <copyright file="SMAMonthlyReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.SMAMonthlyReport
{
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMAReport.Business.Service;
    using Fanex.Service.Core.MailService;
    using Quartz;

    public class SMAMonthlyReport : ScheduleMailBase
    {
        private MailingService service;

        public SMAMonthlyReport()
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
