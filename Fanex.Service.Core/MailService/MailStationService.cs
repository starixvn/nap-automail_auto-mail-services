// -----------------------------------------------------------------------
// <copyright file="MailService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.MailService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Utilities;
    using MailDelivery.Client;
    using MailDelivery.Core.Models;
    using RazorEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailStationService : IMailStationService
    {
        public Mail CreateMailWithAttachedFile(MailInfo mailInfo)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + MailTemplateConstants.AttachedFileEmail;
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, mailInfo);

            Mail mail = new Mail()
            {
                Subject = mailInfo.Subject,
                Title = AppSettingBase.Title,
                Body = emailContent,
                Attachments = new List<MailAttachment>()
            };

            foreach (string fileNamePath in mailInfo.FileNamesPath)
            {
                mail.Attachments.Add(new MailAttachment(fileNamePath, FileExtension.MS_EXCEL));
            }

            return mail;
        }

        public Mail CreateMailWithNotification(MailInfo mailInfo)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + MailTemplateConstants.NotificationEmail;
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, mailInfo);

            Mail mail = new Mail()
            {
                Subject = mailInfo.Subject,
                Title = AppSettingBase.Title,
                Body = emailContent
            };

            return mail;
        }

        public void SendFunctionMailWithAttachedFile(int functionId, MailInfo mailInfo)
        {
            var mail = CreateMailWithAttachedFile(mailInfo);
            mail.MailingListId = functionId;

            SendEmail(mail);
        }

        public void SendFunctionMailWithNotification(int functionId, MailInfo mailInfo)
        {
            var mail = CreateMailWithNotification(mailInfo);
            mail.MailingListId = functionId;

            SendEmail(mail);
        }

        public Mail CreateMailCustomContentWithAttachedFile(MailInfo mailInfo, string bodyContent)
        {
            Mail mail = new Mail()
            {
                Subject = mailInfo.Subject,
                Title = AppSettingBase.Title,
                Body = bodyContent,
                Attachments = new List<MailAttachment>()
            };

            foreach (string fileNamePath in mailInfo.FileNamesPath)
            {
                mail.Attachments.Add(new MailAttachment(fileNamePath, FileExtension.MS_EXCEL));
            }

            return mail;
        }

        public Mail CreateMailCustomContent(MailInfo mailInfo, string bodyContent)
        {
            Mail mail = new Mail()
            {
                Subject = mailInfo.Subject,
                Title = AppSettingBase.Title,
                Body = bodyContent,
                Attachments = new List<MailAttachment>()
            };

            return mail;
        }

        public void SendFunctionMailCustomContentNotification(int functionId, MailInfo mailInfo, string bodyContent)
        {
            Mail mail = new Mail()
            {
                Subject = mailInfo.Subject,
                Title = AppSettingBase.Title,
                Body = bodyContent,
                Attachments = new List<MailAttachment>(),
                MailingListId = functionId
            };

            SendEmail(mail);
        }

        private void SendEmail(Mail mail)
        {
            MailClientHandler.Start();
            MailClientHandler.SendFunctionMail(mail);
            MailClientHandler.Stop();
        }
    }
}