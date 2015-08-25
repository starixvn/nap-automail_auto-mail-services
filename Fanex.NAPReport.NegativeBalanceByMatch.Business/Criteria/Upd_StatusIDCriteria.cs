namespace Fanex.NAPReport.Business.Criteria
{
    using Fanex.Data.Repository;

    public class Upd_StatusIDCriteria : CriteriaBase
    {
        public string LogIDs { get; set; }

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
            return "Acc_BalanceNegativeUpd_StatusID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
