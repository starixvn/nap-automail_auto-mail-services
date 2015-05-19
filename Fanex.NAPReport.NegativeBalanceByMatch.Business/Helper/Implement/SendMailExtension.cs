using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using Fanex.MailDeliver.Model;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Implement
{
    public class SendMailExtension
    {
        #region private variables
        private static BackgroundWorker _worker = null;
        private static Queue<MailSend> _queueMail = new Queue<MailSend>();
        private static bool _sendMailStatus = true;
        #endregion

        /**
         * Function: Backgroundworker running
         */
        private static void WorkerRunning(object sender, DoWorkEventArgs e)
        {
            if (_queueMail.Count > 0)
            {
                var queueMail = _queueMail.Dequeue();
                e.Result = SendMail(queueMail);
            }
            else
            {
                _worker.CancelAsync();
            }
        }

        public static bool SendMail(MailSend mailSend)
        {
            try
            {
                using (var client = new MailDeliver.MailClient.API.MailDeliver())
                {
                    var mailItem = new Fanex.MailDeliver.Model.MailRecord
                    {
                        MailTo = mailSend.To,
                        MailCc = mailSend.Cc,
                        MailBcc = mailSend.Bcc,
                        Title = mailSend.Title,
                        Subject = mailSend.Subject,
                        Body = mailSend.Body
                    };
                    if (mailSend.Attachment != null && mailSend.Attachment.FileName != null)
                    {
                        mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                        var attItem = new Fanex.MailDeliver.Model.MailAttachment
                        {
                            FileName = mailSend.Attachment.FileName,
                            Content = mailSend.Attachment.Content
                        };
                        if (mailSend.Attachment.ContentType.ToLower().IndexOf("excel") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_EXCEL;
                        }
                        else if (mailSend.Attachment.ContentType.ToLower().IndexOf("doc") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_DOC;
                        }
                        else
                        {
                            attItem.ContentType = FileExtentions.IMG_GIF;
                        }
                        mailItem.Attachments.Add(attItem);
                    }
                    if (mailSend.AltView != null)
                    {
                        mailItem.AlternateView = new List<MailLinkedResource>();
                        var linkedResource = mailSend.AltView.LinkedResources;
                        foreach (var linked in linkedResource)
                        {
                            var stream = linked.ContentStream;
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var resource = new MailLinkedResource
                            {
                                FileName = string.Empty,
                                ImageType = FileExtentions.IMG_GIF,
                                ContentCID = linked.ContentId,
                                StreamContent = new MemoryStream(buffer)
                            };
                            mailItem.AlternateView.Add(resource);
                        }
                    }
                    // send mail
                    client.SendMail(mailItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHandler.Log(ex);
                return false;
            }
        }
        /**
         * Function: Backgroundworker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _sendMailStatus = (bool)e.Result;
        }

        public static bool AutoSendMail(string to, string cc, string bcc, string mailTitle, string subject, string body, MailAttachment mailAttachment)
        {
            try
            {

                var mailSend = new MailSend()
                                   {
                                       //HostName = hostName,
                                       To = to,
                                       Cc = cc,
                                       Bcc = bcc,
                                       Title = mailTitle,
                                       Subject = subject,
                                       Body = body,
                                       Attachment = null,
                                       AltView = null
                                   };
               // _queueMail.Enqueue(mailSend);
                SendMail(mailSend);

                /* using (_worker = new BackgroundWorker())
                {
                    _worker.WorkerSupportsCancellation = true;
                    _worker.DoWork += new DoWorkEventHandler(WorkerRunning);
                    _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
                    _worker.RunWorkerAsync();

                    return true;
                }*/
            }
            catch (Exception ex)
            {
                LogHandler.Log(ex);
            }
            return false;

        }
    }

    public class MailSend
    {
        //public string HostName { get; set; }
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
