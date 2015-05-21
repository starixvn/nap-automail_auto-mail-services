namespace Fanex.BAMonthlyWinLoss.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.BAMonthlyWinLoss.Business.Model;
    using Fanex.Data;

    public class BetAggregatorRepository : IBetAggregatorRepository
    {
        public List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria)
        {
            var listResult = new List<BALicWinlossDaily>();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataTable = objectDb.ExecuteDataSet(criteria.GetSpParams());
                if (dataTable != null && dataTable.Tables.Count > 0)
                {
                    foreach (DataRow row in dataTable.Tables[0].Rows)
                    {
                        listResult.Add(new BALicWinlossDaily
                        {
                            LicenseeName = Convert.ToString(row["LicenseeName"]),
                            LicenseeID = Convert.ToInt32(row["LicenseeID"]),
                            Currency = Convert.ToInt32(row["Currency"]),
                            CurrencyName = Convert.ToString(row["CurrencyName"]),
                            TurnOverRM = Convert.ToDouble(row["turnoverRM"]),
                            WinlostRM = Convert.ToDouble(row["WinlostRM"]),
                            AmtDueToBARM = Convert.ToDouble(row["AmtDueToBARM"]),
                            CommRM = Convert.ToDouble(row["CommRM"]),
                            BetCount = Convert.ToInt32(row["BetCount"]),
                            WinlostDate = Convert.ToDateTime(row["winlostdate"]),
                        });
                    }
                }
            }

            return listResult;
        }
    }
}
