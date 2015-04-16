namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get All Sport Type Criteria.
    /// </summary>
    public class GetAllSportType : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "GetAllSportType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
