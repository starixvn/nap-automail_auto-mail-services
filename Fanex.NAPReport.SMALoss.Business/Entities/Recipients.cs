namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    /// <summary>
    /// The recipients of an email.
    /// </summary>
    public class Recipients
    {
        public int SiteID { get; set; }

        public string Site { get; set; }

        public string ToList { get; set; }

        public string CcList { get; set; }

        public string BccList { get; set; }

        public int IsAvailabe { get; set; }
    }
}