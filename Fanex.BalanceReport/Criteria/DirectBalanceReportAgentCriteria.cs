namespace Fanex.NAPReport.BalanceReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class DirectBalanceReportAgentCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int Currency { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Direct_Agent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}