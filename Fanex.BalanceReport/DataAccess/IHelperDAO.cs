using System.Collections.Generic;
using Fanex.NAPReport.BalanceReport.Model;
using Fanex.NAPReport.BalanceReport;

namespace Fanex.NAPReport.BalanceReport.DataAccess
{
    public interface IHelperDAO
    {
        List<Exchange> GetExchange(ExchangeCriteria criteria);
    }
}