// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Service
{
    using System;
    using System.IO;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingServiceBase
    {
        private const string _SMA_REPORT_TEMPLATE = @"SiteConfig\SMAReport\Templates\SMAReportTemplate.cshtml";
        private ISMAExportExcelService _excelService;
        private MailInfo _mailInfo;
        private IMailStationService mailStation;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\SMAReport\Settings\SMAReport.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();

            _excelService = new SMAExportExcelService();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();
            _excelService.ExportReport();
            var data = _excelService.FileNames;
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + _SMA_REPORT_TEMPLATE;
            string template = File.ReadAllText(emailTemplatePath);
            var bodyContent = Razor.Parse(template, data);

            _mailInfo = new MailInfo()
            {
                FileName = string.Join(", ", _excelService.FileNames),
                FileNamesPath = _excelService.DirectoryOfFileNames
            };

            var mail = mailStation.CreateMailCustomContent(_mailInfo, bodyContent);
            mailResult.Mail = mail;

            return mailResult;
        }

        public void SendMailToNormalGroup()
        {
            if (AppSettings.FunctionId > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }
    }
}
