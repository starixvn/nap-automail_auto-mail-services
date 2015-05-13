namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    /// <summary>
    /// SMA loss item view model.
    /// </summary>
    public class FormattedSMALossLevelItem
    {
        public FormattedSMALossLevelItem()
        {
            this.Value = string.Empty;
            this.LevelID = 0;
        }

        public FormattedSMALossLevelItem(SMALossLevel currentLevel, SMALossLevel higherLevel)
        {
            int minimum = -1;
            int maximun = -1;

            if (higherLevel != null)
            {
                maximun = higherLevel.Value - 1;
            }

            if (currentLevel != null)
            {
                minimum = currentLevel.Value;
                this.LevelID = currentLevel.LevelID;

                if (maximun > 0)
                {
                    this.Value = string.Format("{0}% - {1}%", minimum, maximun);
                }
                else
                {
                    this.Value = string.Format("{0}% & Above", minimum);
                }
            }
        }

        public byte LevelID { get; set; }

        public string Value { get; set; }
    }
}