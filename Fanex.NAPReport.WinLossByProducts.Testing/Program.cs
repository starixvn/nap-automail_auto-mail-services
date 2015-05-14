namespace Fanex.NAPReport.WinLossByProducts.Testing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business;
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            IWinLossByProductsService service = new WinLossByProductsService();
            IWinLossByProductsExportDataService exportDataService = new WinLossByProductsExportDataService();
            Console.WriteLine("Running Win Loss By Products Report");
            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            var report = service.GetWinLossByProductsData(criteria);
            exportDataService.ExportReportToExcel(report, criteria);
            string reportPath = exportDataService.ReportPath;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants.WinLossByProductsTemplate;
            string template = File.ReadAllText(emailTemplatePath);
            var emailContent = Razor.Parse(template, report);

            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettings.Subject, criteria.WinLossDate.ToShortFormatedDateWithHourMinute()),
                Title = AppSettings.Title,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(reportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            mail.MailingListId = mailStationAppSettings.GetFunctionId(AppSettings.MailFunctionName);
            MailClientHandler.SendMail(mail);
            Console.WriteLine(reportPath);

            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}