namespace Fanex.HorseTransaction3rdServices.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.HorseTransaction3rdServices.Criteria;
    using Fanex.HorseTransaction3rdServices.Libs;
    using Fanex.HorseTransaction3rdServices.Model;

    public class ExchangeRepository : IExchangeRepository
    {
        public List<Exchange> GetExchange(GetExchangeCriteria criteria)
        {
            var list = new List<Exchange>();
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
                            list.Add(new Exchange()
                            {
                                Currency = Convert.ToString(item["currency"]),
                                ExchangeID = Convert.ToInt32(item["exchangeid"])
                            });
                        }
                    }

                    LogService.Write("GetExchange", string.Format("--Execute Sp-->exec {0} at {1} successully", criteria.GetSettingKey(), DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                LogService.Write("GetExchange", string.Format("--Execute Sp--> exec {0} at {1} --> failed: {2}", criteria.GetSettingKey(), DateTime.Now, ex.Message));
            }

            return list;
        }
    }
}
