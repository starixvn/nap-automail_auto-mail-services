namespace Fanex.NAPReport.SMALoss.Business.Process
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMALoss.Business.Criteria;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.NAPReport.SMALoss.Business.Services;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;

    public class SMALossProcess : ISMALossProcess
    {
        private readonly ISMALossService _lossService = null;
        private string _template = null;

        public SMALossProcess()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        /// <param name="lossService">SMA loss service.</param>
        private SMALossProcess(
            ISMALossService lossService)
        {
            _lossService = lossService ?? new SMALossService();
        }

        /// <summary>
        /// Notifies the email.
        /// </summary>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>Boolean value indicates whether the email is sent successfully or not.</returns>
        public bool NotifyEmail(string templateUrl)
        {
            var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            var timezoneInd = DateTime.Now.IsDaylightSavingTime() ? offset - 1 : offset;
            var subject = string.Format(AppSettings.Subject, DateTime.Now, string.Format("{0}{1}", timezoneInd > 0 ? "+" : string.Empty, timezoneInd));
            var lossItems = GetReachedSMALoss();

            var template = GetTemplate(templateUrl);
            var content = Razor.Parse(template, lossItems);

            var isNotified = SendMail(new EmailContent
            {
                Subject = subject,
                Title = AppSettings.Title,
                Body = content
            });

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
                    Body = content.Body
                };
                mail.MailingListId = content.MailingListId;
                MailClientHandler.Start();
                MailClientHandler.SendMail(mail);
                MailClientHandler.Stop();
                return true;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return false;
            }
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>The email template.</returns>
        private string GetTemplate(string templateUrl)
        {
            if (this._template == null)
            {
                var appLocation = AppDomain.CurrentDomain.BaseDirectory;
                this._template = File.ReadAllText(appLocation + templateUrl);
            }

            return this._template;
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