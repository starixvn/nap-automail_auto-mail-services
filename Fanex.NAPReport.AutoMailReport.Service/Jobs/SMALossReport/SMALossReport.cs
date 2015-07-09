// -----------------------------------------------------------------------
// <copyright file="SMALossReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.SMALossReport
{
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMALoss.Business.Services;
    using Fanex.Service.Core.MailService;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMALossReport : ScheduleMailBase
    {
         private IMailingServiceBase service;

         public SMALossReport()
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
