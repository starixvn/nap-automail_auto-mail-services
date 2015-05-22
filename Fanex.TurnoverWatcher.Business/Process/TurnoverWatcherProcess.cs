namespace Fanex.NAPReport.TurnoverWatcher.Business.Process
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.TurnoverWatcher.Business.Constants;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.NAPReport.TurnoverWatcher.Business.Service;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;
    using mailStationAppSettings = Fanex.MailDelivery.Client.Configurations.AppSettings;

    public class TurnoverWatcherProcess : ITurnoverWatcherProcess
    {
        private string _template = null;
        private ITurnoverWatcherService _turnoverWatcher;

        public TurnoverWatcherProcess()
            : this(null)
        {
        }

        private TurnoverWatcherProcess(ITurnoverWatcherService service)
        {
            _turnoverWatcher = service ?? new TurnoverWatcherService();
        }

        public void Process()
        {
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
                        sendMailSuccess = NotifyEmail(notifiedCustomerMatchTurnovers, fromPercentage, reportPath);
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
        }

        private bool NotifyEmail(List<TurnoverInfo> turnoverInfo, int fromPercentage, string path)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            var timezoneInd = DateTime.Now.IsDaylightSavingTime() ? offset - 1 : offset;
            var mailTimezone = timezoneInd > 0 ? ("+" + timezoneInd) : timezoneInd.ToString(CultureInfo.InvariantCulture);
            var subject = string.Format(
                      AppSettings.MailSubject,
                      fromPercentage.ToString(),
                      DateTime.Now,
                      mailTimezone);

            var template = GetTemplate(EmailTemplateConstants.TurnoverWatcherEmailTemplate);
            var content = Razor.Parse(template, turnoverInfo);

            var mail = new Mail
            {
                Subject = subject,
                Title = AppSettings.MailTitle,
                Body = content,
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(path, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            var isNotified = SendMail(mail);

            return isNotified;
        }

        private bool SendMail(Mail mail)
        {
            try
            {
                mail.MailingListId = mailStationAppSettings.GetFunctionId("Turnover Watcher Report");
                MailClientHandler.SendMail(mail);

                return true;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return false;
            }
        }

        private string GetTemplate(string templateUrl)
        {
            if (this._template == null)
            {
                var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                this._template = File.ReadAllText(appLocation + templateUrl);
            }

            return this._template;
        }
    }
}