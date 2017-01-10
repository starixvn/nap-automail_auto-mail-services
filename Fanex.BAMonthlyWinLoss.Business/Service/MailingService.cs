// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using MailDelivery.Core.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingService
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BAMonthlyWinLoss\Settings\BAMonthlyWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        public Mail SendMailToVIPGroup()
        {
            var service = new BAReportService();
            service.GenerateReport();

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(service.DirectoryOfFileName);

            _mailInfo = new MailInfo()
            {
                Subject = string.Format(AppSettings.Subject, DateTime.Now.ToString("MM/yyyy")),
                FileName = service.FileName,
                FileNamesPath = fileNamesPath
            };

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
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