// -----------------------------------------------------------------------
// <copyright file="TurnoverWatcherRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.TurnoverWatcher.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.TurnoverWatcher.Business.Criteria;
    using Fanex.TurnoverWatcher.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverWatcherRepository : ITurnoverWatcherRepository
    {
        private SqLite _db = null;

        public TurnoverWatcherRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
        }

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="task">The SMA task.</param>
        /// <returns>Identifier of the task.</returns>
        public int InsertTask(TurnoverWatcherTask task)
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
        /// Updates the task.
        /// </summary>
        /// <param name="task">The task be updated.</param>
        /// <param name="status">The status.</param>
        public void UpdateTask(TurnoverWatcherTask task, Status status)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)status).ToString());
                data.Add("MailReceiver", task.EmailReceiver);
                data.Add("UpdatedDate", DateTime.Now.ToString());

                string where = " Id = " + task.Id;
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
        /// <returns>The current task.</returns>
        public TurnoverWatcherTask GetCurrentTask()
        {
            TurnoverWatcherTask task = null;
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
        /// Mailings the receiver.
        /// </summary>
        /// <param name="functionId">The function identifier.</param>
        /// <returns>The receiver.</returns>
        public EmailReceiver MailingReceiver(int functionId)
        {
            EmailReceiver receiver = new EmailReceiver();
            var criteria = new EmailReceiverCriteria { FuncId = functionId };

            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDB.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    receiver.To = Convert.ToString(reader["tolist"]);
                    receiver.Cc = Convert.ToString(reader["cclist"]);
                    receiver.Bcc = Convert.ToString(reader["bcclist"]);
                }

                reader.Close();
            }

            return receiver;
        }

        /// <summary>
        /// Gets the turn over information.
        /// </summary>
        /// <returns>The turn over information.</returns>
        public List<TurnoverInfo> GetTurnOverInfo()
        {
            var customerTurnovers = new List<TurnoverInfo>();
            var criteria = new TurnoverNotifiedSelectCriteria();

            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDB.ExecuteReader();
                while (reader.Read())
                {
                    MatchInfo matchInfo = new MatchInfo
                    {
                        MatchId = Convert.ToInt32(reader["matchid"]),
                        HomeId = Convert.ToInt32(reader["homeid"]),
                        AwayId = Convert.ToInt32(reader["awayid"]),
                        LeagueId = Convert.ToInt32(reader["leagueid"]),
                        EventDate = Convert.ToDateTime(reader["eventdate"]),
                        SportType = Convert.ToInt32(reader["sporttype"])
                    };

                    CustomerInfo customerInfo = new CustomerInfo
                    {
                        CustomerName = Convert.ToString(reader["username"]),
                        Currency = Convert.ToString(reader["currency"]),
                        MaxTurnover = Convert.ToDecimal(reader["maxturnover"]),
                        MemberTurnover = Convert.ToDecimal(reader["totalturnover"]),
                        CompanyTurnover = Convert.ToDecimal(reader["houseturnover"]),
                        CustId = Convert.ToInt32(reader["custid"])
                    };

                    customerTurnovers.Add(
                        new TurnoverInfo
                        {
                            CustomerInfo = customerInfo,
                            MatchInfo = matchInfo,
                            SendMailLevel = Convert.ToInt32(reader["nlevel"])
                        });
                }

                reader.Close();
            }

            return customerTurnovers;
        }

        /// <summary>
        /// Removes the notified customer.
        /// </summary>
        /// <param name="notifiedCustomer">The notified customer.</param>
        /// <param name="sendMailLevel">The send mail level.</param>
        /// <returns>The result of remove action.</returns>
        public Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel)
        {
            var criteria = new TurnoverNotifiedUpdateCriteria
            {
                SendMailLevel = sendMailLevel
            };

            foreach (var customerTurnoverInfo in notifiedCustomer)
            {
                criteria.Matchids += customerTurnoverInfo.MatchInfo.MatchId + ",";
                criteria.Supers += customerTurnoverInfo.CustomerInfo.CustId + ",";
            }

            var result = new Result();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var hashResult = db.GetOuputParams();
                result.ErrorCode = Convert.ToInt32(hashResult["ErrCode"]);
                result.ErrorMessage = Convert.ToString(hashResult["ErrMsg"]);
            }

            return result;
        }

        /// <summary>
        /// Maps the task data.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>Turn over watcher task.</returns>
        private TurnoverWatcherTask MapTaskData(DataRow row)
        {
            TurnoverWatcherTask task = new TurnoverWatcherTask()
            {
                Id = Converter.ToInt(row["Id"]),
                TaskStatus = (Status)Converter.ToInt(row["Status"]),
                CreatedDate = Converter.ToString2(row["CreatedDate"]),
                EmailReceiver = Converter.ToString2(row["MailReceiver"]),
                UpdatedDate = Converter.ToString2(row["UpdatedDate"])
            };

            return task;
        }
    }
}