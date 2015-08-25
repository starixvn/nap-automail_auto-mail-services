// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.GameWinLimitControl.Business.Criteria;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingService
    {
        private const string GameWinLimitEmailTemplate = @"SiteConfig\GameWinLimitControl\Templates\GameWinLimitControlEmailTemplate.cshtml";
        private IGameWinLimitService _gameWinLimitService;
        private IGameWinLimitExcelService _excelService;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\GameWinLimitControl\Settings\GameWinLimitControl.Settings.xml");
            configurationParser.Parse<AppSettings>();            
            _gameWinLimitService = new GameWinLimitService();
            _excelService = new GameWinLimitExcelService();        
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();

            SelMemberWinLimitCriteria criteria = new SelMemberWinLimitCriteria();
            var members = _gameWinLimitService.GetMemberWinLimit(criteria);

            if (members.Count == 0)
            {
                return new MailResult() 
                { 
                    JobStatus = JobStatus.SuccessWithoutSendmail,
                };
            }

            if (AppSettings.IsCallFPSBlockMember)
            {
                var blockMembersResult = _gameWinLimitService.BlockMembers(members);
                if (blockMembersResult.ErrCode > 0)
                {
                    LogExtensions.LogInfo(blockMembersResult.ErrMsg);
                }
            }
            
            _excelService.ExportReportToExcel(members, criteria);
            var result = _gameWinLimitService.UdpMemberWinLimit(members);
            if (result.ErrCode != 0)
            {
                return new MailResult()
                {
                    JobStatus = JobStatus.Failed,
                };
            }

            Mail mail = CreateMail(members);
            mailResult.Mail = mail;

            return mailResult;
        }

        private Mail CreateMail(List<GameWinLimit> members)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + GameWinLimitEmailTemplate;
            string template = File.ReadAllText(emailTemplatePath);
            var bodyContent = Razor.Parse(template, members);
            DateTime dt = DateTime.Now;
            string fromDate = dt.AddSeconds(-AppSettings.IntervalInSecond).ToString(DateTimeFormatConstants.YearMonthDateMinute2);
            string toDate = dt.ToString(DateTimeFormatConstants.YearMonthDateMinute2);

            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettings.Subject, fromDate, toDate),
                Title = AppSettings.Title,
                Body = bodyContent,
                Attachments = new List<MailAttachment>(),
            };

            mail.Attachments.Add(new MailAttachment(_excelService.DirectoryOfFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            return mail;
        }

        public void SendMailToNormalGroup()
        {
        }        
    }
}
