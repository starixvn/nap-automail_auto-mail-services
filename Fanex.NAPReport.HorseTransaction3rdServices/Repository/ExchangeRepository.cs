namespace Fanex.NAPReport.HorseTransaction3rd.Business.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Libs;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;

    public class ExchangeRepository : IExchangeRepository
    {
        public List<Exchange> GetExchange(GetExchangeCriteria criteria)
        {
            var list = new List<Exchange>();
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
            }

            return list;
        }
    }
}
