// -----------------------------------------------------------------------
// <copyright file="WinLossByProducts.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.BalanceReport.Bussiness;
    using Fanex.NAPReport.BalanceReport.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Quartz;
    using RazorEngine;
    using Fanex.NAPReport.BalanceReport.Core;
    using Fanex.CreditMovement.Business.Service;
using Fanex.CreditMovement.Business.Criteria;
using Fanex.CreditMovement.Business;

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

            var mail = _creditMovementService.ProcessToSendEmail(criteria);

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }
    }
}