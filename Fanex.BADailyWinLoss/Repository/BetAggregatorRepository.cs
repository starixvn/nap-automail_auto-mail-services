namespace Fanex.BAMonthlyWinLoss.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.BADailyWinLoss.Business.Criteria;
    using Fanex.BADailyWinLoss.Business.Model;
    using Fanex.Data;

    public class BetAggregatorRepository : IBetAggregatorRepository
    {
        public List<BALicWinLossDaily> GetWinlossDaily(GetByDateCriteria criteria)
        {
            var listResult = new List<BALicWinLossDaily>();            
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var ds = db.ExecuteDataSet(criteria.GetSpParams());
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dataTable = ds.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        listResult.Add(ToWinLossDaily(row));
                    }
                }
            }

            return listResult;
        }

        public List<BALicWinLossDaily> GetWinlossMonthToDate(GetMonthToDateCriteria criteria)
        {
            var listResult = new List<BALicWinLossDaily>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var ds = db.ExecuteDataSet(criteria.GetSpParams());
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dataTable = ds.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        listResult.Add(ToWinLossDaily(row));
                    }
                }
            }

            return listResult;
        }

        public List<BALicWinLossDaily> GetWinlossYearToDate(GetYearToDateCriteria criteria)
        {
            var listResult = new List<BALicWinLossDaily>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var ds = db.ExecuteDataSet(criteria.GetSpParams());
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dataTable = ds.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        listResult.Add(ToWinLossDaily(row));
                    }
                }
            }

            return listResult;
        }

        private BALicWinLossDaily ToWinLossDaily(DataRow row)
        {
            var winLoss = new BALicWinLossDaily()
            {
                LicenseeName = Convert.ToString(row["LicenseeName"]),
                LicenseeID = Convert.ToInt32(row["LicenseeID"]),
                Currency = Convert.ToInt32(row["Currency"]),
                CurrencyName = Convert.ToString(row["CurrencyName"]),
                TurnOverRM = Convert.ToDouble(row["turnoverRM"]),
                WinlostRM = Convert.ToDouble(row["WinlostRM"]),
                AmtDueToBARM = Convert.ToDouble(row["AmtDueToBARM"]),
                CommRM = Convert.ToDouble(row["CommRM"]),
                BetCount = Convert.ToInt32(row["BetCount"])
            };

            try
            {
                winLoss.LiveIndicator = Convert.ToBoolean(row["liveindicator"]);
            }
            catch
            {
                winLoss.LiveIndicator = false;
            }

            return winLoss;
        }
    }
}
