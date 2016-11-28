// -----------------------------------------------------------------------
// <copyright file="ReportingServices.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.ReportingServices
{
    using Fanex.NAPReport.ReportingServices.Business.Service;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportingServices : ScheduleMailBase
    {
        private IMailingServiceBase _service;

        public ReportingServices()
        {
            _service = new MailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = _service.SendMailToVIPGroup();

            return new JobMessage()
            {
                JobStatus = mail.JobStatus,
                Mail = mail.Mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            _service.SendMailToNormalGroup();
        }
    }
}