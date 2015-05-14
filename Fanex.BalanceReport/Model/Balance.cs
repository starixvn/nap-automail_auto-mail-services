namespace Fanex.NAPReport.BalanceReport.Model
{
    using System;

    /// <summary>
    /// Customer Information.
    /// </summary>
    public class BalanceModel
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public int Currency { get; set; }

        public double Balance { get; set; }

        public double BaseBalance { get; set; }
    }
}