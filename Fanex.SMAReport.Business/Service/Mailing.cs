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
using Fanex.MailDelivery.Client.Models;

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
        public static Mail SendSMAReportEmail(List<string> fileNames, string reportFrom, string reportTo)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettings.EmailTemplate);
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, fileNames);
            string emailSubject = string.Format(AppSettings.MailSubject, reportFrom, reportTo);
            string emailTitle = AppSettings.MailTitle;

            return new Mail
            {
                Subject = emailSubject,
                Title = emailTitle,
                Body = emailContent
            };
        }
    }
}