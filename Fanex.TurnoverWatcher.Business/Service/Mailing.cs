// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Fanex.Service.Core.Extensions;
    using Fanex.TurnoverWatcher.Business.Model;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        public const string TurnoverWatcherEmailTemplate = @"/Templates/EmailTemplate.cshtml";
        public const string NotificationEmailTemplate = @"/Templates/NotificationEmailTemplate.cshtml";

        /// <summary>
        /// Sends the turnover watcher email.
        /// </summary>
        /// <param name="turnoverInfo">The turnover information.</param>
        /// <param name="fromPercentage">From percentage.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns>Mail was sent success or fail.</returns>
        public static bool SendTurnoverWatcherEmail(List<TurnoverInfo> turnoverInfo, int fromPercentage, EmailReceiver receiver)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                var mailTimezone = timezone > 0 ? ("+" + timezone) : timezone.ToString(CultureInfo.InvariantCulture);

                string filePath = root + TurnoverWatcherEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, turnoverInfo);

                var emailSubject = string.Format(
                      AppSettings.MailSubject,
                      fromPercentage.ToString(),
                      DateTime.Now,
                      mailTimezone);

                string emailTitle = AppSettings.MailTitle;

                var mailItem = new Fanex.MailDeliver.Model.MailRecord
                {
                    MailTo = receiver.To,
                    MailCc = receiver.Cc,
                    MailBcc = receiver.Bcc,
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
                throw;
            }

            return isSuccess;
        }

        /// <summary>
        /// Errors the email notification.
        /// </summary>
        /// <param name="fromPercentage">From percentage.</param>
        /// <param name="result">The result.</param>
        public static void ErrorEmailNotification(int fromPercentage, Result result)
        {
            try
            {
                var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                var mailTimezone = timezone > 0 ? ("+" + timezone) : timezone.ToString(CultureInfo.InvariantCulture);
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + NotificationEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, result);
                string emailTo = AppSettings.AdminEmail;
                var emailSubject = string.Format(
                      AppSettings.MailTo,
                      fromPercentage.ToString(),
                      DateTime.Now,
                      mailTimezone);
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