namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    public class DirectBalanceReportMasterCriteria : BalanceReportBaseCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Direct_Master";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}