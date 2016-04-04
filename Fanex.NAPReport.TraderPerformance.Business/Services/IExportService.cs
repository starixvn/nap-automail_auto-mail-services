using Fanex.NAPReport.TraderPerformance.Business.Model;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public interface IExportService
    {
        void Export(TraderPerformanceModel traderModel, TraderPerformanceDetailModel traderDetailModel);
    }
}
