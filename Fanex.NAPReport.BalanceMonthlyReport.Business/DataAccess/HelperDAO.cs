using System;
using System.Collections.Generic;
using System.Data;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Configurations;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Core;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
using Fanex.NAPReport.BalanceMonthlyReport.Business;
using System.Linq;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess
{
    public class HelperDAO : QueryService, IHelperDAO
    {
        public List<Exchange> GetExchange(ExchangeCriteria criteria)
        {
            return GetData<Exchange>(criteria).ToList();
        }
    }
}
