// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Service
{
    using System;
    using Fanex.HorseRacingReport.Business.Criteria;
    using Fanex.HorseRacingReport.Business.Constants;
    using System.IO;
    using RazorEngine;
    using Fanex.Service.Core.Extensions;
    using Fanex.MailDeliver.Model;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        /// <summary>
        /// Sends the SMA report email.
        /// </summary>
        /// <param name="fileNames">The file name.</param>
        /// <param name="reportFrom">The report from.</param>
        /// <param name="reportTo">The report to.</param>
        /// <returns>Email will be send success or not.</returns>
        public static bool SendRacingReportEmail(string reportUrl, TimeRangeCriteria criteria)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.RacingReportEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, criteria);
                string emailTo = AppSettings.MailTo;
                string emailSubject = string.Format(
                    AppSettings.MailSubject,
                    criteria.FromDate.ToShortFormatedDateWithSlash(),
                    criteria.ToDate.ToShortFormatedDateWithSlash());
                string emailTitle = AppSettings.MailTitle;

                var mailItem = new Fanex.MailDeliver.Model.MailRecord
                {
                    MailTo = emailTo,
                    MailCc = AppSettings.MailCcTo,
                    MailBcc = AppSettings.MailBCcTo,
                    Subject = emailSubject,
                    Title = emailTitle,
                    Body = emailContent
                };

                mailItem.Attachments = new List<MailAttachment>();
                var attachmentItem = new Fanex.MailDeliver.Model.MailAttachment(reportUrl, Fanex.MailDeliver.Model.FileExtentions.TEXT);
                mailItem.Attachments.Add(attachmentItem);

                if (!string.IsNullOrEmpty(mailItem.MailTo.Trim()))
                {
                    using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                    {
                        client.SendMail(mailItem);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                isSuccess = false;
                throw;
            }

            return isSuccess;
        }

        /// <summary>
        /// Errors the email notification.
        /// </summary>
        /// <param name="reportFrom">The report from.</param>
        /// <param name="reportTo">The report to.</param>
        public static void ErrorEmailNotification(TimeRangeCriteria criteria)
        {
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.RacingReportEmailNotificationTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template);
                string emailTo = AppSettings.AdminEmail;
                string emailSubject = string.Format(
                    AppSettings.MailSubject,
                    criteria.FromDate.ToShortFormatedDateWithSlash(),
                    criteria.ToDate.ToShortFormatedDateWithSlash());
                string emailTitle = AppSettings.MailTitle;

                var mailItem = new Fanex.MailDeliver.Model.MailRecord
                {
                    MailTo = emailTo,
                    Subject = emailSubject,
                    Title = emailTitle,
                    Body = emailContent
                };

                if (!string.IsNullOrEmpty(mailItem.MailTo.Trim()))
                {
                    using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                    {
                        client.SendMail(mailItem);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}