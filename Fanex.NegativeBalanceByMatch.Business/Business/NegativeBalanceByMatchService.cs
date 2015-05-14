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
    using Newtonsoft.Json;

    public class NegativeBalanceByMatchService : ServiceBase
    {
        private INegativeBalanceByMatchRepository _repository;
        private string LogIDUpdate = string.Empty;
        private List<object> _logData = new List<object>();

        public NegativeBalanceByMatchService()
        {
            _repository = new NegativeBalanceByMatchRepository();
        }

        public IDictionary<string, IList<Match>> GetLicenseeToMatchs()
        {
            var licenseeToMatchs = new Dictionary<string, IList<Match>>();

            string[] licensees = AppSettings.Licensees.Split(',');
            foreach (string licensee in licensees)
            {
               licenseeToMatchs.Add(licensee, GetMatchList(new Sel_BySiteCriteria {Site = licensee}));
            }

            return licenseeToMatchs;
        } 

        public void ProcessBusiness()
        {
            string[] licenseeNameList = AppSettings.Licensees.Split(',');
            var timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;

            foreach (string licenseeName in licenseeNameList)
            {
                var mailing = new Mailing();
                _logData = new List<object>();

                if (AppSettings.IsTesting || AppSettings.IsSendEmailFromConfigFileWithRealCurrency)
                {
                    mailing.To = AppSettings.MailTo;
                    mailing.Cc = AppSettings.MailCC;
                    mailing.Bcc = AppSettings.MailBCC;
                }
                else
                {
                    mailing = SelMailAddress(licenseeName);
                    if (!string.IsNullOrWhiteSpace(AppSettings.MailBCC))
                    {
                        if (string.IsNullOrWhiteSpace(mailing.Bcc))
                        {
                            mailing.Bcc = AppSettings.MailBCC;
                        }
                        else
                        {
                            mailing.Bcc += ";" + AppSettings.MailBCC;
                        }
                    }
                }

                if (mailing.IsValid)
                {
                    mailing.LicenseeName = licenseeName.ToUpper();
                    _logData.Add(mailing);

                    var subject = licenseeName.ToUpper() + " - " +
                                    string.Format(AppSettings.MailSubject, DateTime.Now, timezone > 0 ? ("+" + timezone) : timezone.ToString());
                    ProcessNegativeBalanceByMatch(licenseeName, subject, AppSettings.MailTitle, "", mailing.To, mailing.Cc, mailing.Bcc);
                }
            }
        }

        private bool ProcessNegativeBalanceByMatch(string licensee, string subject, string title, string filePathAttach,
            string tomail, string ccMail, string bccMail)
        {
            var result = false;
            var resultUpdate = new Result();
            int taskId = 0;
            try
            {
                Sel_BySiteCriteria balanceNegativeSel_BySiteCriteria = new Sel_BySiteCriteria { Site = licensee };

                List<Match> matchList = GetMatchList(balanceNegativeSel_BySiteCriteria);
                if (matchList != null && matchList.Count > 0)
                {
                    NegativeBalanceTaskInfo taskInfo = new NegativeBalanceTaskInfo
                    {
                        TaskStatus = Status.Running
                    };
                    taskId = _repository.InsertTask(taskInfo);
                    string appLocation = AppDomain.CurrentDomain.BaseDirectory;
                    string template = string.Empty;
                    if (licensee.ToLower().Equals("haifa"))
                    {
                        template = File.ReadAllText(appLocation + @"/Templates/EmailTemplateDualLanguage.cshtml");
                    }
                    else
                    {
                        template = File.ReadAllText(appLocation + @"/Templates/EmailTemplateSingleLanguage.cshtml");
                    }

                    Dictionary<object, object> dataSendEmail = new Dictionary<object, object>();
                    dataSendEmail.Add("DataSendEmail", matchList);
                    _logData.Add(dataSendEmail);

                    var content = Razor.Parse(template, matchList);

                    result = SendEmail(subject, title, tomail, ccMail, bccMail, result, taskId, content);

                    if (result)
                    {
                        UpdateLog(resultUpdate, taskId);
                    }
                }
            }
            catch (Exception ex)
            {
                var log = JsonConvert.SerializeObject(_logData);
                _repository.UpdateTaskStatus(taskId, Status.Failed, log);
                LogExtensions.ExceptionHandler(ex);
                resultUpdate.ErrorCode = 100;
                resultUpdate.ErrorMessage = ex.Message;
            }

            return result;
        }

        private void UpdateLog(Result resultUpdate, int taskId)
        {
            Upd_StatusIDCriteria updateLogCriteria = new Upd_StatusIDCriteria
            {
                LogIDs = LogIDUpdate
            };

            using (var db = new ObjectDb(updateLogCriteria.SpName))
            {
                db.ExecuteNonQuery(updateLogCriteria.GetSpParams());
                var hashResult = db.GetOuputParams();
                resultUpdate.ErrorCode = Converter.To<int>(hashResult["ErrCode"]);
                resultUpdate.ErrorMessage = Converter.ToString2(hashResult["ErrMsg"]);

                updateLogCriteria.Result = resultUpdate;
                _logData.Add(updateLogCriteria);
            }

            var log = JsonConvert.SerializeObject(_logData);

            _repository.UpdateTaskStatus(taskId, Status.Successful, log);
        }

        private bool SendEmail(string subject, string title, string tomail, string ccMail, string bccMail, bool result, int taskId, string content)
        {
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
                using (var dbUserInfor = new ObjectDb(criteria.SpName))
                {
                    DataSet dataSet_Userinfo = dbUserInfor.ExecuteDataSet(criteria.GetSpParams());
                    if (dataSet_Userinfo != null
                         && dataSet_Userinfo.Tables.Count > 0)
                    {
                        List<CustomerBalanceInfo> customerList = new List<CustomerBalanceInfo>();
                        int index = 1;
                        DataTable userTable = dataSet_Userinfo.Tables[0];

                        criteria.Result = userTable;
                        _logData.Add(criteria);

                        foreach (DataRow row in userTable.Rows)
                        {
                            LogIDUpdate = LogIDUpdate + (index == 1 ? "" : ",") + Converter.To<string>(row["LogID"]);
                            AssignCustomerBalanceInfo(customerList, row);
                            index++;
                        }

                        var customers = DistinctCustomer(customerList);
                        customerList = customers;
                        matchList = customerList.GroupBy(x => x.MatchID).Select(x => new Match
                            {
                                MatchInfo = new MatchInfo { MatchID = x.Key },
                                CustomerInfoList = x.OrderBy(y => y.TotalBalance).ToList()
                            }).ToList();

                        foreach (Match match in matchList)
                        {
                            match.LicenseeName = criteria.Site;
                            var groupIDList = match.CustomerInfoList.Select(x => x.GroupID).Distinct().ToList();

                            foreach (int groupID in groupIDList)
                            {
                                Sel_MatchLogCriteria matchLogCriteria = new Sel_MatchLogCriteria
                                {
                                    MatchID = Converter.To<int>(match.MatchInfo.MatchID),
                                    GroupID = groupID
                                };

                                using (var dbMatchInfo = new ObjectDb(matchLogCriteria.SpName))
                                {
                                    DataSet dataSetMatch = dbMatchInfo.ExecuteDataSet(matchLogCriteria.GetSpParams());
                                    if (dataSetMatch != null
                                        && dataSetMatch.Tables.Count > 0)
                                    {
                                        DataTable matchInfoTable = dataSetMatch.Tables[0];
                                        AssignMatchInfo(match, groupID, matchInfoTable);

                                        DataTable matchSettleTable = dataSetMatch.Tables[1];
                                        AssignMatchSettleLogInfo(match, groupID, matchSettleTable);

                                        var matchesLog = DistinctMatchLog(match);
                                        match.MatchSettleLogList = matchesLog;
                                    }

                                    matchLogCriteria.Result = dataSetMatch;
                                    _logData.Add(matchLogCriteria);
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

        private void AssignMatchSettleLogInfo(Match match, int groupID, DataTable matchSettleTable)
        {
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

        private void AssignMatchInfo(Match match, int groupID, DataTable matchInfoTable)
        {
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
        }

        private void AssignCustomerBalanceInfo(List<CustomerBalanceInfo> customerList, DataRow row)
        {
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
        }

        private List<MatchSettleLog> DistinctMatchLog(Match match)
        {
            var logids = match.MatchSettleLogList.Select(m => m.LogID).Distinct().ToList();
            var matchesLog = new List<MatchSettleLog>();
            foreach (var logid in logids)
            {
                var machLog = match.MatchSettleLogList.Where(m => m.LogID == logid).FirstOrDefault();
                if (machLog == null)
                {
                    continue;
                }

                matchesLog.Add(machLog);
            }

            return matchesLog;
        }

        private List<CustomerBalanceInfo> DistinctCustomer(List<CustomerBalanceInfo> customerList)
        {
            var ids = customerList.GroupBy(c => new
            {
                c.MatchID,
                c.CustID,
                c.TotalBalance
            }).Distinct().ToList();

            var customers = new List<CustomerBalanceInfo>();
            foreach (var id in ids)
            {
                var customer = customerList.Where(c =>
                    c.CustID == id.Key.CustID
                    && c.MatchID == id.Key.MatchID
                    && c.TotalBalance == id.Key.TotalBalance
                    ).FirstOrDefault();

                if (customer == null)
                {
                    continue;
                }

                customers.Add(customer);
            }

            return customers;
        }
    }
}