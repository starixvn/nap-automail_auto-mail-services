// -----------------------------------------------------------------------
// <copyright file="DailyBettypeWith3rdProductJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Repositories;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingService
    {
        private IMailStationService _mailStation;
        private Fanex.Service.Core.Model.MailInfo _mailInfo;
        private IRepository _repository;
        private IExportExcelService _excel;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BettypeWith3rdProducts\Settings\Bettype_With_3rd.Settings.xml");
            configurationParser.Parse<AppSettings>();

            _mailStation = new MailStationService();
            _mailInfo = new Fanex.Service.Core.Model.MailInfo();
            _repository = new Repository();
            _excel = new ExportExcelService();
        }

        public MailResult ExportBettypeWith3rdProducts(bool isDaily, string subject)
        {
            MailResult mailResult = new MailResult();

            DailyBettypeWith3rdProductsCriteria criteria = new DailyBettypeWith3rdProductsCriteria();

            _mailInfo.Subject = isDaily ? 
                string.Format(
                    AppSettings.SubjectDaily,
                    DateTime.Now.ToString("yyyy/MM/dd")) :
                string.Format(
                    AppSettings.SubjectWeekly,
                    criteria.DateStart.ToString("dd MMM"),
                    criteria.DateEnd.ToString("dd MMM,yyyy"));

            var lastDate = _repository.GetLastDateDataAvaiable();

            bool isNotifyEmail = criteria.DateEnd != lastDate && AppSettings.IsCheckEnoughData;
            if (isNotifyEmail)
            {
                mailResult.JobStatus = JobStatus.DataIsNotReady;
                SendFunctionMailCustomContentNotification(subject);
            }
            else
            {
                var mail = SendEmail(isDaily, criteria);
                mailResult.Mail = mail;

                SendMailToNormalGroup(isDaily);
            }

            return mailResult;
        }

        public Mail SendEmail(bool isDaily, DailyBettypeWith3rdProductsCriteria criteria)
        {
            if (AppSettings.IsSingleFile)
            {
                _excel.ExportExcelCombineRealAndVirtual(isDaily);                
            }
            else
            {
                _excel.ExportExcelSeparateRealAndVirtual(false, isDaily);                
                _excel.ExportExcelSeparateRealAndVirtual(true, isDaily);                
            }
            
            _mailInfo.FileName = string.Join(", ", _excel.FileNames);
            _mailInfo.FileNamesPath = _excel.DirectoryOfFileNames;
            
            var mail = _mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }

        private void SendFunctionMailCustomContentNotification(string subject)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + @"SiteConfig\BettypeWith3rdProducts\Templates\NotificationEmailTemplate.cshtml";
            string template = File.ReadAllText(emailTemplatePath);

            _mailInfo.FileName = subject;
            _mailStation.SendFunctionMailCustomContentNotification(AppSettings.NotificationEmailFunctionId, _mailInfo, template);
        }

        private void SendMailToNormalGroup(bool isDaily)
        {
            if (isDaily)
            {
                if (AppSettings.FunctionIdDaily > 0)
                {
                    _mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdDaily, _mailInfo);
                }
            }
            else
            {
                if (AppSettings.FunctionIdWeekly > 0)
                {
                    _mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdWeekly, _mailInfo);
                }
            }
        }
    }
}
