// -----------------------------------------------------------------------
// <copyright file="VirtualSportRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.NAPReport.VirtualSportWinLoss.Business;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Enums;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VirtualSportRepository : IVirtualSportRepository
    {
        private SqLite _db = null;

        public VirtualSportRepository()
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

                id = Convert.ToInt32(_db.Insert("Log", data));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
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
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the log not enough data to send email.
        /// </summary>
        /// <returns>Log data table.</returns>
        public DataTable GetLogNotCompleteTask()
        {
            DataTable log = new DataTable();
            try
            {
                var sql = "select * from Log where ID in( select max(ID) from Log) and Status = " + ((int)Status.SentEmailFailure).ToString();
                log = _db.GetDataTable(sql);
                if (log.Rows.Count > 0)
                {
                    //// Just get today log
                    if (Convert.ToDateTime(log.Rows[0]["CreatedDate"]).ToShortDateString() != DateTime.Today.ToShortDateString())
                    {
                        log = new DataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return log;
        }

        /// <summary>
        /// Determines whether [has task already run today].
        /// </summary>
        /// <returns>Task has already.</returns>
        public bool HasTaskAlreadyRunToday()
        {
            bool already = false;

            var sql = "select * from Log where ID in( select max(ID) from Log) and Type = " + ((int)TaskType.Daily).ToString();
            var dataTable = _db.GetDataTable(sql);
            if (dataTable.Rows.Count > 0)
            {
                already = dataTable.Rows[0]["CreatedDate"] == DBNull.Value ?
                    false :
                    (Convert.ToDateTime(dataTable.Rows[0]["CreatedDate"]).ToShortDateString() == DateTime.Today.ToShortDateString());
            }

            return already;
        }
    }
}