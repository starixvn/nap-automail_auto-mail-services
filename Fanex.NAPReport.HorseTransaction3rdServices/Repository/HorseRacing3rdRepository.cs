namespace Fanex.NAPReport.HorseTransaction3rd.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Libs;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;

    public class HorseRacing3rdRepository : IHorseRacing3rdRepository
    {
        public bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria.GetSpParams());
                return true;
            }
        }

        public List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria)
        {
            var list = new List<RacingTote>();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataSet = objectDb.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    var dataTable = dataSet.Tables[0];
                    foreach (DataRow item in dataTable.Rows)
                    {
                        list.Add(new RacingTote()
                        {
                            Currency = Convert.ToInt32(item["poolcurrency"]),
                            DividendWon_PoolCurr = Convert.ToDecimal(item["dividendWon_poolcurr"]),
                            TotalTO_Partner_PoolCurr = Convert.ToDecimal(item["totalto_partner_poolcurr"])
                        });
                    }
                }
            }

            return list;
        }

        public List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria)
        {
            var list = new List<RacingTo3rd>();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataSet = objectDb.ExecuteDataSet(criteria.GetSpParams());
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    var dataTable = dataSet.Tables[0];
                    foreach (DataRow item in dataTable.Rows)
                    {
                        list.Add(new RacingTo3rd()
                        {
                            Currency = Convert.ToString(item["currency"]),
                            To3rd = Convert.ToDecimal(item["to3rd"]),
                            DividendWon3rd = Convert.ToDecimal(item["dividendwon3rd"])
                        });
                    }
                }
            }

            return list;
        }
    }
}
