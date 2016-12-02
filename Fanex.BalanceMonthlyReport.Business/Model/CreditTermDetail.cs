namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Model
{
    using System;

    /// <summary>
    /// Customer Information.
    /// </summary>
    public class CreditTermDetail
    {
        public int GroupID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public int Currency { get; set; }

        public string Role { get; set; }

        public double Balance { get; set; }

        public double Balance_Base { get; set; }

        public double Balance_Group { get; set; }

        public string AccountRemark { get; set; }

        public string Phone { get; set; }

        public int CheckerName { get; set; }

        public DateTime AccountRemarkDate { get; set; }

        public bool IsGroupHeadLine { get; set; }
    }
}