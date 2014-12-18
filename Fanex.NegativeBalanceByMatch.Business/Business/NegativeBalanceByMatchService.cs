namespace Fanex.NegativeBalanceByMatch.Business.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NegativeBalanceByMatch.Business.Criteria;
    using Fanex.NegativeBalanceByMatch.Business.Entities;
    using Fanex.NegativeBalanceByMatch.Business.Helper.Implement;
    using Fanex.NegativeBalanceByMatch.Business.Repositories;
    using Fanex.Service.Core.Extensions;
    using RazorEngine;
    using RemotingRef;
    using Fanex.Service.Core.Enums;

    public class NegativeBalanceByMatchService : ServiceBase
    {
        private INegativeBalanceByMatchRepository _repository;
        private string LogIDUpdate = string.Empty;

        public NegativeBalanceByMatchService()
        {
            _repository = new NegativeBalanceByMatchRepository();
        }

        private bool ProcessNegativeBalanceByMatch(string licensee, string subject, string title, string filePathAttach, string tomail, string ccMail, string bccMail)
        {
            var result = false;
            var resultUpdate = new Result();
            int taskId = 0;
            try
            {
                Sel_BySiteCriteria balanceNegativeSel_BySiteCriteria = new Sel_BySiteCriteria
                    {
                        Site = licensee,
                    };

                var matchList = GetMatchList(balanceNegativeSel_BySiteCriteria);
                if (matchList != null && matchList.Count > 0)
                {
                    NegativeBalanceTaskInfo taskInfo = new NegativeBalanceTaskInfo
                    {
                        TaskStatus = Status.Running
                    };
                    taskId = _repository.InsertTask(taskInfo);
                    string appLocation = AppDomain.CurrentDomain.BaseDirectory;
                    var template = File.ReadAllText(appLocation + @"/Templates/EmailTemplate.cshtml");
                    var content = Razor.Parse(template, matchList);

                    try
                    {
                        var mailItem = new Fanex.MailDeliver.Model.MailRecord
                        {
                            MailTo = tomail,
                            MailCc = ccMail,
                            MailBcc = bccMail,
                            Subject = subject,
                            Title = title,
                            Body = content
                        };

                        if (!string.IsNullOrEmpty(mailItem.MailTo.Trim()))
                        {
                            using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
                            {
                                client.SendMail(mailItem);
                                result = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _repository.UpdateTaskStatus(taskId, Status.Failed);
                        result = false;
                        LogExtensions.ExceptionHandler(ex);
                    }

                    if (result)
                    {
                        Upd_StatusIDCriteria updateLogCriteria = new Upd_StatusIDCriteria
                            {
                                LogIDs = LogIDUpdate
                            };

                        using (var db = new ObjectDb(updateLogCriteria.GetSettingKey()))
                        {
                            db.ExecuteNonQuery(updateLogCriteria.GetSpParams());
                            var hashResult = db.GetOuputParams();
                            resultUpdate.ErrorCode = Converter.To<int>(hashResult["ErrCode"]);
                            resultUpdate.ErrorMessage = Converter.ToString2(hashResult["ErrMsg"]);
                        }

                        _repository.UpdateTaskStatus(taskId, Status.Successful);
                    }
                }
            }
            catch (Exception ex)
            {
                _repository.UpdateTaskStatus(taskId, Status.Failed);
                LogExtensions.ExceptionHandler(ex);
                resultUpdate.ErrorCode = 100;
                resultUpdate.ErrorMessage = ex.Message;
            }

            return result;
        }

        private Mailing SelMailAddress(string licensee)
        {
            DataTable dt = new DataTable();
            var mailing = new Mailing();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_MailingListSel"))
                {
                    DataSet ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }

                DataRow[] rows = dt.Select("site='" + licensee + "'");
                foreach (var dataRow in rows)
                {
                    mailing = new Mailing()
                                                 {
                                                     LicenseeName = licensee,
                                                     To = dataRow["tolist"].ToString(),
                                                     Cc = dataRow["cclist"].ToString(),
                                                     Bcc = dataRow["bcclist"].ToString()
                                                 };
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return mailing;
        }

        private List<Match> GetMatchList(Sel_BySiteCriteria criteria)
        {
            List<Match> matchList = new List<Match>();
            LogIDUpdate = string.Empty;

            try
            {
                using (var dbUserInfor = new ObjectDb(criteria.GetSettingKey()))
                {
                    DataSet dataSet_Userinfo = dbUserInfor.ExecuteDataSet(criteria);
                    if (dataSet_Userinfo != null
                         && dataSet_Userinfo.Tables.Count > 0)
                    {
                        List<CustomerBalanceInfo> customerList = new List<CustomerBalanceInfo>();
                        int index = 1;
                        DataTable userTable = dataSet_Userinfo.Tables[0];
                        foreach (DataRow row in userTable.Rows)
                        {
                            LogIDUpdate = LogIDUpdate + (index == 1 ? "" : ",") + Converter.To<string>(row["LogID"]);
                            customerList.Add(new CustomerBalanceInfo
                            {
                                LogID = Converter.To<int>(row["LogID"]),
                                LogDate = Converter.To<DateTime>(row["LogDate"]),
                                MatchID = Converter.To<string>(row["MatchID"]),
                                GroupID = Converter.To<int>(row["GroupID"]),
                                CustID = Converter.To<string>(row["CustID"]),
                                Username = Converter.To<string>(row["Username"]),
                                TotalBalance = Converter.To<double>(row["TotalBalance"]),
                                CurrencyName = Converter.To<string>(row["CurrencyName"]),
                                Site = Converter.To<string>(row["Site"]),
                                CustomInfo1 = Converter.To<string>(row["CustomInfo1"])
                            });
                            index++;
                        }

                        matchList = customerList.GroupBy(x => x.MatchID).Select(x => new Match
                            {
                                MatchInfo = new MatchInfo { MatchID = x.Key },
                                CustomerInfoList = x.OrderBy(y => y.TotalBalance).ToList()
                            }).ToList();

                        foreach (Match match in matchList)
                        {
                            var groupIDList = match.CustomerInfoList.Select(x => x.GroupID).Distinct().ToList();

                            foreach (int groupID in groupIDList)
                            {
                                Sel_MatchLogCriteria matchLogCriteria = new Sel_MatchLogCriteria
                                {
                                    MatchID = Converter.To<int>(match.MatchInfo.MatchID),
                                    GroupID = groupID
                                };

                                using (var dbMatchInfo = new ObjectDb(matchLogCriteria.GetSettingKey()))
                                {
                                    DataSet dataSetMatch = dbMatchInfo.ExecuteDataSet(matchLogCriteria);
                                    if (dataSetMatch != null
                                        && dataSetMatch.Tables.Count > 0)
                                    {
                                        DataTable matchInfoTable = dataSetMatch.Tables[0];

                                        foreach (DataRow row in matchInfoTable.Rows)
                                        {
                                            match.MatchInfo.MatchID = Converter.ToString2(row["MatchID"]);
                                            match.MatchInfo.LeagueID = Converter.To<int>(row["LeagueID"]);
                                            match.MatchInfo.LeagueName = RefData.GetLeagueName(row["LeagueID"].ToString(), "en");
                                            match.MatchInfo.MatchCode = Converter.ToString2(row["MatchCode"]);
                                            match.MatchInfo.EventDate = Converter.To<DateTime>(row["EventDate"]);
                                            match.MatchInfo.HomeName = RefData.GetTeamName(row["HomeID"].ToString(), "en");
                                            match.MatchInfo.AwayName = RefData.GetTeamName(row["AwayID"].ToString(), "en");
                                            match.MatchInfo.GroupId = groupID;
                                        }

                                        DataTable matchSettleTable = dataSetMatch.Tables[1];
                                        foreach (DataRow row in matchSettleTable.Rows)
                                        {
                                            if (groupID == 3)
                                            {
                                                match.MatchSettleLogList.Add(
                                                    new MatchSettleLog
                                                    {
                                                        LogID = Converter.To<int>(row["logid"]),
                                                        SettlementTime = Converter.To<DateTime>(row["logdate"]),
                                                        Action = Converter.ToString2(row["ActionNameEN"]),
                                                        Result = Converter.ToString2(row["Result"]),
                                                        Remark = Converter.ToString2(row["Remark"])
                                                    });
                                            }
                                            else
                                            {
                                                match.MatchSettleLogList.Add(
                                                    new MatchSettleLog
                                                    {
                                                        LogID = Converter.To<int>(row["logid"]),
                                                        SettlementTime = Converter.To<DateTime>(row["logdate"]),
                                                        Action = Converter.ToString2(row["ActionNameEN"]),
                                                        FinalHomeScore = Converter.To<int>(row["finalhomescore"]),
                                                        FinalAwayScore = Converter.To<int>(row["finalawayscore"]),
                                                        HTHomeScore = Converter.To<int>(row["hthomescore"]),
                                                        HTAwayScore = Converter.To<int>(row["htawayscore"]),
                                                    });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return matchList;
        }

        public void ProcessBusiness()
        {
            string licensee = AppSettings.Licensees;
            string[] licenseeNameList = licensee.Split(',');
            var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            foreach (string licenseeName in licenseeNameList)
            {
                var mailing = new Mailing();

                bool isTesting = AppSettings.IsTesting;
                if (isTesting)
                {
                    mailing.To = AppSettings.MailTo;
                    mailing.Cc = AppSettings.MailCC;
                    mailing.Bcc = AppSettings.MailBCC;
                }
                else
                {
                    mailing = SelMailAddress(licenseeName);
                }

                if (mailing.IsValid)
                {
                    var subject = licenseeName.ToUpper() + " - " +
                    string.Format(AppSettings.MailSubject, DateTime.Now, timezone > 0 ? ("+" + timezone) : timezone.ToString());
                    ProcessNegativeBalanceByMatch(licenseeName, subject, AppSettings.MailTitle, "", mailing.To, mailing.Cc, mailing.Bcc);
                }
            }
        }
    }
}