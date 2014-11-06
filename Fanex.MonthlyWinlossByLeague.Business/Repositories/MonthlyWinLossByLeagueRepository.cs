// -----------------------------------------------------------------------
// <copyright file="MonthlyWinLossByLeagueRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.RegularExpressions;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.MonthlyWinlossByLeague.Business.Model;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLossByLeagueRepository : IMonthlyWinLossByLeagueRepository
    {
        private List<string> _bettypes;
        private SqLite _db = null;

        public MonthlyWinLossByLeagueRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
            _bettypes = new List<string>();
        }

        public List<string> Bettypes
        {
            get
            {
                return _bettypes;
            }
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
        /// <returns>SMA report task.</returns>
        public Task GetCurrentTask()
        {
            Task model = null;
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

        /// <summary>
        /// Gets the task ready run.
        /// </summary>
        /// <returns>SMA report task.</returns>
        public Task GetTaskReadyRun()
        {
            Task model = null;
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

        /// <summary>
        /// Gets the task by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SMA report task.</returns>
        public Task GetTaskById(int id)
        {
            Task model = null;
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

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="task">The SMA task.</param>
        /// <returns>Identifier of the task.</returns>
        public int InsertTask(Task task)
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
        /// Gets the monthly win loss by league.
        /// </summary>
        /// <returns>Monthly win loss by league.</returns>
        public List<MonthlyWinLossByLeague> GetMonthlyWinLossByLeague()
        {
            List<MonthlyWinLossByLeague> monthlyWinLossByLeague = new List<MonthlyWinLossByLeague>();
            using (var objectDb = new ObjectDb("Monthly_WinLoss_ByLeague_GetReport"))
            {
                IDataReader reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    string bettype = Regex.Replace(Converter.ToString2(reader["Bet type"]), @"\r\n|\r|\n", string.Empty);
                    monthlyWinLossByLeague.Add(new MonthlyWinLossByLeague
                    {
                        Period = Converter.ToInt(reader["period"]),
                        LiveStatus = Converter.ToString2(reader["Live Status"]),
                        GroupName = Converter.ToString2(reader["Group"]),
                        League = Converter.ToString2(reader["League"]),
                        BetTypeWinLoss = new BettypeWinLoss
                        {
                            Bettype = bettype,
                            WinLoss = Converter.ToDecimal(reader["Winloss Sunbet"]),
                            Turnover = Converter.ToDecimal(reader["Turnover Sunbet"]),
                            BetCount = Converter.ToInt(reader["BetCount"]),
                            MatchCount = Converter.ToInt(reader["matchcount"])
                        }
                    });

                    if (!_bettypes.Contains(bettype) && !string.IsNullOrEmpty(bettype))
                    {
                        _bettypes.Add(bettype);
                    }
                }
            }

            return monthlyWinLossByLeague;
        }

        /// <summary>
        /// Maps the task data.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The task information.</returns>
        private Task MapTaskData(DataRow row)
        {
            Task model = new Task()
            {
                Id = Converter.ToInt(row["Id"]),
                TaskStatus = (Status)Converter.ToInt(row["Status"]),
                CreatedDate = Converter.ToString2(row["CreatedDate"]),
                TaskType = Converter.ToInt(row["Type"]),
                UpdatedDate = Converter.ToString2(row["UpdatedDate"])
            };

            return model;
        }
    }
}