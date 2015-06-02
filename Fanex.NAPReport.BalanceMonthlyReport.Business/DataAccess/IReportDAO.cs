namespace Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    using Fanex.NAPReport.BalanceMonthlyReport.Business;

    public interface IReportDAO
    {
        List<CreditTerm> GetCreditTerm(CreditTermReportCriteria criteria);
        List<BalanceModel> GetSuperBalance(SuperBalanceReportCriteria criteria);
        List<BalanceModel> GetDirectBalanceMaster(DirectBalanceReportMasterCriteria criteria);
        List<BalanceModel> GetDirectBalanceAgent(DirectBalanceReportAgentCriteria criteria);
        List<BalanceModel> GetDirectBalanceMember(DirectBalanceReportMemberCriteria criteria);
    }
}