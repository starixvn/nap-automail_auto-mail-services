// -----------------------------------------------------------------------
// <copyright file="SQLiteDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.OandaExChangeRate.Business.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OandaExChangeRate.Business.Enums;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SQLiteDataAccess
    {
        private SqLite _db = null;

        public SQLiteDataAccess()
        {
            _db = new SqLite(AppSettings.ConnectionString);
        }

        /// <summary>
        /// Inserts the specified type.
        /// </summary>
        /// <param name="type">The type of task.</param>
        /// <returns>Identify of task.</returns>
        public int Insert(TaskType type)
        {
            int id = 0;
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)Status.Running).ToString());
                data.Add("CreatedDate", DateTime.Now.ToString());
                data.Add("Type", ((int)type).ToString());
                data.Add("CountRunner", (0).ToString());

                id = Convert.ToInt32(_db.Insert("Log", data));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateStatus(int id, Status status)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)status).ToString());
                data.Add("UpdatedDate", DateTime.Now.ToString());

                string where = " Id = " + id;
                _db.Update("Log", data, where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the Count Runner.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateCountRunner(int id, int countRunner)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("CountRunner", countRunner.ToString());

                string where = " Id = " + id;
                _db.Update("Log", data, where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTodayLogId()
        {
            int Id = -1;
            try
            {
                var sql = "select * from Log where ID in (select max(ID) from Log)";
                var dataTable = _db.GetDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    if (Convert.ToDateTime(row["CreatedDate"]).ToShortDateString() == DateTime.Today.ToShortDateString()
                        || Convert.ToInt32(row["CountRunner"]) < AppSettings.CountRunner)
                    {
                        Id = Converter.ToInt(row["Id"]);
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Id;
        }

        public int GetCurrentCountRunner(int id)
        {
            int countRunner = 0;
            try
            {
                var sql = "select * from Log where ID = " + id.ToString();
                var log = _db.GetDataTable(sql);
                if (log.Rows.Count > 0)
                {
                    countRunner = Converter.ToInt(log.Rows[0]["CountRunner"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return countRunner;
        }

        public DateTime GetLastUpdatedDate(int id)
        {
            DateTime updatedDate = DateTime.Today;
            try
            {
                var sql = "select * from Log where ID = " + id.ToString();
                var dataTable = _db.GetDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    if (row["UpdatedDate"] != DBNull.Value && Convert.ToDateTime(row["UpdatedDate"]).ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        updatedDate = Convert.ToDateTime(row["UpdatedDate"]);
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return updatedDate;
        }

        public void UpdateCurrentCurrency(int id, string currentCurrency)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("CurrentCurrency", currentCurrency); 

                string where = " Id = " + id;
                _db.Update("Log", data, where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
