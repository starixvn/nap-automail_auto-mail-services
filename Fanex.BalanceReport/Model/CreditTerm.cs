namespace Fanex.NAPReport.BalanceReport.Model
{
    using System;

    /// <summary>
    /// Customer Information.
    /// </summary>
    public class CreditTerm
    {
        public int GroupID { get; set; }

        public string Username { get; set; }

        public int Currency { get; set; }

        public string Role { get; set; }

        public double Balance { get; set; }

        public double Balance_Base { get; set; }

        public double PreAgreeAmount { get; set; }

        public string AccountRemark { get; set; }

        public string Phone { get; set; }

        public int CheckerID { get; set; }

        public string CheckerName { get; set; }

        public DateTime AccountRemarkDate { get; set; }

        public bool OverLimit { get; set; }
    }
}