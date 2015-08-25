namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;

    public interface IServiceBase
    {
        IEnumerable<T> GetReportData<T>(ICriteria criteria);
    }
}
