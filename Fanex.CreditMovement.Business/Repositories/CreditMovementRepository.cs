// -----------------------------------------------------------------------
// <copyright file="CreditMovementRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.Data;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// Credit movement repository.
    /// </summary>
    public class CreditMovementRepository : ICreditMovementRepository
    {
        private SqLite _db = null;

        public CreditMovementRepository() { }

        /// <summary>
        /// Gets the report data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Credit movement report data.</returns>
        public List<CreditMovement> GetReportData(GetCreditMovementCriteria criteria)
        {
            List<CreditMovement> report = new List<CreditMovement>();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    string downLineInformation = string.Empty;
                    string superName = Converter.ToString2(reader["SuperName"]);
                    downLineInformation = string.IsNullOrEmpty(superName) ? downLineInformation : superName;
                    string masterName = Converter.ToString2(reader["MasterName"]);
                    downLineInformation = string.IsNullOrEmpty(masterName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, masterName);
                    string agentName = Converter.ToString2(reader["AgentName"]);
                    downLineInformation = string.IsNullOrEmpty(agentName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, agentName);
                    string memberName = Converter.ToString2(reader["CustomerName"]);
                    downLineInformation = string.IsNullOrEmpty(memberName) ? downLineInformation : string.Format("{0} / {1}", downLineInformation, memberName);
                    report.Add(new CreditMovement()
                    {
                        Date = Converter.ToDateTime(reader["TransDate"]).ToShortFormatedDate(),
                        Currency = Converter.ToString2(reader["Currency"]),
                        Site = Converter.ToString2(reader["Site"]),
                        DownLinesInformation = downLineInformation,
                        OriginalCurrency = Converter.ToDecimal(reader["Amount"]),
                        BaseCurrency = Converter.ToDecimal(reader["BaseAmount"])
                    });
                }

                reader.Close();
            }

            return report;
        }

        /// <summary>
        /// Updates the task status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateTaskStatus(int id, Status status)
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
        /// Gets the current task.
        /// </summary>
        /// <returns>The task information.</returns>
        public TaskInformation GetCurrentTask()
        {
            TaskInformation task = null;
            try
            {
                string sql = "select * from log where Status = " + (int)Status.Running + " LIMIT 1";
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return task;
                }

                DataRow row = dataTable.Rows[0];
                task = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return task;
        }

        /// <summary>
        /// Gets the task ready run.
        /// </summary>
        /// <returns>The task information.</returns>
        public TaskInformation GetTaskReadyRun()
        {
            TaskInformation task = null;
            try
            {
                string sql = "select * from log order by Id desc LIMIT 1";
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return task;
                }

                DataRow row = dataTable.Rows[0];
                task = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return task;
        }

        /// <summary>
        /// Gets the task by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The task information.</returns>
        public TaskInformation GetTaskById(int id)
        {
            TaskInformation task = null;
            try
            {
                string sql = "select * from log where Id = " + id;
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return task;
                }

                DataRow row = dataTable.Rows[0];
                task = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return task;
        }

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="task">The SMA task.</param>
        /// <returns>Identifier of the task.</returns>
        public int InsertTask(TaskInformation task)
        {
            int id = 0;
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)Status.Running).ToString());
                data.Add("CreatedDate", DateTime.Now.ToString());
                data.Add("Type", task.TaskType.ToString());

                id = Converter.ToInt(_db.Insert("Log", data));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return id;
        }

        /// <summary>
        /// Maps the task data.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The task information.</returns>
        private TaskInformation MapTaskData(DataRow row)
        {
            TaskInformation task = new TaskInformation()
            {
                Id = Converter.ToInt(row["Id"]),
                TaskStatus = (Status)Converter.ToInt(row["Status"]),
                CreatedDate = Converter.ToString2(row["CreatedDate"]),
                TaskType = Converter.ToString2(row["Type"]),
                UpdatedDate = Converter.ToString2(row["UpdatedDate"])
            };

            return task;
        }

        /// <summary>
        /// Gets the site identifier.
        /// </summary>
        /// <returns>The list site identifier.</returns>
        public List<string> GetSiteId()
        {
            List<string> sites = new List<string>();
            using (ObjectDb objectDb = new ObjectDb("Misc_CreditMovement_GetSites"))
            {
                IDataReader reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    sites.Add(Converter.ToString2(reader["siteid"]));
                }

                reader.Close();
            }

            return sites;
        }
    }
}