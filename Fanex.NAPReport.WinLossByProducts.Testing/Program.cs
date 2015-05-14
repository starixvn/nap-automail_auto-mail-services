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
        private static IWinLossByProductsService _service;
        private static IWinLossByProductsExportDataService _exportDataService;
        private static IWinLossByProductsMailService _mailService;

        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            InitServices();
            Console.WriteLine("Running Win Loss By Products Report");
            GetWinLossByProductsCriteria criteria = CreateWinLossByProductCriteria();
            var report = _service.GetWinLossByProductsData(criteria);
            _exportDataService.ExportReportToExcel(report, criteria);
            string reportPath = _exportDataService.ReportPath;
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants.WinLossByProductsTemplate;
            string template = File.ReadAllText(emailTemplatePath);
            var emailContent = Razor.Parse(template, report);
            Mail mail = _mailService.CreateMail(emailContent, reportPath, criteria);
            _mailService.SendMailByFunction(mail);
            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }

        private static void InitServices()
        {
            _service = new WinLossByProductsService();
            _exportDataService = new WinLossByProductsExportDataService();
            _mailService = new WinLossByProductsMailService();
        }

        private static GetWinLossByProductsCriteria CreateWinLossByProductCriteria()
        {
            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            return criteria;
        }
    }
}