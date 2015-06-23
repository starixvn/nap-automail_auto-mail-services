// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingServiceBase 
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;
        private IWinLossByProductsService _service;
        private IWinLossByProductsExportDataService _exportDataService;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\WinLossByProducts.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();

            _service = new WinLossByProductsService();
            _exportDataService = new WinLossByProductsExportDataService();
        }

        public Mail SendMailToVIPGroup()
        {
            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            var report = _service.GetWinLossByProductsData(criteria);
            _exportDataService.ExportReportToExcel(report, criteria);
            string reportPath = _exportDataService.DirectoryOfFileName;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants.WinLossByProductsTemplate;
            string template = File.ReadAllText(emailTemplatePath);

            WinLossProductsDTO data = new WinLossProductsDTO();
            data.WinLossByProducts = report;

            var bodyContent = Razor.Parse(template, data);

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(_exportDataService.DirectoryOfFileName);

            _mailInfo = new MailInfo()
            {
                FileName = _exportDataService.FileName,
                FileNamesPath = fileNamesPath
            };

            var mail = mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, bodyContent);

            return mail;
        }

        public void  SendMailToNormalGroup()
        {
            if (AppSettings.FunctionId > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }
    }
}
