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
    using Fanex.NAPReport.BalanceReport;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DirectBalanceReport : JobBase
    {
        private IBalanceManager _masterBalanceService;
        private IBalanceManager _agentBalanceService;
        private IBalanceManager _memberBalanceService;
        private IMailService _mailService;

        public DirectBalanceReport()
        {
            var masterCriteria = new DirectBalanceReportMasterCriteria
                {
                    Date = DateTime.Now,
                    Name = "Master Balance",
                    Level = "Master"
                };
            _masterBalanceService = new BalanceManager(masterCriteria);

            var agentCriteria = new DirectBalanceReportAgentCriteria
            {
                Date = DateTime.Now,
                Name = "Direct Agent",
                Level = "Agent"
            };
            _agentBalanceService = new BalanceManager(agentCriteria);

            var memberCriteria = new DirectBalanceReportMemberCriteria
            {
                Date = DateTime.Now,
                Name = "Direct Member",
                Level = "Member"
            };
            _memberBalanceService = new BalanceManager(memberCriteria);

            _mailService = new MailService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var report = new Report
            {
                Name = "Direct Balance",
                Date = DateTime.Now,
                IsDirectReport = true
            };
            var subject = Utilities.LoadSubject(report);
            var body = Utilities.LoadBody(report);

            _masterBalanceService.ExportExcel();
            _agentBalanceService.ExportExcel();
            _memberBalanceService.ExportExcel();

            var listAttachment = new List<MailAttachment>();
            listAttachment.Add(new MailAttachment(_masterBalanceService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            listAttachment.Add(new MailAttachment(_agentBalanceService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            listAttachment.Add(new MailAttachment(_memberBalanceService.ReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            Mail mail = _mailService.CreateMail(subject, string.Empty, body, listAttachment, "Direct Balance Report");

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