// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.HorseRacingReport.Business.Constants;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        /// <summary>
        /// Sends the racing report email.
        /// </summary>
        /// <param name="reportUrl">The report URL.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Email sent success or fail.</returns>
        public static Mail SendCreditMovementReportEmail(string reportUrl, GetCreditMovementCriteria criteria)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettings.EmailTemplate);
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, criteria);
            string emailSubject = string.Format(
                AppSettings.MailSubject,
                criteria.FromDate.ToString("yyyy-MM-dd"),
                criteria.ToDate.ToString("yyyy-MM-dd"));
            string emailTitle = AppSettings.MailTitle;

            var listAttachment = new List<MailAttachment>();
            listAttachment.Add(new MailAttachment(reportUrl, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            return new Mail
            {
                Subject = emailSubject,
                Title = emailTitle,
                Body = emailContent,
                Attachments = listAttachment
            };
        }
    }
}