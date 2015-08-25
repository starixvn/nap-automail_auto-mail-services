namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    public class DirectBalanceReportMemberCriteria : BalanceReportBaseCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Direct_Member";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}