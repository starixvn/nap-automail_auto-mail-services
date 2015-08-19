namespace Fanex.NAPReport.Business.Criteria
{
    using Fanex.Data.Repository;

    public class Sel_MatchLogCriteria : CriteriaBase
    {
        public int MatchID { get; set; }

        public int GroupID { get; set; }

        [SpParam(Ignored = true)]
        public string SpName
        {
            get { return GetSettingKey(); }
        }

        [SpParam(Ignored = true)]
        public object Result
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Acc_BalanceNegativeSel_MatchLog";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
