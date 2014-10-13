// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Service.Core.Extensions;
    using Fanex.SMAReport.Business.Constants;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        /// <summary>
        /// Sends the SMA report email.
        /// </summary>
        /// <param name="fileAttachments">The file attachments.</param>
        /// <param name="reportFrom">The report from.</param>
        /// <param name="reportTo">The report to.</param>
        /// <returns>Email will be send success or not.</returns>
        public static bool SendSMAReportEmail(List<string> fileAttachments, string reportFrom, string reportTo)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.SMAReportEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template);
                string emailTo = AppSettings.MailTo;
                string emailSubject = string.Format(AppSettings.MailSubject, reportFrom, reportTo);
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

                mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                foreach (var fileAttachment in fileAttachments)
                {
                    var attachmentItem = new Fanex.MailDeliver.Model.MailAttachment(
                        @fileAttachment,
                        Fanex.MailDeliver.Model.FileExtentions.MS_EXCEL);
                    mailItem.Attachments.Add(attachmentItem);
                }

                if (!string.IsNullOrEmpty(mailItem.MailTo.Trim()))
                {
                    using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                    {
                        System.Threading.Thread.Sleep(300000);
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
        public static void ErrorEmailNotification(string reportFrom, string reportTo)
        {
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.SMAReportEmailNotificationTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template);
                string emailTo = AppSettings.AdminEmail;
                string emailSubject = string.Format(AppSettings.MailSubject, reportFrom, reportTo);
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