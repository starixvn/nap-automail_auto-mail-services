namespace Fanex.NAPReport.BalanceReport.DataAccess
{
    using Fanex.NAPReport.BalanceReport.Infrastructure.Configurations;
    public class BaseDataAccess
    {
        protected virtual StoreProcedureElement GetStoreProcedure(StoreProcedureSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = StoreProcedureSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }
    }
}
