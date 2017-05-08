using Fanex.Data.Repository;

namespace Fanex.NAPReport.HorseTransaction3rd.Business.Criteria
{
    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class GetExchangeCriteria : CriteriaBase
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