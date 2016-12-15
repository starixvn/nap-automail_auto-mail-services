namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    public class DirectBalanceReportAgentCriteria : BalanceReportBaseCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Direct_Agent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}