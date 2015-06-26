// -----------------------------------------------------------------------
// <copyright file="Mailing.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.NAPReport.VirtualSportWinLoss.Business;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Mailing
    {
        public const string VirtualSportEmailTemplate = @"/Templates/EmailTemplate.cshtml";

        /// <summary>
        /// Mailings the win loss by virtual sport.
        /// </summary>
        /// <param name="fileAttachment">The file attachment.</param>
        /// <returns>Send email is success or not.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public static bool MailingWinLossByVirtualSport(string fileAttachment)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + VirtualSportEmailTemplate;
                var yesterday = DateTime.Now.AddDays(-1);
                var startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);

                var periodReportTime = new PeriodTimeReport();
                periodReportTime.FromDate = startMonth.ToYearMonthDayFormat().Replace("/", "-");
                periodReportTime.ToDate = yesterday.ToYearMonthDayFormat().Replace("/", "-");
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, periodReportTime);

                string emailTo = AppSettings.MailTo;
                string emailSubject = string.Format(
                    AppSettings.MailSubject,
                    DateTime.Now.ToYearMonthDayFormat().Replace("/", "-"));
                string emailTitle = AppSettings.MailTitle;
                string emailCcTo = AppSettings.MailCcTo;
                string emailBCcTo = AppSettings.MailBCcTo;

                //var mailItem = new Fanex.MailDeliver.Model.MailRecord
                //{
                //    MailTo = emailTo,
                //    MailCc = emailCcTo,
                //    MailBcc = emailBCcTo,
                //    Subject = emailSubject,
                //    Title = emailTitle,
                //    Body = emailContent
                //};

                //mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                //var attachmentItem = new Fanex.MailDeliver.Model.MailAttachment(@fileAttachment, Fanex.MailDeliver.Model.FileExtentions.MS_EXCEL);
                //mailItem.Attachments.Add(attachmentItem);

                //if (!string.IsNullOrEmpty(mailItem.MailTo.Trim()))
                //{
                //    using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                //    {
                //        client.SendMail(mailItem);
                //    }
                //}
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