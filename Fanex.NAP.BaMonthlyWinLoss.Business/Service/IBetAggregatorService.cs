namespace Fanex.NAP.BAMonthlyWinLoss.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Model;

    public interface IBetAggregatorService
    {        
        List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria);
    }
}
