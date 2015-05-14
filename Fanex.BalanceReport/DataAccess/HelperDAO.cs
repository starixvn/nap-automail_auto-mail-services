using System;
using System.Collections.Generic;
using System.Data;
using Fanex.NAPReport.BalanceReport.Infrastructure.Configurations;
using Fanex.NAPReport.BalanceReport.Core;
using Fanex.NAPReport.BalanceReport.Model;
using Fanex.NAPReport.BalanceReport;
using System.Linq;

namespace Fanex.NAPReport.BalanceReport.DataAccess
{
    public class HelperDAO : QueryService, IHelperDAO
    {
        public List<Exchange> GetExchange(ExchangeCriteria criteria)
        {
            return GetData<Exchange>(criteria).ToList();
        }
    }
}
