namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.TurnoverWatcher.Business.Constants;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using RazorEngine;

    public class MailingService : IMailingServiceBase
    {
        private IMailStationService _mailStation;
        private MailInfo _mailInfo;
        private ITurnoverWatcherService _service;
        private IExportExcelService _excel;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\TurnoverWatcher\Settings\TurnoverWatcher.Settings.xml");
            configurationParser.Parse<AppSettings>();            
            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();
            _service = new TurnoverWatcherService();
            _excel = new ExportExcelService();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();
            var data = _service.GetSMATurnover();

            if (data.Count > 0)
            {
                var sendMailLevels = data.Select(x => x.SendMailLevel).Distinct().ToList();
                for (var i = 0; i < sendMailLevels.Count; i++)
                {
                    var toPercentage = i == sendMailLevels.Count - 1 ? 101 : sendMailLevels[i + 1];
                    var fromPercentage = sendMailLevels[i];
                    var notifiedCustomerMatchTurnovers = data.Where(
                        x => x.CustomerInfo.PercentageUsed >= fromPercentage
                        && (x.CustomerInfo.PercentageUsed < toPercentage ||
                            i == sendMailLevels.Count - 1)).ToList();

                    if (notifiedCustomerMatchTurnovers != null && notifiedCustomerMatchTurnovers.Count > 0)
                    {
                        _excel.ExportExcel(notifiedCustomerMatchTurnovers);

                        var template = GetTemplate();
                        var bodyContent = Razor.Parse(template, notifiedCustomerMatchTurnovers);

                        var fileNamesPath = new List<string>();
                        fileNamesPath.Add(_excel.DirectoryOfFileName);

                        _mailInfo = new MailInfo()
                        {
                            FileName = _excel.FileName,
                            FileNamesPath = fileNamesPath
                        };

                        var mail = _mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, bodyContent);
                        mail.MailingListId = AppSettings.FunctionId;
                        mailResult.Mail = mail;
                        MailClientHandler.SendMail(mail);
                        if (!AppSettings.IsTest)
                        {
                            _service.RemoveNotifiedCustomer(notifiedCustomerMatchTurnovers, fromPercentage);
                        }
                    }
                }
            }

            return mailResult;
        }

        public void SendMailToNormalGroup()
        {
        }

        private string GetTemplate()
        {
            var appLocation = AppDomain.CurrentDomain.BaseDirectory;
            string template = File.ReadAllText(appLocation + EmailTemplateConstants._TURNOVER_WATCHER_EMAIL_TEMPLATE);
            return template;
        }
    }
}