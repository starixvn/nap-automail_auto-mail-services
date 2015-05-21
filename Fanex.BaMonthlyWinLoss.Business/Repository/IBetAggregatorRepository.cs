namespace Fanex.BAMonthlyWinLoss.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorRepository
    {
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);        
    }
}
