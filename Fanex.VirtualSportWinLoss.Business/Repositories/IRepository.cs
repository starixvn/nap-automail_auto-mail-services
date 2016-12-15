namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;

    public interface IRepository
    {
        BalanceUp BalanceUp { get; }

        List<WinLossSummaryBySportType> GetWinLossSummaryListBySportType(CriteriaBase criteria);

        List<WinLossDetailByBettype> GetWinLossDetailListByBettype(CriteriaBase criteria);        
    }
}
