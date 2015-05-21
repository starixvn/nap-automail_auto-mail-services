namespace Fanex.NAP.HorseTransaction3rd.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.NAP.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAP.HorseTransaction3rd.Business.Libs;
    using Fanex.NAP.HorseTransaction3rd.Business.Model;

    public class HorseRacing3rdRepository : IHorseRacing3rdRepository
    {
        public bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(criteria.GetSpParams());

                    LogService.Write(
                        "InsertDataFromFileToDb",
                        string.Format("--Execute Sp--> {0} at {1} successully", criteria.GetSettingKey(), DateTime.Now));

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogService.Write(
                    "InsertDataFromFileToDb",
                    string.Format("<span color='red'>--Execute Sp--> {0} at {1} failed: {2}</span>", criteria.GetSettingKey(), DateTime.Now, ex.Message));
                throw;
            }
        }

        public List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria)
        {
            var list = new List<RacingTote>();
            try
            {
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

                    LogService.Write(
                        "WinlostHR_OpenTote",
                    string.Format("--Execute Sp--> exec {0}  @winlostdate='{1}' ---- at {2}", criteria.GetSettingKey(), criteria.WinLostDate, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                LogService.Write(
                    "WinlostHR_OpenTote",
                    string.Format("<span color='red'>--Execute Sp--> {0} at {1} failed: {2}</span>", criteria.GetSettingKey(), DateTime.Now, ex.Message));
            }

            return list;
        }

        public List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria)
        {
            var list = new List<RacingTo3rd>();
            try
            {
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

                    LogService.Write(
                        "Horse_OpenToteTO3rd",
                        string.Format("--Execute Sp--> exec {0} @winlostdate='{1}' ---- at {2}", criteria.GetSettingKey(), criteria.WinLostDate, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                LogService.Write(
                    "WinlostHR_OpenTote",
                    string.Format("<span color='red'>--Execute Sp--> {0} at {1} failed: {2}</span>", criteria.GetSettingKey(), DateTime.Now, ex.Message));
            }

            return list;
        }
    }
}
