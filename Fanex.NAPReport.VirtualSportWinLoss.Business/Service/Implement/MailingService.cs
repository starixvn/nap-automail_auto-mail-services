namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    public class MailingService : IMailingServiceBase
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;
        private IExportExcelService _service;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\VirtualSportWinLoss\Settings\VirtualSportWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
            _service = new ExportExcelService();
        }

        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();
            _service.ExportExcel();

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(_service.DirectoryOfFileName);

            _mailInfo.FileName = _service.FileName;
            _mailInfo.FileNamesPath = fileNamesPath;
            _mailInfo.Subject = string.Format(AppSettings.Subject, DateTime.Now.ToString("yyyy/MM/dd"));

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);
            mailResult.Mail = mail;

            return mailResult;
        }

        public void SendMailToNormalGroup()
        {
            if (AppSettings.FunctionId > 0)
            {
                mailStation.SendFunctionMailWithNotification(AppSettings.FunctionId, _mailInfo);
            }
        }
    }
}
