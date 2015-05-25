namespace Fanex.NAPReport.TurnoverWatcher.Service.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Configuration;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.Service.Core.Configuration;
    using RazorEngine;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running VirtualSportWinLoss");

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            var virtualSportEmailTemplate = @"/Templates/EmailTemplate.cshtml";
            var settings = @"/Templates/VirtualSportWinLoss.Settings.xml";
            var service = new VirtualSportService();
            var excelService = new ExportExcelService();
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + virtualSportEmailTemplate;
            string template = File.ReadAllText(filePath);
            var periodReportTime = new PeriodTimeReport();
            var yesterday = DateTime.Now.AddDays(-1);
            var startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);

            periodReportTime.FromDate = startMonth.ToYearMonthDayFormat().Replace("/", "-");
            periodReportTime.ToDate = yesterday.ToYearMonthDayFormat().Replace("/", "-");
            var emailContent = Razor.Parse(template, periodReportTime);

            var configurationParser = new ConfigurationParser(AppDomain.CurrentDomain.BaseDirectory + settings);
            configurationParser.Parse();
            configurationParser.Parse<Settings>();

            Mail mail = new Mail()
            {
                Subject = Settings.Subject,
                Title = Settings.Subject,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            var fileAttachment = excelService.ExportExcelToDisk();
            mail.Attachments.Add(new MailAttachment(fileAttachment, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            Console.WriteLine("Done at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:s tt"));
            Console.ReadLine();
        }
    }
}