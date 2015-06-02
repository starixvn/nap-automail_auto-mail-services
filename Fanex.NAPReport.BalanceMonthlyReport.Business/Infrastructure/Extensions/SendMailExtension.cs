using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Configurations;
using Fanex.MailDeliver.Model;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Extensions
{
    public class SendMailExtension
    {
        #region private variables
        private static BackgroundWorker _worker = null;
        private static Queue<MemoryStream> _queueAttachFile = new Queue<MemoryStream>();
        private static Queue<MailSection.HostName> _queueHostName = new Queue<MailSection.HostName>();
        private static Queue<string> _queueTo = new Queue<string>();
        private static Queue<string> _queueCc = new Queue<string>();
        private static Queue<string> _queueMailTile = new Queue<string>();
        private static Queue<string> _queueSubject = new Queue<string>();
        private static Queue<string> _queueBody = new Queue<string>();
        private static Queue<string> _queueFileName = new Queue<string>();
        #endregion
        /**
         * Function: SendMail Handler
         */
        public static void SendMailHandler(string to, string cc, string bcc, string mailTitle, 
            string subject, string body, MemoryStream attachFile, string fileName)
        {
            SendMail(to, cc, bcc, mailTitle,subject, body, attachFile, fileName);
        }
        /**
         * Function: Backgroundworker running
         */
       
        /**
         * Function: Backgroundworker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO
        }

        public static void SendMail(string to, string cc, string bcc, string mailTitle, string subject, string body, MemoryStream attachFile, string fileName)
        {
            var listHostname = GetListHostByPriority();

            var attachFiles = new List<MemoryStream>{attachFile};
            var fileNames = new List<string>{fileName};

            if (listHostname.Count > 0)
            {
                try
                {
                    SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
                }
                catch (Exception)
                {
                    if (listHostname.Count > 1)
                    {
                        try
                        {
                            SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
                        }
                        catch (Exception)
                        {
                            if (listHostname.Count > 2)
                            {
                                try
                                {
                                    SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
                                }
                                catch (Exception ex)
                                {
                                    Helper.Logger.WriteLog(ex.Message);
                                    Helper.Logger.WriteLog(ex.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
            }
        }

        public static void SendMail(string to, string cc, string bcc, string mailTitle, string subject, string body, List<MemoryStream> attachFiles, List<string> fileNames)
        {
            var listHostname = GetListHostByPriority();
            if (listHostname.Count > 0)
            {
                try
                {
                    SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
                }
                catch (Exception ex)
                {


                    Helper.Logger.WriteLog(ex.Message);
                    Helper.Logger.WriteLog(ex.StackTrace);

                }
            }
            else
            {
                SendMailWithAttachMultiFile(to, cc, bcc, mailTitle, subject, body, attachFiles, fileNames);
            }
        }

        private static void SendMailWithAttachMultiFile(string to, string cc, string bcc, string mailTitle, string subject, string body, List<MemoryStream> attachFiles, List<string> fileNames)
        {
            try
            {
                using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                {


                    var mailSend = new MailSend()
                    {
                        To = to,
                        Cc = cc,
                        Bcc = bcc,
                        Title = mailTitle,
                        Subject = subject,
                        Body = body
                    };

                    

                    var mailItem = new MailRecord
                    {
                        MailTo = mailSend.To,
                        MailCc = mailSend.Cc,
                        MailBcc = mailSend.Bcc,
                        Title = mailSend.Title,
                        Subject = mailSend.Subject,
                        Body = mailSend.Body
                    };
                    if ((attachFiles != null && attachFiles.Count > 0) && (fileNames != null && fileNames.Count > 0))
                    {
                        mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                        int index = -1;
                        foreach (var attachFile in attachFiles)
                        {
                            ++index;
                            var attItem = new Fanex.MailDeliver.Model.MailAttachment
                            {
                                FileName = fileNames[index],
                                Content = attachFile
                            };
                            attItem.ContentType = FileExtentions.MS_EXCEL;
                            mailItem.Attachments.Add(attItem);
                        }


                        
                    }
                    
                    // send mail
                    client.SendMail(mailItem);
                }

                
            }
            catch (Exception ex)
            {
                //LogExtensions.ExceptionHandler(ex);
            }




        }

        private static List<string> GetListHostByPriority()
        {
            var listHostName = new List<string>();
            var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "MailSection.config");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileInfo.FullName);
            var serverElement = xmlDocument.GetElementsByTagName("Server")[0];

            int priority = 0;
            while (listHostName.Count != serverElement.ChildNodes.Count)
            {
                if (serverElement.ChildNodes.Count > 0)
                        if (serverElement.ChildNodes[0].Attributes["order"] == null) break;
                foreach (XmlNode xmlNode in serverElement)
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes["order"] != null)
                        if (xmlNode.Attributes["order"].Value == priority.ToString())
                        {
                            listHostName.Add(xmlNode.Attributes["name"].Value);
                            ++priority;
                        }
                }
            }
            return listHostName;
        }
    }

    public class MailSend
    {
        public MailSection.HostName HostName { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailAttachment Attachment { get; set; }
        public AlternateView AltView { get; set; }

    }

    public class MailAttachment
    {
        public MemoryStream Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
