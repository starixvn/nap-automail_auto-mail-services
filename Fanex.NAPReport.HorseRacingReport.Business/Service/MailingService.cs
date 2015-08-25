// -----------------------------------------------------------------------
// <copyright file="MailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingService : IMailingService
    {
        private IMailStationService mailStation;
        private MailInfo _mailInfo;

        public MailingService()
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\HorseRacingReport\Settings\HorseRacingReport.Settings.xml");
            configurationParser.Parse<AppSettings>();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        public Mail SendMailToVIPGroup()
        {
            IHorseRacingService service = new HorseRacingService();
            TimeRangeCriteria timeRangeCriteria = AssignRangeDate(service);
            service.GetData(timeRangeCriteria);

            var fileNamesPath = new List<string>();
            fileNamesPath.Add(service.ReportZipFolderPath);

            _mailInfo = new MailInfo() 
            {
                FileName = service.FileName,
                FileNamesPath = fileNamesPath,                
            };

            var mail = mailStation.CreateMailWithAttachedFile(_mailInfo);

            return mail;
        }

        private static TimeRangeCriteria AssignRangeDate(IHorseRacingService service)
        {
            TimeRangeCriteria timeRangeCriteria = new TimeRangeCriteria();

            bool isRunningDay = IsRunningDay();

            if (isRunningDay)
            {
                timeRangeCriteria = service.GetTimeRangeCriteria();
            }
            else
            {
                timeRangeCriteria.FromDate = DateTime.Today.AddDays(-4);
                timeRangeCriteria.ToDate = DateTime.Today.AddDays(-1);
            }

            return timeRangeCriteria;
        }

        private static bool IsRunningDay()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.DayOfWeek == DayOfWeek.Thursday || currentTime.DayOfWeek == DayOfWeek.Monday;
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
