namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class CreditTermReportCriteria : CriteriaBase
    {
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