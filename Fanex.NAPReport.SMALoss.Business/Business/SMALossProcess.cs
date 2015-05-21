namespace Fanex.NAPReport.SMALoss.Business.Business
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.NAPReport.SMALoss.Business.Services;
    using Fanex.NAPReport.SMALoss.Business.Services.ExcelExport;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;

    public class SMALossProcess : ISMALossProcess
    {
        private const string UpwardArrowUrl = @"/SiteConfig/SMALoss/Templates/Images/UpwardArrow.png";
        private const string DownwardArrowUrl = @"/SiteConfig/SMALoss/Templates/Images/DownardArrow.png";
        private const string HorizontalLineUrl = @"/SiteConfig/SMALoss/Templates/Images/HorizontalLine.png";
        private const string TemplateUrl = @"/SiteConfig/SMALoss/Templates/EmailTemplate.cshtml";
        private readonly ISMALossService _lossService = null;
        private readonly ISMALossExcelService _lossExcelService = null;

        public SMALossProcess()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        /// <param name="lossService">SMA loss service.</param>
        private SMALossProcess(
            ISMALossService lossService,
            ISMALossExcelService lossExcelService)
        {
            _lossService = lossService ?? new SMALossService();
            _lossExcelService = lossExcelService ?? new SMALossExcelService();
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>The email template.</returns>
        private string GetTemplate
        {
            get
            {
                var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                return File.ReadAllText(appLocation + TemplateUrl);
            }
        }

        /// <summary>
        /// Notifies the email.
        /// </summary>
        /// <returns>Boolean value indicates whether the email is sent successfully or not.</returns>
        public bool NotifyEmail()
        {
            var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            var timezoneInd = DateTime.Now.IsDaylightSavingTime() ? offset - 1 : offset;
            var subject = string.Format(AppSettings.Subject, DateTime.Now, string.Format("{0}{1}", timezoneInd > 0 ? "+" : string.Empty, timezoneInd));
            var isNotified = true;
            var lossItems = GetReachedSMALoss();

            if (lossItems != null && lossItems.Any())
            {
                var content = Razor.Parse(GetTemplate, lossItems);

                isNotified = SendMail(new EmailContent
                {
                    Subject = subject,
                    Title = AppSettings.Title,
                    Body = content
                });
            }

            return isNotified;
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>Return true or false.</returns>
        private bool SendMail(EmailContent content)
        {
            try
            {
                var mail = new Mail()
                {
                    Subject = content.Subject,
                    Title = content.Title,
                    Body = content.Body,
                    MailingListId = content.MailingListId
                };
                var reportDirectory = this.CreateSMAReportDataDirectory();

                var curDateTime = DateTime.UtcNow;
                var fileName = string.Format("SMA_Loss_Report_{0}.xlsx", curDateTime.ToString("yyyy-MM-dd"));
                var fileNamePath = Path.Combine(reportDirectory, fileName);
                var sheetName = "SMA Loss Report";
                var exportSettings = new ExportSettings();
                var lossItems = GetReachedSMALoss();

                _lossExcelService.GenerateReport(content.Subject, sheetName, lossItems, exportSettings, fileNamePath);

                mail.Attachments = new List<MailAttachment>();
                mail.Attachments.Add(new MailAttachment(fileNamePath, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

                ////var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                ////var upwardArrow = new MailLinkedResource(appLocation + UpwardArrowUrl, FileExtension.IMG_PNG, "lruworradrawpu");
                ////var downwardArrow = new MailLinkedResource(appLocation + DownwardArrowUrl, FileExtension.IMG_PNG, "lruworradrawnwod");
                ////var horizontalLineArrow = new MailLinkedResource(appLocation + HorizontalLineUrl, FileExtension.IMG_PNG, "lruworradrawenillatnoziroh");
                ////mail.AlternativeViews = new List<MailLinkedResource>();
                ////mail.AlternativeViews.Add(upwardArrow);
                ////mail.AlternativeViews.Add(downwardArrow);
                ////mail.AlternativeViews.Add(horizontalLineArrow);

                MailClientHandler.SendMail(mail);
                return true;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return false;
            }
        }

        private string CreateSMAReportDataDirectory()
        {
            var reportDataDirectory = string.Format("{0}\\{1}\\{2}", AppSettings.ExportDataDirectory, DateTime.Now.Year, DateTime.Now.Month);
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);

            return reportDataDirectory;
        }

        /// <summary>
        /// Gets the reached SMA loss.
        /// </summary>
        /// <returns>List of FormattedSMALossGroup objects.</returns>
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

        /// <summary>
        /// Gets the SMA loss levels.
        /// </summary>
        /// <returns>List of SMALossLevelItem objects.</returns>
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
    }
}