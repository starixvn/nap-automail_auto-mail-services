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
    using Fanex.NAPReport.WinLossByProducts.Business.Constants;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using MailDelivery.Core.Models;
    using RazorEngine;
    using Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService 
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;
        private IWinLossByProductsService _service;
        private IWinLossByProductsExportDataService _exportDataService;
        private bool isEnoughData = false;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\OverallWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();

            _service = new WinLossByProductsService();
            _exportDataService = new WinLossByProductsExportDataService();
        }

        public JobMessage Execute()
        {
            JobMessage jobMessage = new JobMessage();
            var message = string.Empty;
            var mail = new Mail();
            try
            {
                GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
                {
                    WinLossDate = DateTime.Now.AddDays(-1)
                };

                _mailInfo.Subject = string.Format(AppSettings.SubjectProductWL, DateTime.Now.ToString("yyyy/MM/dd"));

                var report = _service.GetWinLossByProductsData(criteria);
                isEnoughData = report.IsEnoughData;

                if (!AppSettings.IsCheckEnoughData)
                {
                    isEnoughData = true;
                }

                if (!isEnoughData)
                {
                    message = "DataIsNotReady";                    
                    SendFunctionMailCustomContentNotification();                    
                }
                else
                {
                    mail = SendNotification(criteria, report);
                    SendMailToNormalGroup();
                }
            }
            catch(Exception exception)
            {
                message = exception.Message;
                LogExtensions.ExceptionHandler(exception);
            }
            finally
            {
                jobMessage.JobStatus = isEnoughData ? JobStatus.Success : JobStatus.DataIsNotReady;
                jobMessage.Mail = mail;
                jobMessage.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = message
                };
            }

            return jobMessage;
        }

        private Mail SendNotification(GetWinLossByProductsCriteria criteria, WinLossByProducts report)
        {
            _exportDataService.ExportReportToExcel(report, criteria);
            string reportPath = _exportDataService.DirectoryOfFileName;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants._WIN_LOSS_BY_PRODUCTS_TEMPLATE;
            string template = File.ReadAllText(emailTemplatePath);

            WinLossProductsDto data = new WinLossProductsDto();
            data.WinLossByProducts = report;

            var bodyContent = Razor.Parse(template, data);

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(_exportDataService.DirectoryOfFileName);

            _mailInfo.FileName = _exportDataService.FileName;
            _mailInfo.FileNamesPath = fileNamesPath;

            var mail = mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, bodyContent);

            return mail;
        }

        private void SendMailToNormalGroup()
        {
            if (isEnoughData && AppSettings.FunctionId > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }

        private void SendFunctionMailCustomContentNotification()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string emailTemplatePath = root + EmailTemplateConstants._NOTIFICATION_EMAIL_TEMPLATE;
            string template = File.ReadAllText(emailTemplatePath);

            _mailInfo.FileName = "Win Loss By Products";
            mailStation.SendFunctionMailCustomContentNotification(Service.Core.Utilities.AppSettingBase.NotificationEmailFunctionId, _mailInfo, template);
        }
    }
}