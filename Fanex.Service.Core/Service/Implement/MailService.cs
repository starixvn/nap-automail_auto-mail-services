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
    using Fanex.Service.Core.Extensions;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailService : IMailService
    {
        public Mail CreateMail(string subject, string title, string emailContent, List<MailAttachment> listAttachment, string functionName)
        {
            Mail mail = new Mail()
            {
                Subject = subject,
                Title = title,
                Body = emailContent,
                Attachments = listAttachment
            };

            mail.MailingListId = mailStationAppSettings.GetFunctionId(functionName);
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