// -----------------------------------------------------------------------
// <copyright file="BalanceMonthlyWinLossRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Repository
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BalanceRepository : IBalanceRepository
    {
        private IObjectDb GetDrawData(CriteriaBase criteria)
        {
            IObjectDb db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());

            return db;
        }

        public List<Exchange> GetCurrencyList()
        {
            ExchangeCriteria criteria = new ExchangeCriteria();
            List<Exchange> currencies;

            using (IObjectDb db = GetDrawData(criteria))
            {
                currencies = db.QueryMultiple().Read<Exchange>().ToList();
            }

            return currencies;
        }

        public CreditTerm GetCreditTerm(CriteriaBase criteria)
        {
            CreditTerm creditTerm = new CreditTerm();
            using (IObjectDb db = GetDrawData(criteria))
            {
                var multi = db.QueryMultiple();
                
                creditTerm.CreditTermMains = multi.Read<CreditTermMain>().ToList();
                creditTerm.CreditTermDetails = multi.Read<CreditTermDetail>().ToList();
            }

            return creditTerm;
        }

        public IDataReader GetBalance(BalanceReportBaseCriteria criteria)
        {
            ObjectDb db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());

            return db.ExecuteReader();
        }
    }
}
