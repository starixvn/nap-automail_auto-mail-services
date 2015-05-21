namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorRepository
    {
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);        
    }
}
