namespace Fanex.NAPReport.RngCasino.Business.Criteria
{
    using Fanex.Data.Repository;

    public class GetCurrencyListCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_ExchangeCurrencySel";
        }
    }
}
