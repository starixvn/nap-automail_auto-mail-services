// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Template.Business.Model;
    using Fanex.Template.Business.Constants;
    using System.IO;
    using Fanex.Template.Business.Common;
    using RazorEngine;
    using Fanex.Template.Business.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        /// <summary>
        /// Wins the loss by product email notification.
        /// </summary>
        /// <param name="criteria">The win loss by products criteria.</param>
        /// <returns>Email will be send success or not.</returns>
        public static bool ResetWinningEmailNotification(SMAReportModel model, Result result)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.ResetWinningNotificationTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, result); 
                string emailTo = AppSettings.AdminEmail;
                string emailSubject = string.Format(AppSettings.MailSubject, model.ResetOnDate);
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
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool ResetWinningSuccess(SMAReportModel model)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + EmailTemplateConstants.ResetWinningTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, model);
                string emailTo = AppSettings.AdminEmail;
                string emailSubject = string.Format(AppSettings.MailSubject, model.ResetOnDate);
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
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
