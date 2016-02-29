namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Repositories;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    public class OverallWinLossProductsMailingService : IMailingServiceBase
    {
        private IMailStationService _mailStation;
        private MailInfo _mailInfo;
        private IExportExcelService _excel;

        public OverallWinLossProductsMailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\OverallWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();

            _excel = new ExportExcelService();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();

            OverallWinLossProductsCriteria criteria = new OverallWinLossProductsCriteria();
            criteria.RptDate = AppSettings.IsTesting ? AppSettings.WinLossDate : DateTime.Today.AddDays(-1);

            var lastDate = new Repository().GetLastDateDataAvaiable();

            bool isNotifyEmail = criteria.RptDate != lastDate && AppSettings.IsCheckEnoughData;
            _mailInfo.Subject = string.Format(AppSettings.SubjectOverall, DateTime.Now.ToString("yyyy/MM/dd"));

            if (isNotifyEmail)
            {
                mailResult.JobStatus = JobStatus.SuccessWithoutSendmail;
                SendFunctionMailCustomContentNotification();
            }
            else
            {
                var mail = SendEmail(criteria);
                mailResult.Mail = mail;
            }

            return mailResult;
        }

        private Mail SendEmail(OverallWinLossProductsCriteria criteria)
        {
            _excel.ExportExcel(criteria);

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(_excel.DirectoryOfFileName);

            _mailInfo.FileName = _excel.FileName;
            _mailInfo.FileNamesPath = fileNamesPath;

            var mail = _mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }

        private void SendFunctionMailCustomContentNotification()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + @"SiteConfig\WinLossByProducts\Templates\OverallWLNotificationEmailTemplate.cshtml";
            string template = File.ReadAllText(emailTemplatePath);

            _mailInfo.FileName = "Overall Products Win Loss";
            _mailStation.SendFunctionMailCustomContentNotification(Service.Core.Utilities.AppSettingBase.NotificationEmailFunctionId, _mailInfo, template);
        }

        public void SendMailToNormalGroup()
        {
            if (AppSettings.FunctionId > 0)
            {
                _mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }
    }
}
