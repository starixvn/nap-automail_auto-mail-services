namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorService
    {        
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);
    }
}
