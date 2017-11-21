namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    using System;

    /// <summary>
    /// SMA Loss entity contains data queried from DB.
    /// </summary>
    public class SMALoss
    {
        public byte LevelID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public byte RoleID { get; set; }

        public string Currency { get; set; }

        public int GroupHeadlineId { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal MaxLoss { get; set; }

        public int PercentageLoss { get; set; }

        public int Movement { get; set; }
    }
}