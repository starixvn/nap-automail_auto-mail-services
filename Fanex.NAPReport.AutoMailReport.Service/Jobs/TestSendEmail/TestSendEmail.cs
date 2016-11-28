// -----------------------------------------------------------------------
// <copyright file="TestSendEmail.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.TestSendEmail
{
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TestSendEmail : ScheduleMailBase
    {
        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            Mail mail = new Mail()
            {
                Subject = schedule.Name,
                Title = "Title - Testing",
                Body = "Body - Testing"
            };

            return new JobMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }
    }
}