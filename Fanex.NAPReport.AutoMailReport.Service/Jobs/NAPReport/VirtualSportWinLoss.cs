namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.HorseRacingReport.Business;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Configuration;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.Service.Core.Configuration;
    using Quartz;
    using RazorEngine;

    public class VirtualSportWinLoss : JobBase
    {
        private const string VirtualSportEmailTemplate = @"/SiteConfig/VirtualSportWinLoss/Templates/EmailTemplate.cshtml";
        private const string AppSettings = @"/SiteConfig/VirtualSportWinLoss/VirtualSportWinLoss.Settings.xml";

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var service = new VirtualSportService();
            var excelService = new ExportExcelService();

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + VirtualSportEmailTemplate;
            string template = File.ReadAllText(filePath);
            var periodReportTime = new PeriodTimeReport();
            var yesterday = DateTime.Now.AddDays(-1);
            var startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);

            periodReportTime.FromDate = startMonth.ToYearMonthDayFormat().Replace("/", "-");
            periodReportTime.ToDate = yesterday.ToYearMonthDayFormat().Replace("/", "-");
            var emailContent = Razor.Parse(template, periodReportTime);

            var configurationParser = new ConfigurationParser(AppDomain.CurrentDomain.BaseDirectory + AppSettings);
            configurationParser.Parse<Settings>();

            string emailSubject = string.Format(
                    Settings.Subject,
                    DateTime.Now.ToYearMonthDayFormat().Replace("/", "-"));

            Mail mail = new Mail()
            {
                Subject = emailSubject,
                Title = Settings.Title,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            var fileAttachment = excelService.ExportExcelToDisk();
            mail.Attachments.Add(new MailAttachment(fileAttachment, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }
    }
}