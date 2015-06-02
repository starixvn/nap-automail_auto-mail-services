using System.Collections.Generic;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
using Fanex.NAPReport.BalanceMonthlyReport.Business;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess
{
    public interface IHelperDAO
    {
        List<Exchange> GetExchange(ExchangeCriteria criteria);
    }
}