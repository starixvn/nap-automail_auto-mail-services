namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;

    public class Repository : IRepository
    {
        public List<WinLossSummaryBySportType> GetWinLossSummaryListBySportType(CriteriaBase criteria)
        {
            List<WinLossSummaryBySportType> winlossSummaryListbySportType = new List<WinLossSummaryBySportType>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                objDb.AssignParamValues(param);
                var row = objDb.ExecuteReader();
                while (row.Read())
                {
                    var winlossSummarybySportType = new WinLossSummaryBySportType
                    {
                        ProductId = Converter.To<int>(row["ProductId"]),
                        ProductName = Converter.To<string>(row["ProductName"]),
                        SportID = Converter.To<int>(row["SportID"]),
                        SportName = Converter.To<string>(row["SportName"]),
                        BettypeID = Converter.To<int>(row["BetTypeID"]),
                        BettypeName = Converter.To<string>(row["BetType"]),
                        BetCount = Converter.To<int>(row["BetCount"]),
                        TurnOverIBC = Converter.To<double>(row["TurnoverIBC"]),
                        WinLostIBC = Converter.To<double>(row["WinLostIBC"])
                    };

                    winlossSummaryListbySportType.Add(winlossSummarybySportType);
                }
            }

            return winlossSummaryListbySportType;
        }

        public List<WinLossDetailByBettype> GetWinLossDetailListByBettype(CriteriaBase criteria)
        {
            List<WinLossDetailByBettype> winlossDetailListbySportType = new List<WinLossDetailByBettype>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                objDb.AssignParamValues(param);
                var row = objDb.ExecuteReader();
                while (row.Read())
                {
                    var winlossDetailbySportType = new WinLossDetailByBettype
                    {
                        WinLossDate = Converter.To<DateTime>(row["RPTDate"]),
                        SportID = Converter.To<int>(row["SportID"]),
                        SportName = Converter.To<string>(row["SportName"]),
                        BettypeID = Converter.To<int>(row["BetTypeID"]),
                        BettypeName = Converter.To<string>(row["BetType"]),
                        BetCount = Converter.To<int>(row["BetCount"]),
                        TurnOver = Converter.To<double>(row["TurnoverIBC"]),
                        WinLoss = Converter.To<double>(row["WinLostIBC"])
                    };

                    winlossDetailListbySportType.Add(winlossDetailbySportType);
                }
            }

            return winlossDetailListbySportType;
        }
    }
}