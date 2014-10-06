// -----------------------------------------------------------------------
// <copyright file="SMAReportRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Service.Core.Extensions;
    using Fanex.SMAReport.Business.Enums;
    using Fanex.SMAReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReportRepository : ISMAReportRepository
    {
        private SqLite _db = null;

        public SMAReportRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
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
        public SMAReportModel GetCurrentTask()
        {
            SMAReportModel model = null;
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
        public SMAReportModel GetTaskReadyRun()
        {
            SMAReportModel model = null;
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
        public SMAReportModel GetTaskById(int id)
        {
            SMAReportModel model = null;
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
        public int InsertTask(SMAReportModel task)
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
        /// Gets the SMA report.
        /// </summary>
        /// <returns>SMA report.</returns>
        public SMAReport GetSMAReport()
        {
            var report = new SMAReport();

            using (var objectDb = new ObjectDb("SMA_Report_GetData"))
            {
                IDataReader reader = objectDb.ExecuteReader();
                var superReportData = this.MapReportData(reader);
                report.Report.Add(superReportData);

                if (reader.NextResult())
                {
                    var masterReportData = this.MapReportData(reader);
                    report.Report.Add(masterReportData);
                }
            }

            return report;
        }

        /// <summary>
        /// Maps the report data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>SMA report data.</returns>
        private SMAReport MapReportData(IDataReader reader)
        {
            Dictionary<int, string> userInfo = new Dictionary<int, string>();
            SMAReport result = new SMAReport();
            SMAReport total = new SMAReport();
            total.ReportByMonths = this.InitMonthlySMADataByUser();
            int numberMonth = 0;
            int userIndex = 0;

            while (reader.Read())
            {
                string userName = Converter.ToString2(reader["Username"]);
                int month = Converter.ToInt(reader["rptMonth"]);
                SMAReportDetail smaReportDetail = new SMAReportDetail
                    {
                        Month = month,
                        TurnOver = Converter.ToDecimal(reader["TurnOver"]),
                        WinLoss = Converter.ToDecimal(reader["Winloss"])
                    };
                total.ReportByMonths[month - 1].CalculateTotal(smaReportDetail);
                total.ReportByMonths[total.ReportByMonths.Count - 1].CalculateTotal(smaReportDetail);
                var userIsExist = userInfo.ContainsValue(userName);

                if (userIsExist)
                {
                    int currentUserIndex = userInfo.FirstOrDefault(x => x.Value == userName).Key;
                    numberMonth = result.Report[currentUserIndex].ReportByMonths.Count;
                    result.Report[currentUserIndex].ReportByMonths[month - 1] = smaReportDetail;
                    result.Report[currentUserIndex].ReportByMonths[numberMonth - 1].CalculateTotal(smaReportDetail);
                }
                else
                {
                    userInfo.Add(userIndex, userName);
                    userIndex++;
                    var monthlySMADataReport = this.InitMonthlySMADataByUser();
                    numberMonth = monthlySMADataReport.Count;
                    monthlySMADataReport[month - 1] = smaReportDetail;
                    monthlySMADataReport[numberMonth - 1].CalculateTotal(smaReportDetail);
                    result.Report.Add(new SMAReport
                    {
                        UserName = userName,
                        VirtualRate = Converter.ToDecimal(reader["VirtualRate"]) / 100,
                        Currency = Converter.ToString2(reader["Currency"]),
                        ReportByMonths = monthlySMADataReport
                    });
                }
            }

            result.Report.Add(total);
            return result;
        }

        /// <summary>
        /// Initializes the monthly SMA data by user.
        /// </summary>
        /// <returns>Initializes monthly data.</returns>
        private List<SMAReportDetail> InitMonthlySMADataByUser()
        {
            List<SMAReportDetail> monthlySMADataReport = new List<SMAReportDetail>();
            int currentMonth = DateTime.Now.Month;

            for (int i = 0; i < currentMonth; i++)
            {
                monthlySMADataReport.Add(new SMAReportDetail
                {
                    Month = i + 1,
                    TurnOver = 0,
                    WinLoss = 0
                });
            }

            return monthlySMADataReport;
        }

        private SMAReportModel MapTaskData(DataRow row)
        {
            SMAReportModel model = new SMAReportModel()
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