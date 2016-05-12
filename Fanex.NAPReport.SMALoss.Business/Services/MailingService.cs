namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.NAPReport.SMALoss.Business.Services.ExcelExport;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using RazorEngine;

    public class MailingService : IMailingServiceBase
    {
        private const string UpwardArrowUrl = @"/SiteConfig/SMALoss/Templates/Images/UpwardArrow.png";
        private const string DownwardArrowUrl = @"/SiteConfig/SMALoss/Templates/Images/DownardArrow.png";
        private const string HorizontalLineUrl = @"/SiteConfig/SMALoss/Templates/Images/HorizontalLine.png";
        private const string TemplateUrl = @"/SiteConfig/SMALoss/Templates/EmailTemplate.cshtml";
        private readonly ISMALossService _lossService = null;
        private readonly ISMALossExcelService _lossExcelService = null;
        private IMailStationService mailStation;
        private MailInfo _mailInfo;

        public MailingService()
            : this(null, null)
        {
        }

        private MailingService(
            ISMALossService lossService,
            ISMALossExcelService lossExcelService)
        {
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\SMALoss\Settings\SMALoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            _lossService = lossService ?? new SMALossService();
            _lossExcelService = lossExcelService ?? new SMALossExcelService();

            mailStation = new MailStationService();
            _mailInfo = new MailInfo();
        }

        private string DirectoryOfFileName
        {
            get;
            set;
        }

        private string FileName
        {
            get;
            set;
        }

        private string GetTemplate
        {
            get
            {
                var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                return File.ReadAllText(appLocation + TemplateUrl);
            }
        }

        private IEnumerable<FormattedSMALossGroup> GetReachedSMALoss()
        {
            var rawData = _lossService.GetNotifiedSMALoss(new MonitorListCriteria());
            var levels = this.GetSMALossLevels();
            List<FormattedSMALossGroup> result = null;

            if (rawData != null && rawData.Any())
            {
                IEnumerable<IGrouping<byte, SMALoss>> groups = rawData.GroupBy(obj => obj.LevelID);
                FormattedSMALossGroup lossGroup = null;
                SMALoss firstItem = null;
                FormattedSMALossLevelItem level = null;

                result = new List<FormattedSMALossGroup>();

                foreach (IGrouping<byte, SMALoss> group in groups)
                {
                    firstItem = group.ElementAt(0);
                    level = levels.FirstOrDefault(obj => obj.LevelID == firstItem.LevelID);
                    lossGroup = new FormattedSMALossGroup(firstItem, level.Value);

                    foreach (SMALoss item in group)
                    {
                        lossGroup.SMALossItems.Add(new FormattedSMALossItem(item));
                    }

                    if (lossGroup.SMALossItems.Count > 0)
                    {
                        result.Add(lossGroup);
                    }
                }
            }

            return result;
        }
      
        private List<FormattedSMALossLevelItem> GetSMALossLevels()
        {
            IEnumerable<SMALossLevel> rawData = this._lossService.GetSMALossLevels(new LimitLossActionConfigCriteria());
            List<FormattedSMALossLevelItem> result = null;

            if (rawData.Any())
            {
                SMALossLevel higherLevel = null;

                rawData = rawData.OrderByDescending(obj => obj.LevelID);
                result = new List<FormattedSMALossLevelItem>();

                foreach (SMALossLevel level in rawData)
                {
                    result.Add(new FormattedSMALossLevelItem(level, higherLevel));
                    higherLevel = level;
                }
            }

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public MailResult SendMailToVIPGroup()
        {
            MailResult mailResult = new MailResult();

            string content = string.Empty;
            var lossItems = GetReachedSMALoss();

            int changedLevelUp = 0;
            int changedLevelDown = 0;
            int changedLevelHorizontal = 0;

            lossItems.ToList().ForEach(s => changedLevelUp += s.SMALossItems.Where(a => a.ChangedLevel > 0).Count());
            lossItems.ToList().ForEach(s => changedLevelDown += s.SMALossItems.Where(a => a.ChangedLevel < 0).Count());
            lossItems.ToList().ForEach(s => changedLevelHorizontal += s.SMALossItems.Where(a => a.ChangedLevel == 0).Count());
            
            if (lossItems != null && lossItems.Any())
            {
                content = Razor.Parse(GetTemplate, lossItems);

                var curDateTime = DateTime.UtcNow;
                FileName = string.Format("List_Of_SMA_Reaching_Allowable_Max_Loss_For_{0}.xlsx", curDateTime.ToString("yyyy_MM_dd"));
                DirectoryOfFileName = Path.Combine(AppSettings.ExportDataDirectoryWithYearMonth, FileName);
                var sheetName = "SMA Loss Report";
                var exportSettings = new ExportSettings();
                _lossExcelService.GenerateReport(string.Empty, sheetName, lossItems, exportSettings, DirectoryOfFileName);

                var fileNamesPath = new List<string>();
                fileNamesPath.Add(DirectoryOfFileName);

                _mailInfo = new MailInfo()
                {
                    Subject = string.Format(AppSettings.Subject, DateTime.Now.ToString("yyyy/MM/dd")),
                    FileName = FileName,
                    FileNamesPath = fileNamesPath
                };
                
                var mail = mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, content);

                var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                var upwardArrow = new MailLinkedResource(appLocation + UpwardArrowUrl, FileExtension.IMG_PNG, "lruworradrawpu");
                var downwardArrow = new MailLinkedResource(appLocation + DownwardArrowUrl, FileExtension.IMG_PNG, "lruworradrawnwod");
                var horizontalLineArrow = new MailLinkedResource(appLocation + HorizontalLineUrl, FileExtension.IMG_PNG, "lruworradrawenillatnoziroh");
                mail.AlternativeViews = new List<MailLinkedResource>();

                if (changedLevelUp > 0)
                {
                    mail.AlternativeViews.Add(upwardArrow);
                }

                if (changedLevelDown > 0)
                {
                    mail.AlternativeViews.Add(downwardArrow);
                }

                if (changedLevelHorizontal > 0)
                {
                    mail.AlternativeViews.Add(horizontalLineArrow);
                }
                
                mailResult.Mail = mail;
            }
            else
            {
                mailResult.JobStatus = JobStatus.DataIsNotReady;
            }

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