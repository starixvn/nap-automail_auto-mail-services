// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Balance;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.CreditTerm;
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
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BalanceMonthlyReport\Settings\BalanceMonthlyReport.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        public Mail SendMailCreditTermToVIPGroup()
        {
            ICreditTermService service = new CreditTermService();
            service.ExportExcelCreditTerm();

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(service.DirectoryOfFileName);

            _mailInfo = new MailInfo()
            {
                Subject = string.Format(AppSettings.CreditTermSubject, DateTime.Now.ToString("MM/yyyy")),
                FileName = service.FileName,
                FileNamesPath = fileNamesPath,
            };

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }

        public void SendMailToCreditTermNormalGroup()
        {
            if (AppSettings.FunctionIdCreditTerm > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdCreditTerm, _mailInfo);
            }
        }

        public void SendMailToSuperBalanceNormalGroup()
        {
            if (AppSettings.FunctionIdSuperBalance > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdSuperBalance, _mailInfo);
            }
        }

        public void SendMailToDirectBalanceNormalGroup()
        {
            if (AppSettings.FunctionIdDirectBalance > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdDirectBalance, _mailInfo);
            }
        }

        public Mail SendMailSuperBalanceToVIPGroup()
        {
            var superCriteria = new SuperBalanceReportCriteria
            {
                Date = DateTime.Now,
                Name = "Super_Balance_Report",
                Level = "Super",

                DataDirectory = AppSettings.DataDirectoryDirectBalance,
                DataDirectoryWithYear = AppSettings.DataDirectoryWithYearSuperBalance,
                ExcelPasswordProtected = AppSettings.ExcelPasswordProtectedSuperBalance,
                FunctionId = AppSettings.FunctionIdSuperBalance
            };

            IBalanceService service = new BalanceService(superCriteria);
            service.ExportExcelBalance();

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(service.DirectoryOfFileName);

            _mailInfo = new MailInfo()
            {
                Subject = string.Format(AppSettings.SuperBalanceSubject, DateTime.Now.ToString("MM/yyyy")),
                FileName = service.FileName,
                FileNamesPath = fileNamesPath,
            };

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public Mail SendMailDirectBalanceToVIPGroup()
        {
            var masterCriteria = new DirectBalanceReportMasterCriteria
            {
                Date = DateTime.Now,
                Name = "Direct_Master_Report",
                Level = "Master",

                DataDirectory = AppSettings.DataDirectoryDirectBalance,
                DataDirectoryWithYear = AppSettings.DataDirectoryWithYearDirectBalance,
                ExcelPasswordProtected = AppSettings.ExcelPasswordProtectedDirectBalance,
                FunctionId = AppSettings.FunctionIdDirectBalance
            };

            var agentCriteria = new DirectBalanceReportAgentCriteria
            {
                Date = DateTime.Now,
                Name = "Direct_Agent_Report",
                Level = "Agent",

                DataDirectory = AppSettings.DataDirectoryDirectBalance,
                DataDirectoryWithYear = AppSettings.DataDirectoryWithYearDirectBalance,
                ExcelPasswordProtected = AppSettings.ExcelPasswordProtectedDirectBalance,
                FunctionId = AppSettings.FunctionIdDirectBalance
            };

            var memberCriteria = new DirectBalanceReportMemberCriteria
            {
                Date = DateTime.Now,
                Name = "Direct_Member_Report",
                Level = "Member",

                DataDirectory = AppSettings.DataDirectoryDirectBalance,
                DataDirectoryWithYear = AppSettings.DataDirectoryWithYearDirectBalance,
                ExcelPasswordProtected = AppSettings.ExcelPasswordProtectedDirectBalance,
                FunctionId = AppSettings.FunctionIdDirectBalance
            };

            var fileNamesPath = new List<string>();
            var fileNames = new List<string>();

            IBalanceService service = new BalanceService(masterCriteria);
            service.ExportExcelBalance();
            fileNamesPath.Add(service.DirectoryOfFileName);
            fileNames.Add(service.FileName);

            service = new BalanceService(agentCriteria);
            service.ExportExcelBalance();
            fileNamesPath.Add(service.DirectoryOfFileName);
            fileNames.Add(service.FileName);

            service = new BalanceService(memberCriteria);
            service.ExportExcelBalance();
            fileNamesPath.Add(service.DirectoryOfFileName);
            fileNames.Add(service.FileName);

            _mailInfo = new MailInfo()
            {
                Subject = string.Format(AppSettings.DirectBalanceSubject, DateTime.Now.ToString("MM/yyyy")),
                FileName = string.Join(", ", fileNames),
                FileNamesPath = fileNamesPath,
            };

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }
    }
}