namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class ExchangeCriteria : CriteriaBase
    {
        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_ExchangeCurrencySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}