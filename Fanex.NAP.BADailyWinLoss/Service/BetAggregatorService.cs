namespace Fanex.NAP.BADailyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.BAMonthlyWinLoss.Business.Repository;
    using Fanex.BAMonthlyWinLoss.Business.Service;
    using Fanex.Data;
    using Fanex.NAP.BADailyWinLoss.Business.Criteria;
    using Fanex.NAP.BADailyWinLoss.Business.Model;

    public class BetAggregatorService : IBetAggregatorService
    {
        private IBetAggregatorRepository _repository;

        public BetAggregatorService()
        {
            _repository = new BetAggregatorRepository();
        }

        public BetAggregatorService(IBetAggregatorRepository repository)
        {
            _repository = repository;
        }

        public List<BALicWinLossDaily> GetWinlossDaily(DateTime date)
        {
            var listResult = new List<BALicWinLossDaily>();
            var criteria = new GetByDateCriteria() { Date = date };
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

        public List<BALicWinLossDaily> GetWinlossMonthToDate(DateTime wldate)
        {
            var listResult = new List<BALicWinLossDaily>();
            var criteria = new GetMonthToDateCriteria(wldate);
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

        public List<BALicWinLossDaily> GetWinlossYearToDate(DateTime wldate)
        {
            var listResult = new List<BALicWinLossDaily>();
            var criteria = new GetYearToDateCriteria(wldate);
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
