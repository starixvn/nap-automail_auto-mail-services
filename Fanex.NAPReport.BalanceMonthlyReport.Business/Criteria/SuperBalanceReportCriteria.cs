namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    public class SuperBalanceReportCriteria : BalanceReportBaseCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Super_Yesterday";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}