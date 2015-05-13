namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.SMALoss.Business.Entities;

    public class SMALossProcessService
    {
        private const string UpwardArrowUrl = @"/Templates/Images/UpwardArrow.png";
        private const string DownwardArrowUrl = @"/Templates/Images/DownardArrow.png";
        private const string HorizontalLineUrl = @"/Templates/Images/HorizontalLine.png";
        private string _template = null;

        /// <summary>
        /// Notifies the email.
        /// </summary>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>Boolean value indicates whether the email is sent successfully or not.</returns>
        public bool NotifyEmail(Uri templateUrl)
        {
            var isNotified = true;
            var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            var timezoneInd = DateTime.Now.IsDaylightSavingTime() ? offset - 1 : offset;
            var subject = string.Format(AppSettings.Subject, DateTime.Now, string.Format("{0}{1}", timezoneInd > 0 ? "+" : string.Empty, timezoneInd));
            var recipients = GetRecipients();
            IEnumerable<FormattedSMALossGroup> lossItems = GetReachedSMALoss();

            if (recipients != null && !string.IsNullOrEmpty(recipients.ToList) && lossItems != null && lossItems.Any())
            {
                string template = GetTemplate(templateUrl.ToString());
                string content = Razor.Parse(template, lossItems);

                SendMail(new EmailContent
                {
                    Recievers = recipients,
                    Title = AppSettings.Title,
                    Subject = subject,
                    Body = content
                });
            }

            return isNotified;
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
                string appLocation = AppDomain.CurrentDomain.BaseDirectory;

                this._template = File.ReadAllText(appLocation + templateUrl);
            }

            return this._template;
        }

        /// <summary>
        /// Gets the recipients.
        /// </summary>
        /// <returns>Recipients object.</returns>
        private Recipients GetRecipients()
        {
            var recipients = this._repository.GetReceivers(AppSettings.FunctionID);

            recipients = recipients.IsAvailabe != 0 ? recipients : null;

            return recipients;
        }

        /// <summary>
        /// Gets the reached SMA loss.
        /// </summary>
        /// <returns>List of FormattedSMALossGroup objects.</returns>
        private IEnumerable<FormattedSMALossGroup> GetReachedSMALoss()
        {
            IEnumerable<SMALoss> rawData = this._repository.GetNotifiedSMALoss();
            List<FormattedSMALossLevelItem> levels = this.GetSMALossLevels();
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

        ///// <summary>
        ///// Gets the SMA loss levels.
        ///// </summary>
        ///// <returns>List of SMALossLevelItem objects.</returns>
        //private List<FormattedSMALossLevelItem> GetSMALossLevels()
        //{
        //    IEnumerable<SMALossLevel> rawData = this._repository.GetSMALossLevels();
        //    List<FormattedSMALossLevelItem> result = null;

        //    if (rawData.Any())
        //    {
        //        SMALossLevel higherLevel = null;

        //        rawData = rawData.OrderByDescending(obj => obj.LevelID);
        //        result = new List<FormattedSMALossLevelItem>();

        //        foreach (SMALossLevel level in rawData)
        //        {
        //            result.Add(new FormattedSMALossLevelItem(level, higherLevel));
        //            higherLevel = level;
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Sends the mail.
        ///// </summary>
        ///// <param name="content">The content.</param>
        //private void SendMail(EmailContent content)
        //{
        //    using (var client = new MailDeliver())
        //    {
        //        var mailItem = new MailRecord
        //        {
        //            MailTo = content.Recievers.ToList,
        //            MailCc = content.Recievers.CcList,
        //            MailBcc = content.Recievers.BccList,
        //            Title = content.Title,
        //            Subject = content.Subject,
        //            Body = content.Body
        //        };
        //        string appLocation = AppDomain.CurrentDomain.BaseDirectory;
        //        var upwardArrow = new MailLinkedResource(appLocation + UpwardArrowUrl, FileExtentions.IMG_PNG, "lruworradrawpu");
        //        var downwardArrow = new MailLinkedResource(appLocation + DownwardArrowUrl, FileExtentions.IMG_PNG, "lruworradrawnwod");
        //        var horizontalLineArrow = new MailLinkedResource(appLocation + HorizontalLineUrl, FileExtentions.IMG_PNG, "lruworradrawenillatnoziroh");

        //        mailItem.AlternateView = new List<MailLinkedResource>();
        //        mailItem.AlternateView.Add(upwardArrow);
        //        mailItem.AlternateView.Add(downwardArrow);
        //        mailItem.AlternateView.Add(horizontalLineArrow);

        //        client.SendMail(mailItem);
        //    }
        //}
    }
}