// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsMailService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.Service.Core.Extensions;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsMailService : IWinLossByProductsMailService
    {
        public Mail CreateMail(string emailContent, string attachReportPath, GetWinLossByProductsCriteria criteria)
        {
            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettings.Subject, criteria.WinLossDate.ToShortFormatedDateWithHourMinute()),
                Title = AppSettings.Title,
                Body = emailContent
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(attachReportPath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            mail.MailingListId = mailStationAppSettings.GetFunctionId("Win Loss by Product Mail Function");
            return mail;
        }

        public void SendMailByFunction(Mail mail)
        {
            MailClientHandler.Start();
            MailClientHandler.SendMail(mail);
            MailClientHandler.Stop();
        }
    }
}