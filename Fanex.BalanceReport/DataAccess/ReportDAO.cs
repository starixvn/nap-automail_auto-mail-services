namespace Fanex.NAPReport.BalanceReport.DataAccess
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BalanceReport;
    using Fanex.NAPReport.BalanceReport.Core;
    using Fanex.NAPReport.BalanceReport.Model;
    using System.Linq;

    public class ReportDAO : QueryService, IReportDAO
    {
        public List<CreditTerm> GetCreditTerm(CreditTermReportCriteria criteria)
        {
            return GetData<CreditTerm>(criteria).ToList();
        }

        public List<BalanceModel> GetSuperBalance(SuperBalanceReportCriteria criteria)
        {
            return GetData<BalanceModel>(criteria).ToList();
        }

        public List<BalanceModel> GetDirectBalanceMaster(DirectBalanceReportMasterCriteria criteria)
        {
            return GetData<BalanceModel>(criteria).ToList();
        }

        public List<BalanceModel> GetDirectBalanceAgent(DirectBalanceReportAgentCriteria criteria)
        {
            return GetData<BalanceModel>(criteria).ToList();
        }

        public List<BalanceModel> GetDirectBalanceMember(DirectBalanceReportMemberCriteria criteria)
        {
            return GetData<BalanceModel>(criteria).ToList();
        }
    }
}
