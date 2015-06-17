// -----------------------------------------------------------------------
// <copyright file="IBalanceMonthlyWinLossRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    using System.Data;
    using Fanex.Data.Repository;
    using Fanex.Data;

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
