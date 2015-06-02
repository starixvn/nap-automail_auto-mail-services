namespace Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess
{
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Infrastructure.Configurations;
    public class BaseDataAccess
    {
        protected virtual StoreProcedureElement GetStoreProcedure(StoreProcedureSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = StoreProcedureSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }
    }
}
