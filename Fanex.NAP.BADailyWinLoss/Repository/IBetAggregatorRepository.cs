namespace Fanex.BAMonthlyWinLoss.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BADailyWinLoss.Business.Criteria;
    using Fanex.NAPReport.BADailyWinLoss.Business.Model;

    public interface IBetAggregatorRepository
    {
        List<BALicWinLossDaily> GetWinlossDaily(GetByDateCriteria criteria);

        List<BALicWinLossDaily> GetWinlossMonthToDate(GetMonthToDateCriteria criteria);

        List<BALicWinLossDaily> GetWinlossYearToDate(GetYearToDateCriteria criteria);        
    }
}
