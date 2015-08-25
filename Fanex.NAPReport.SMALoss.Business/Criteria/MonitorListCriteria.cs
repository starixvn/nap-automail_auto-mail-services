namespace Fanex.NAPReport.SMALoss.Business.Criteria
{
    using Fanex.Data.Repository;

    public class MonitorListCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_MonitorList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}