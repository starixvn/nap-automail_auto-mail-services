namespace Fanex.NAPReport.SMALoss.Business.Criteria
{
    using Fanex.Data.Repository;

    public class MonitorListCriteria : CriteriaBase
    {
        public int ActionID
        {
            get { return 2; }
        }

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