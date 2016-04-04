using Fanex.NAPReport.TraderPerformance.Business.Model;
using System.Collections.Generic;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public interface ICalculationService
    {
        RawTrader CalculateGrandTotal(List<RawTrader> rawTraders);

        TraderDetailItem CalculateDetailTotal(List<RawTraderDetail> rawTraderDetails);

        TraderDetailItem FromRawTraderDetail(RawTraderDetail rawTraderDetail);

        TraderDetailByUserName TraderDetailByUserNameItem(string userName, IEnumerable<RawTraderDetail> rawTraderDetails);

        List<TraderDetailByUserName> TraderDetailByUserNames(List<RawTraderDetail> rawTraderDetails);
    }
}
