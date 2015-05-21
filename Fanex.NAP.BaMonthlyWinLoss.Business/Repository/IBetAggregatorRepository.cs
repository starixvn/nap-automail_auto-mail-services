namespace Fanex.NAP.BAMonthlyWinLoss.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorRepository
    {
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);        
    }
}
