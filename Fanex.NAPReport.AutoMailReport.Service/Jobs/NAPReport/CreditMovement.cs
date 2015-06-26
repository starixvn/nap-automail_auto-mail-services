// -----------------------------------------------------------------------
// <copyright file="WinLossByProducts.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using System;
    using Fanex.CreditMovement.Business;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Service;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditMovement : JobBase
    {
        private ICreditMovementService _creditMovementService;
        private IMailService _mailService;

        public CreditMovement()
        {
            _creditMovementService = new CreditMovementService();
            _mailService = new MailService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            GetCreditMovementCriteria criteria = new GetCreditMovementCriteria
            {
                FromDate = AppSettings.FromDate,
                ToDate = AppSettings.ToDate,
                SiteList = string.Join(",", AppSettings.ListSite),
                Amount = AppSettings.Amount
            };

            string subject = schedule.Name + " - " + DateTime.Now.ToString("yyyy-MM-dd");
            var mail = _creditMovementService.ProcessToSendEmail(criteria, subject);

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }
    }
}