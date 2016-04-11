using Fanex.NAPReport.TraderPerformance.Business.Criteria;
using Fanex.NAPReport.TraderPerformance.Business.Model;
using System.Collections.Generic;

namespace Fanex.NAPReport.TraderPerformance.Business.Repositories
{
    public interface ITraderPerformanceRepository
    {
        List<RawTrader> GetRawData(GetTraderPerformanceCriteria criteria);

        List<RawTraderDetail> GetTraderDetailRawData(GetTraderPerformanceDetailCriteria criteria);
    }
}
