namespace Fanex.NAPReport.SMALoss.Business.Criteria
{
    using Fanex.Data.Repository;

    public class LimitLossActionConfigCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_ActionConfigSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}