// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingServiceBase
    {
        private IMailStationService _mailStation;
        private MailInfo _mailInfo;

        public MailingService()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            ConfigurationParser configurationParser = new ConfigurationParser(root + @"SiteConfig\ReportingServices\Settings\ReportingService.Settings.xml");
            configurationParser.Parse<AppSettings>();

            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();

            var serviceProcess = new ServiceProcess();

            var companyBalance = new CompanyBalanceProcessData(serviceProcess);
            var companyStatement = new CompanyStatementProcessData(serviceProcess);
            var dailyTurnover = new DailyTurnoverProcessData(serviceProcess);
            var superYesterdayBalance = new SuperYesterdayBalanceProcessData(serviceProcess);
            var matchWinLoss2a = new MatchWinLoss2aProcessData(serviceProcess);
            var currentDate = DateTime.Today.AddDays(-1);
            if (AppSettings.IsTest)
            {
                currentDate = AppSettings.WinLossDate;
            }

            serviceProcess.RunService(currentDate);

            var fileNamesPath = new List<string>();
            var fileNames = new List<string>();

            fileNamesPath.Add(companyBalance.FileName);
            fileNamesPath.Add(companyStatement.FileName);
            fileNamesPath.Add(dailyTurnover.FileName);
            fileNamesPath.Add(superYesterdayBalance.FileName);
            fileNamesPath.Add(matchWinLoss2a.FileName);

            foreach (string fileNamePath in fileNamesPath)
            {
                fileNames.Add(Path.GetFileName(fileNamePath));
            }

            _mailInfo = new MailInfo()
            {
                FileName = string.Join(", ", fileNames),
                FileNamesPath = fileNamesPath
            };

            var mail = _mailStation.CreateMailWithNotification(_mailInfo);
            mail.Subject = "Automail Report - Reporting Service on " + AppSettings.WinLossDate.ToShortDateString();
            mailResult.Mail = mail;

            return mailResult;
        }

        public void SendMailToNormalGroup()
        {
        }
    }
}
