namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// SMA loss group view model.
    /// </summary>
    public class FormattedSMALossGroup
    {
        public FormattedSMALossGroup(SMALoss rawData, string levelRange)
        {
            if (rawData != null)
            {
                this.LossLevel = rawData.LevelID;
                this.LossLevelRange = levelRange;
                this.SMALossItems = new List<FormattedSMALossItem>();
            }
        }

        public byte LossLevel { get; set; }

        public string LossLevelRange { get; set; } = string.Empty;

        public List<FormattedSMALossItem> SMALossItems { get; set; } = new List<FormattedSMALossItem>();

        public string UrlDownloadFile
        { 
            get 
            { 
                return AppSettingBase.NapReportURL; 
            }
        }
    }
}