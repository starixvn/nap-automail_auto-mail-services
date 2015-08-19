namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Criteria
{
    using Fanex.Data.Repository;

    public class Sel_BySiteCriteria : CriteriaBase
    {
        public string Site { get; set; }

        public bool IsTest 
        { 
            get 
            { 
                return AppSettings.IsTesting; 
            } 
        }

        [SpParam(Ignored = true)]
        public string SpName
        {
            get { return GetSettingKey(); }
        }
        
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Acc_BalanceNegativeSel_BySite";
        }
    }
}
