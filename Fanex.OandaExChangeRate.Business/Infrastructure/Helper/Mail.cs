namespace Fanex.OandaExChangeRate.Business.Infrastructure.Helper
{
    using System.Net.Mail;
    using System.Net;

    using Fanex.OandaExChangeRate.Business.Infrastructure.Configurations;
    using System;
    using Fanex.OandaExChangeRate.Business.Common;
    using System.Collections.Generic;

    public class Mail
    {
        

        public static bool SendMail(string emailContent)
        {
            bool isSuccess = true;
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;

                string emailTo = AppSettings.MailTo;
                string emailSubject = string.Format(AppSettings.MailSubject, DateTime.Now);
                string emailTitle = AppSettings.MailTitle;
                string emailCcTo = AppSettings.MailCcTo;
                string emailBCcTo = AppSettings.MailBCcTo;

                var mailItem = new Fanex.MailDeliver.Model.MailRecord
                {
                    MailTo = emailTo,
                    MailCc = emailCcTo,
                    MailBcc = emailBCcTo,
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
            }

            return isSuccess;
        }
    }
}
