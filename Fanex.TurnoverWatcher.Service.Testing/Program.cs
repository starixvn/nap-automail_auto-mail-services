namespace Fanex.NAPReport.TurnoverWatcher.Service.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.TurnoverWatcher.Business;
    using Fanex.NAPReport.TurnoverWatcher.Business.Constants;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.NAPReport.TurnoverWatcher.Business.Service;
    using RazorEngine;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running TurnoverWatcher");

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            var service = new TurnoverWatcherService();
            var exportDataService = new TurnoverWatcherExportDataService();

            var data = service.ProcessTurnoverWatcher();
            exportDataService.ExportReportToExcel(data);
            string reportPath = exportDataService.ReportPath;

            if (data.Count > 0)
            {
                var sendMailLevels = data.Select(x => x.SendMailLevel).Distinct().ToList();
                for (var i = 0; i < sendMailLevels.Count; i++)
                {
                    var sendMailSuccess = false;
                    var toPercentage = i == sendMailLevels.Count - 1 ? 101 : sendMailLevels[i + 1];
                    var fromPercentage = sendMailLevels[i];
                    var notifiedCustomerMatchTurnovers = data.Where(
                        x => x.CustomerInfo.PercentageUsed >= fromPercentage
                        && (x.CustomerInfo.PercentageUsed < toPercentage ||
                            i == sendMailLevels.Count - 1)).ToList();

                    if (notifiedCustomerMatchTurnovers != null && notifiedCustomerMatchTurnovers.Count > 0)
                    {
                        sendMailSuccess = SendTurnoverWatcherEmail(notifiedCustomerMatchTurnovers, fromPercentage, reportPath);
                    }

                    if (sendMailSuccess)
                    {
                        var resultRemove = service.RemoveNotifiedCustomer(notifiedCustomerMatchTurnovers, fromPercentage);
                        if (!resultRemove.ErrorCode.Equals(0))
                        {
                            Mailing.ErrorEmailNotification(fromPercentage, resultRemove);
                        }
                    }
                }
            }

            Console.WriteLine("Done at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:s tt"));
            Console.ReadLine();
        }

        private static bool SendTurnoverWatcherEmail(List<TurnoverInfo> turnoverInfo, int fromPercentage, string path)
        {
            try
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                var mailTimezone = timezone > 0 ? ("+" + timezone) : timezone.ToString(CultureInfo.InvariantCulture);

                string filePath = root + EmailTemplateConstants.TurnoverWatcherEmailTemplate;
                string template = File.ReadAllText(filePath);
                var emailContent = Razor.Parse(template, turnoverInfo);

                var emailSubject = string.Format(
                      AppSettings.MailSubject,
                      fromPercentage.ToString(),
                      DateTime.Now,
                      mailTimezone);

                var mail = new Mail
                {
                    Subject = emailSubject,
                    Title = AppSettings.MailTitle,
                    Body = emailContent,
                };

                mail.Attachments = new List<MailAttachment>();
                mail.Attachments.Add(new MailAttachment(path, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

                mail.MailingListId = mailStationAppSettings.GetFunctionId("Jeff test");
                MailClientHandler.Start();
                MailClientHandler.SendMail(mail);
                MailClientHandler.Stop();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}