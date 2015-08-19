namespace Fanex.NAPReport.RngCasino.Business.Helper.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;

    public interface IServiceBase
    {
        IEnumerable<T> GetReportData<T>(ICriteria criteria);
    }
}
