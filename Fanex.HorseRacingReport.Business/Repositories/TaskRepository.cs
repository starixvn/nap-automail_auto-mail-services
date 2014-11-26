// -----------------------------------------------------------------------
// <copyright file="TaskRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.HorseRacingReport.Business.Model;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private SqLite _db = null;

        public TaskRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
        }

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

        public TaskModel GetCurrentTask()
        {
            TaskModel model = null;
            try
            {
                string sql = "select * from log where Status = " + (int)Status.Running + " LIMIT 1";
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return model;
                }

                DataRow row = dataTable.Rows[0];
                model = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return model;
        }

        public TaskModel GetTaskReadyRun()
        {
            TaskModel model = null;
            try
            {
                string sql = "select * from log order by Id desc LIMIT 1";
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return model;
                }

                DataRow row = dataTable.Rows[0];
                model = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return model;
        }

        public TaskModel GetTaskById(int id)
        {
            TaskModel model = null;
            try
            {
                string sql = "select * from log where Id = " + id;
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return model;
                }

                DataRow row = dataTable.Rows[0];
                model = this.MapTaskData(row);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return model;
        }

        public int InsertTask(TaskModel task)
        {
            int id = 0;
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)Status.Running).ToString());
                data.Add("CreatedDate", DateTime.Now.ToString());

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
        private TaskModel MapTaskData(DataRow row)
        {
            TaskModel model = new TaskModel()
            {
                Id = Converter.ToInt(row["Id"]),
                TaskStatus = (Status)Converter.ToInt(row["Status"]),
                CreatedDate = Converter.ToString2(row["CreatedDate"]),
                UpdatedDate = Converter.ToString2(row["UpdatedDate"])
            };

            return model;
        }
    }
}