// -----------------------------------------------------------------------
// <copyright file="TestSendEmail.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.TestSendEmail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.MailService;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TestSendEmail : ScheduleMailBase
    {
        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            Mail mail = new Mail() 
            {
                Subject = schedule.Name,
                Title = "Title - Testing",
                Body = "Body - Testing"
            };

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }
    }
}
