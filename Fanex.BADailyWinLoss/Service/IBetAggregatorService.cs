namespace Fanex.BAMonthlyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using Fanex.BADailyWinLoss.Business.Model;

    public interface IBetAggregatorService
    {        
        List<BALicWinLossDaily> GetWinlossDaily(DateTime date);

        List<BALicWinLossDaily> GetWinlossMonthToDate(DateTime date);

        List<BALicWinLossDaily> GetWinlossYearToDate(DateTime date);        
    }
}
