namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria
{
    using Fanex.Data.Repository;

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
