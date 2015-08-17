// -----------------------------------------------------------------------
// <copyright file="CreditMovement.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.CreditMovement
{
    using System;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.MailService;
    using Quartz;
    using Fanex.CreditMovement.Business.Service;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditMovement : ScheduleMailBase
    {
        private MailingService _service;

        public CreditMovement()
        {
            _service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = _service.SendMailToVIPGroup();

            //Mail mail = new Mail()
            //{
            //    Subject = schedule.Name,
            //    Title = "Title - Testing",
            //    Body = "Body - Testing"
            //};

            return new JobResultMessage()
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
