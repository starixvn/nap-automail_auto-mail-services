namespace Fanex.NAPReport.BalanceReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class CreditTermReportCriteria : CriteriaBase
    {
        public int GroupID { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_Rpt_CreditTerm";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}