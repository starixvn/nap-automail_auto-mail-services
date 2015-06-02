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
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Bussiness;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Quartz;
    using RazorEngine;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Core;
    using Fanex.NAPReport.BalanceMonthlyReport.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperBalanceReport : JobBase
    {
        private IBalanceManager _balanceService;
        private IMailService _mailService;

        public SuperBalanceReport()
        {
            var superCriteria = new SuperBalanceReportCriteria
                {
                    Date = DateTime.Now,
                    Name = "Super Balance",
                    Level = "Super"
                };
            _balanceService = new BalanceManager(superCriteria);
            _mailService = new MailService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var report = new Report
            {
                Name = "Super Balance",
                Date = DateTime.Now
            };
            var subject = Utilities.LoadSubject(report);
            var body = Utilities.LoadBody(report);

            _balanceService.ExportExcel();

            var listAttachment = new List<MailAttachment>();
            listAttachment.Add(new MailAttachment(_balanceService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            Mail mail = _mailService.CreateMail(subject, string.Empty, body, listAttachment);

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

        public JobResultMessage Run2()
        {
            var report = new Report
            {
                Name = "Super Balance",
                Date = DateTime.Now
            };
            var subject = Utilities.LoadSubject(report);
            var body = Utilities.LoadBody(report);

            _balanceService.ExportExcel();

            var listAttachment = new List<MailAttachment>();
            listAttachment.Add(new MailAttachment(_balanceService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            Mail mail = _mailService.CreateMail(subject, string.Empty, body, listAttachment);

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                // Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }
    }
}