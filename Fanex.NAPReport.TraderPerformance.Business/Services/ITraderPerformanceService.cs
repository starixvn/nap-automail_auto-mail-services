using Fanex.NAPReport.TraderPerformance.Business.Model;
using System;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public interface ITraderPerformanceService
    {
        TraderPerformanceModel GetReport(DateTime reportDate);

        TraderPerformanceDetailModel GetDetailReport(DateTime reportDate);
    }
}
