namespace Fanex.BAMonthlyWinLoss.Business.Service
{
    using System.Collections.Generic;
    using Fanex.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorService
    {        
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);
    }
}
