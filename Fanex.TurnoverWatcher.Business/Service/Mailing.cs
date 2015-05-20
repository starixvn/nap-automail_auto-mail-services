namespace Fanex.TurnoverWatcher.Business.Service
{
    using System;
    using System.Globalization;
    using System.IO;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.Service.Core.Extensions;
    using Fanex.TurnoverWatcher.Business.Model;
    using RazorEngine;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    public static class Mailing
    {
        public const string NotificationEmailTemplate = @"/Templates/NotificationEmailTemplate.cshtml";

        public static void ErrorEmailNotification(int fromPercentage, Result result)
        {
            try
            {
                var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                var mailTimezone = timezone > 0 ? ("+" + timezone) : timezone.ToString(CultureInfo.InvariantCulture);
                var root = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = root + NotificationEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, result);
                string emailTo = AppSettings.AdminEmail;
                var emailSubject = string.Format(
                      AppSettings.MailTo,
                      fromPercentage.ToString(),
                      DateTime.Now,
                      mailTimezone);
                string emailTitle = AppSettings.MailTitle;

                var mailItem = new Fanex.MailDelivery.Client.Models.Mail
                {
                    Subject = emailSubject,
                    Title = emailTitle,
                    Body = emailContent
                };

                mailItem.MailingListId = mailStationAppSettings.GetFunctionId("Jeff test");
                MailClientHandler.Start();
                MailClientHandler.SendMail(mailItem);
                MailClientHandler.Stop();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}