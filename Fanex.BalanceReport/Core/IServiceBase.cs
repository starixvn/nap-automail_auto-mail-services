namespace Fanex.NAPReport.BalanceReport.Core
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;

    public interface IServiceBase
    {
        IEnumerable<T> GetReportData<T>(ICriteria criteria);
    }
}
