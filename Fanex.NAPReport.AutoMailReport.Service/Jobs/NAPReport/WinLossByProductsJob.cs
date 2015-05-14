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
    using Fanex.NAPReport.WinLossByProducts.Business;
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.Extensions;
    using Quartz;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsJob : JobBase
    {
        private IWinLossByProductsService _winLossByProductsService;
        private IWinLossByProductsExportDataService _exportWinLossByProductsDataService;

        public WinLossByProductsJob()
        {
            _winLossByProductsService = new WinLossByProductsService();
            _exportWinLossByProductsDataService = new WinLossByProductsExportDataService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            GetWinLossByProductsCriteria criteria = WinLossByProductsCriteria();
            WinLossByProducts winLossByProducts = _winLossByProductsService.GetWinLossByProductsData(criteria);
            _exportWinLossByProductsDataService.ExportReportToExcel(winLossByProducts, criteria);
            string reportPath = _exportWinLossByProductsDataService.ReportPath;
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants.WinLossByProductsTemplate;
            string template = File.ReadAllText(emailTemplatePath);
            var emailContent = Razor.Parse(template, template);

            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettings.Subject, criteria.WinLossDate.ToShortFormatedDateWithHourMinute()),
                Title = AppSettings.Title,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(reportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

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

        private GetWinLossByProductsCriteria WinLossByProductsCriteria()
        {
            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            return criteria;
        }
    }
}