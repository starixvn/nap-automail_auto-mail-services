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
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.TurnoverWatcher.Business.Criteria;
    using Fanex.TurnoverWatcher.Business.Model;

    public class TurnoverWatcherRepository : ITurnoverWatcherRepository
    {
        public TurnoverWatcherRepository()
        {
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