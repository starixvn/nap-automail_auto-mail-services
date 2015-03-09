// -----------------------------------------------------------------------
// <copyright file="SMAReportRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Service;
    using Fanex.SMAReport.Business.Constants;
    using Fanex.SMAReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReportRepository : ISMAReportRepository
    {
        private SqLite _db = null;
        private Dictionary<int, int> _bettype = new Dictionary<int, int>();
        private Dictionary<string, int> _enabledVirtualRateUser;

        public SMAReportRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
            InitBettype();
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
        /// <returns>SMA report data.</returns>
         [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public SMAReport GetSMAReport()
        {
            SMAReport report = new SMAReport();
            int numberSuperEnableVirtualRate = 0;
            int numberMasterEnableVirtualRate = 0;
            SMAWinLoss winLossInfo = null;
            SMAVirtualRate virtualRateInfo = null;
            using (var objectDb = new ObjectDb("SMA_Report_GetData"))
            {
                IDataReader reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    numberSuperEnableVirtualRate = Converter.ToInt(reader["VRSuperCNT"]);
                    numberMasterEnableVirtualRate = Converter.ToInt(reader["VRMasterCNT"]);
                }

                if (reader.NextResult())
                {
                    winLossInfo = this.MapWinLossReportData(reader);
                }

                if (reader.NextResult())
                {
                    virtualRateInfo = this.MapSMAVirtualRateData(reader);
                }

                report.ReportRows.Add(new SMAReport
                {
                    NumberUserEnabledVirtualRate = numberSuperEnableVirtualRate,
                    EnabledVirtualRateUser = _enabledVirtualRateUser,
                    SMAWinLoss = winLossInfo ?? new SMAWinLoss(),
                    SMAVirtualRate = virtualRateInfo ?? new SMAVirtualRate()
                });

                if (reader.NextResult())
                {
                    winLossInfo = this.MapWinLossReportData(reader);
                }

                if (reader.NextResult())
                {
                    virtualRateInfo = this.MapSMAVirtualRateData(reader);
                }

                report.ReportRows.Add(new SMAReport
                {
                    NumberUserEnabledVirtualRate = numberMasterEnableVirtualRate,
                    EnabledVirtualRateUser = _enabledVirtualRateUser,
                    SMAWinLoss = winLossInfo ?? new SMAWinLoss(),
                    SMAVirtualRate = virtualRateInfo ?? new SMAVirtualRate()
                });

                reader.Close();
            }

            return report;
        }

        /// <summary>
        /// Maps the report data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>SMA report data.</returns>
         [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private SMAWinLoss MapWinLossReportData(IDataReader reader)
        {
            Dictionary<int, string> userInfo = new Dictionary<int, string>();
            SMAWinLoss result = new SMAWinLoss();
            SMAWinLoss total = new SMAWinLoss();
            total.ReportByMonths = this.InitMonthlySMAWinLossDataByUser();
            int numberMonth = 0;
            int userIndex = 0;

            while (reader.Read())
            {
                decimal virtualRate = Converter.ToDecimal(reader["VirtualRate"]);
                string userName = Converter.ToString2(reader["Username"]);
                int month = Converter.ToInt(reader["rptMonth"]);
                SMAWinLossDetail smaReportDetail = new SMAWinLossDetail
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
                    numberMonth = result.ReportRows[currentUserIndex].ReportByMonths.Count;
                    result.ReportRows[currentUserIndex].ReportByMonths[month - 1] = smaReportDetail;
                    result.ReportRows[currentUserIndex].ReportByMonths[numberMonth - 1].CalculateTotal(smaReportDetail);
                }
                else
                {
                    userInfo.Add(userIndex, userName);
                    userIndex++;
                    var monthlySMADataReport = this.InitMonthlySMAWinLossDataByUser();
                    numberMonth = monthlySMADataReport.Count;
                    monthlySMADataReport[month - 1] = smaReportDetail;
                    monthlySMADataReport[numberMonth - 1].CalculateTotal(smaReportDetail);
                    result.ReportRows.Add(new SMAWinLoss
                    {
                        UserName = userName,
                        VirtualRate = virtualRate,
                        Currency = Converter.ToString2(reader["Currency"]),
                        ReportByMonths = monthlySMADataReport
                    });
                }
            }

            result.ReportRows.Add(total);
            List<SMAWinLoss> enabledVirtualRateUser = result.ReportRows.Where(x => x.VirtualRate == 1).OrderBy(x => x.UserName).ToList();
            MapEnabledVirtualRateUserData(enabledVirtualRateUser);
            result.ReportRows.OrderBy(x => x.UserName).ToList();
            return result;
        }

         private void MapEnabledVirtualRateUserData(List<SMAWinLoss> enabledVirtualRateUser)
         {
             int index = 0;
             _enabledVirtualRateUser = new Dictionary<string, int>();
             foreach (var user in enabledVirtualRateUser)
             {
                 _enabledVirtualRateUser.Add(user.UserName, index);
                 index++;
             }
         }

        /// <summary>
        /// Initializes the monthly SMA data by user.
        /// </summary>
        /// <returns>Initializes monthly data.</returns>
        private List<SMAWinLossDetail> InitMonthlySMAWinLossDataByUser()
        {
            List<SMAWinLossDetail> monthlySMADataReport = new List<SMAWinLossDetail>();
            int numberMonth = DateTime.Now.AddMonths(-1).Month + 1;

            for (int i = 0; i < numberMonth; i++)
            {
                monthlySMADataReport.Add(new SMAWinLossDetail
                {
                    Month = i + 1,
                    TurnOver = 0,
                    WinLoss = 0
                });
            }

            return monthlySMADataReport;
        }

        /// <summary>
        /// Maps the task data.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The task information.</returns>
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

        /// <summary>
        /// Initializes the bet type.
        /// </summary>
        private void InitBettype()
        {
            this._bettype.Add(0, BetTypeConstants.Handicap);
            this._bettype.Add(1, BetTypeConstants.OverUnder2);
            this._bettype.Add(2, BetTypeConstants.OddEvenBetType);
            this._bettype.Add(3, BetTypeConstants._1stHandicap);
            this._bettype.Add(4, BetTypeConstants._1stOverUnder);
            this._bettype.Add(5, BetTypeConstants._1stOddEvenBetType);
            this._bettype.Add(6, BetTypeConstants.MoneyLineBettype);
            this._bettype.Add(7, BetTypeConstants._1stMoneyLineBettype);
            this._bettype.Add(8, BetTypeConstants.SetXWinnerBettype);
            this._bettype.Add(9, BetTypeConstants.MatchWinnerBettype);
            this._bettype.Add(10, BetTypeConstants.SetXWinner3Rd);
            this._bettype.Add(11, BetTypeConstants.SetXGameYWinner);
        }

        /// <summary>
        /// Maps the SMA virtual rate data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>SMA virtual rate.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private SMAVirtualRate MapSMAVirtualRateData(IDataReader reader)
        {
            SMAVirtualRate report = new SMAVirtualRate();
            string preUserName = string.Empty;
            string preSportAndLiveStatus = string.Empty;
            SMAVirtualRate virtualRateByUser = null;
            SMAVirtualRate virtualRateBySportAndLiveStatus = null;
            while (reader.Read())
            {
                string userName = Converter.ToString2(reader["Username"]);
                int sportId = Converter.ToInt(reader["sporttype"]);
                int bettypeId = Converter.ToInt(reader["bettype"]);
                bool isLive = Converter.ToBool(reader["isLive"]);
                decimal virtualRate = Converter.ToDecimal(reader["VirtualRate"]);
                string sportAndLiveStatus = string.Join(string.Empty, sportId, isLive);
                string liveStatusTitle = isLive ? "Live" : "None-Live";
                int bettypeIndex = _bettype.FirstOrDefault(x => x.Value == bettypeId).Key;

                if (userName != preUserName)
                {
                    if (virtualRateBySportAndLiveStatus != null)
                    {
                        virtualRateByUser.ReportRows.Add(virtualRateBySportAndLiveStatus);
                        virtualRateBySportAndLiveStatus = null;
                    }

                    if (virtualRateByUser != null)
                    {
                        report.ReportRows.Add(virtualRateByUser);
                    }

                    virtualRateByUser = new SMAVirtualRate
                    {
                        UserName = userName
                    };

                    preUserName = userName;
                }

                if (sportAndLiveStatus != preSportAndLiveStatus)
                {
                    if (virtualRateBySportAndLiveStatus != null)
                    {
                        virtualRateByUser.ReportRows.Add(virtualRateBySportAndLiveStatus);
                    }

                    virtualRateBySportAndLiveStatus = new SMAVirtualRate
                    {
                        Sport = string.Format("{0} ({1})", GetSportName(sportId), liveStatusTitle),
                        VirtualRateByBettype = InitVirtualRateByBettypeInfo()
                    };
                    preSportAndLiveStatus = sportAndLiveStatus;
                }

                virtualRateBySportAndLiveStatus.VirtualRateByBettype[bettypeIndex].VirtualRate = virtualRate;
            }

            virtualRateByUser.ReportRows.Add(virtualRateBySportAndLiveStatus);
            report.ReportRows.Add(virtualRateByUser);
            return report;
        }

        /// <summary>
        /// Initializes the virtual rate by bet type information.
        /// </summary>
        /// <returns>The virtual rate by bet type.</returns>
        private List<VirtualRateByBettype> InitVirtualRateByBettypeInfo()
        {
            var virtualRateByBettype = new List<VirtualRateByBettype>(this._bettype.Count);
            for (int i = 0; i < _bettype.Count; i++)
            {
                var virtualRateByBettypeItem = new VirtualRateByBettype
                {
                    Bettype = CacheRef.GetBettype(_bettype[i])
                };

                virtualRateByBettype.Add(virtualRateByBettypeItem);
            }

            return virtualRateByBettype;
        }

        private string GetSportName(int sportId)
        {
            string sportName = sportId == 99 ? "Other Sports" : CacheRef.GetSportName(sportId);
            return sportName;
        }
    }
}