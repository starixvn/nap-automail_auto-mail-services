// -----------------------------------------------------------------------
// <copyright file="DailyBettypeWith3rdProductJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Repositories;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyBettypeWith3rdProductJob : IDailyBettypeWith3rdProductJob
    {
        private IMailStationService _mailStation;
        private Fanex.Service.Core.Model.MailInfo _mailInfo;
        private IRepository _repository;

        public DailyBettypeWith3rdProductJob()
        {
            _mailStation = new MailStationService();
            _mailInfo = new Fanex.Service.Core.Model.MailInfo();
            _repository = new Repository();
        }

        public MailResult ExportBettypeWith3rdProducts(bool IsDaily, string Subject)
        {
            MailResult mailResult = new MailResult();

            DailyBettypeWith3rdProductsCriteria criteria = new DailyBettypeWith3rdProductsCriteria();
            var lastDate = _repository.GetLastDateDataAvaiable();
            if (criteria.DateEnd != lastDate)
            {
                mailResult.JobStatus = JobStatus.DataIsNotReady;
                SendFunctionMailCustomContentNotification(Subject);
            }
            else
            {
                var mail = SendEmail(IsDaily, Subject);
                mailResult.Mail = mail;
            }

            return mailResult;
        }

        public Mail SendEmail(bool IsDaily, string Subject)
        {
            Fanex.DW.DailyBettypeWith3rdProducts.Business.Model.MailInfo info = new Fanex.DW.DailyBettypeWith3rdProducts.Business.Model.MailInfo(IsDaily);
            DailyBettypeWith3rdProductsService service = new DailyBettypeWith3rdProductsService();
            service.GenerateReport(false, info.ExportDataDirectory);
            
            string subject = Subject + " - " + DateTime.Now.ToString("yyyy-MM-dd");

            Mail mail = new Mail()
            {
                Subject = subject,
                Title = subject,
                Body = info.DailyBettype3rdProductMailBody
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            service.GenerateReport(true, info.ExportDataDirectory);
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            return mail;
        }

        private void SendFunctionMailCustomContentNotification(string Subject)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + @"SiteConfig\DailyBettypeWith3rdProducts\Templates\NotificationEmailTemplate.cshtml";
            string template = File.ReadAllText(emailTemplatePath);

            _mailInfo.FileName = Subject;
            _mailStation.SendFunctionMailCustomContentNotification(AppSettings.NotificationEmailFunctionId, _mailInfo, template);
        }
    }
}
