// -----------------------------------------------------------------------
// <copyright file="Repository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Data;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Repository : IRepository
    {
        public DateTime GetLastDateDataAvaiable()
        {
            DateTime dateTime = DateTime.Today.AddDays(1);
            GetDWStatusByCubeCriteria criteria = new GetDWStatusByCubeCriteria();
            criteria.Cube = "DW";
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                IDataReader reader = db.ExecuteReader();
                while (reader.Read())
                {
                    dateTime = Converter.To<DateTime>(reader["LastDate"]);
                }
            }

            return dateTime;
        }
    }
}
