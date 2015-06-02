// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BADailyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingService
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BADailyWinLoss\Settings\BADailyWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        public Mail SendMailToVIPGroup()
        {
            var service = new BAReportService();
            var isTesting = AppSettings.IsTest;
            var testedDate = AppSettings.TestedDate;
            var winLossDate = isTesting ? testedDate : DateTime.Now.AddDays(-1);

            service.GenerateReport(winLossDate);

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(service.DirectoryOfFileName);

            _mailInfo = new MailInfo() 
            { 
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
