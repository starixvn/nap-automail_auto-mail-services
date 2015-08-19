// -----------------------------------------------------------------------
// <copyright file="IBalanceMonthlyWinLossRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Repository
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBalanceRepository
    {
        List<Exchange> GetCurrencyList();

        CreditTerm GetCreditTerm(CriteriaBase criteria);

        IDataReader GetBalance(BalanceReportBaseCriteria criteria);
    }
}
