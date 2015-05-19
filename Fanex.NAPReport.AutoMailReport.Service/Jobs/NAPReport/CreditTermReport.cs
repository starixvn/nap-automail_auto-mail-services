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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditTermReport : JobBase
    {
        private ICreditTermManager _creditTermService;
        private IMailService _mailService;

        public CreditTermReport()
        {
            _creditTermService = new CreditTermManager();
            _mailService = new MailService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var report = new Report
            {
                Name = "Credir Term",
                Date = DateTime.Now
            };
            var subject = Utilities.LoadSubject(report);
            var body = Utilities.LoadBody(report);
            _creditTermService.ExportExcel(report);
            var listAttachment = new List<MailAttachment>();
            listAttachment.Add(new MailAttachment(_creditTermService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            Mail mail = _mailService.CreateMail(subject, string.Empty, body, listAttachment, "Credit Term");

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