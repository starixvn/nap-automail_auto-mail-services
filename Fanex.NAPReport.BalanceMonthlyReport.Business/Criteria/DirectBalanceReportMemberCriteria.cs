namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class DirectBalanceReportMemberCriteria : BalanceReportBaseCriteria
    {
        public string Username { get; set; }

        public int Currency { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Direct_Member";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}