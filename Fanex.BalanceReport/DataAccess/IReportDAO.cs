namespace Fanex.NAPReport.BalanceReport.DataAccess
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BalanceReport.Model;
    using Fanex.NAPReport.BalanceReport;

    public interface IReportDAO
    {
        List<CreditTerm> GetCreditTerm(CreditTermReportCriteria criteria);
        List<BalanceModel> GetSuperBalance(SuperBalanceReportCriteria criteria);
        List<BalanceModel> GetDirectBalanceMaster(DirectBalanceReportMasterCriteria criteria);
        List<BalanceModel> GetDirectBalanceAgent(DirectBalanceReportAgentCriteria criteria);
        List<BalanceModel> GetDirectBalanceMember(DirectBalanceReportMemberCriteria criteria);
    }
}