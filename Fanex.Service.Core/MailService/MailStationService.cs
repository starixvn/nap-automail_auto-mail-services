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
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Utilities;
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
                Subject = string.Format(AppSettingBase.Subject, mailInfo.FileName),
                Title = AppSettingBase.Title,
                Body = emailContent,
                Attachments = new List<MailAttachment>()
            };

            foreach (string fileNamePath in mailInfo.FileNamesPath)
            {                
                mail.Attachments.Add(new MailAttachment(fileNamePath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
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
                Subject = string.Format(AppSettingBase.Subject, mailInfo.FileName),
                Title = AppSettingBase.Title,
                Body = emailContent
            };

            return mail;
        }

        public void SendFunctionMailWithAttachedFile(int functionId, MailInfo mailInfo)
        {
            var mail = CreateMailWithAttachedFile(mailInfo);
            mail.MailingListId = functionId;

            MailClientHandler.SendMail(mail);
        }

        public void SendFunctionMailWithNotification(int functionId, MailInfo mailInfo)
        {
            var mail = CreateMailWithNotification(mailInfo);
            mail.MailingListId = functionId;

            MailClientHandler.SendMail(mail);
        }

        public Mail CreateMailCustomContentWithAttachedFile(MailInfo mailInfo, string bodyContent)
        {
            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettingBase.Subject, mailInfo.FileName),
                Title = AppSettingBase.Title,
                Body = bodyContent, 
                Attachments = new List<MailAttachment>()
            };

            foreach (string fileNamePath in mailInfo.FileNamesPath)
            {
                mail.Attachments.Add(new MailAttachment(fileNamePath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            }

            return mail;
        }

        public Mail CreateMailCustomContent(MailInfo mailInfo, string bodyContent)
        {
            Mail mail = new Mail()
            {
                Subject = string.Format(AppSettingBase.Subject, mailInfo.FileName),
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
                Subject = string.Format(AppSettingBase.Subject, mailInfo.FileName),
                Title = AppSettingBase.Title,
                Body = bodyContent,
                Attachments = new List<MailAttachment>(),
                MailingListId = functionId
            };

            MailClientHandler.SendMail(mail);
        }
    }
}
