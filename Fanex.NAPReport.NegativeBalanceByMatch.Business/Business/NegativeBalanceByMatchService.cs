namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.Business.Criteria;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Criteria;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Implement;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Model;
    using RazorEngine;
    using RemotingRef;

    public class NegativeBalanceByMatchService : ServiceBase, IStandaloneTimerFunctionMailService
    {
        private readonly ILicenseeService _licenseeService;
        private readonly string _templateBaseDir;
        private string _logIdUpdate = string.Empty;

        // State data
        private IDictionary<LicenseeFunction, string> _sentLicenseeFunctionToSubject;

        private IMailStationService _mailStation;
        private MailInfo _mailInfo;

        public NegativeBalanceByMatchService(ILicenseeService licenseeService, string templateBaseDir = "")
        {
            _licenseeService = licenseeService;
            _templateBaseDir = templateBaseDir;
            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();
            _sentLicenseeFunctionToSubject = new Dictionary<LicenseeFunction, string>();
        }

        public override bool IsActive()
        {
            return AppSettings.IsActive;
        }

        public override void ResetState()
        {
            _sentLicenseeFunctionToSubject = new Dictionary<LicenseeFunction, string>();
        }

        public override bool NotifyMailVipGroup()
        {
            DateTime curDateTime = DateTime.Now;
            ExportSettings exportSettings = new ExportSettings();

            foreach (LicenseeFunction licenseeFunction in _licenseeService.GetLicenseeFunctions())
            {
                string licenseeName = licenseeFunction.LicenseeName;
                try
                {
                    List<Match> matches = GetMatchList(new Sel_BySiteCriteria { Site = licenseeName });

                    if (matches != null && matches.Count > 0)
                    {
                        string body = GetMailVipGroupBody(licenseeName, matches);

                        INegativeBalanceByMatchExcelService excel = new NegativeBalanceByMatchExcelService();
                        string fileNamePath = Path.Combine(AppSettings.ExportDataDirectoryWithYearMonthDay, licenseeName + "_List_Of_Members_With_Negative_Balance_By_Match_Report_For_" + curDateTime.ToString("yyyy_MM_dd_hh_mm_tt") + ".xlsx");
                        excel.GenerateReportForLicensee(licenseeName, matches, exportSettings, fileNamePath);

                        var fileNamesPath = new List<string>();
                        fileNamesPath.Add(fileNamePath);

                        _mailInfo = new MailInfo()
                        {
                            Subject = string.Format(AppSettings.Subject, licenseeName, DateTime.Now.ToString("yyyy/MM/dd")),
                            FileName = Path.GetFileName(fileNamePath),
                            FileNamesPath = fileNamesPath
                        };

                        var mail = _mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, body);
                        mail.MailingListId = licenseeFunction.Function.IdVipGroup;
                        MailClientHandler.SendMail(mail);

                        if (AppSettings.IsUpdateStatusSendingEmail)
                        {
                            UpdateLog();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                }
            }

            return _sentLicenseeFunctionToSubject.Count != 0;
        }

        public override void NotifyMailNormalGroup()
        {
        }

        private string GetMailVipGroupBody(string licenseeName, IList<Match> matches)
        {
            string template = IsDualLanguage(licenseeName)
                                    ? File.ReadAllText(Path.Combine(_templateBaseDir, @"SiteConfig\NegativeBalanceByMatch\Templates\EmailTemplateDualLanguage.cshtml"))
                                    : File.ReadAllText(Path.Combine(_templateBaseDir, @"SiteConfig\NegativeBalanceByMatch\Templates\EmailTemplateSingleLanguage.cshtml"));
            return Razor.Parse(template, matches);
        }

        private bool IsDualLanguage(string licenseeName)
        {
            var licensees = AppSettings.LicenseesDualLanguage.ToLower().Split(',');
            bool isDual = licensees.Contains(licenseeName.ToLower());

            return isDual;
        }

        private void UpdateLog()
        {
            Upd_StatusIDCriteria updateLogCriteria = new Upd_StatusIDCriteria { LogIDs = _logIdUpdate };

            using (ObjectDb db = new ObjectDb(updateLogCriteria.SpName))
            {
                db.ExecuteNonQuery(updateLogCriteria.GetSpParams());
                var hashResult = db.GetOuputParams();
                updateLogCriteria.Result = new Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities.Result()
                {
                    ErrorCode = Converter.To<int>(hashResult["ErrCode"]),
                    ErrorMessage = Converter.ToString2(hashResult["ErrMsg"])
                };
            }
        }

        private List<Match> GetMatchList(Sel_BySiteCriteria criteria)
        {
            List<Match> matchList = new List<Match>();
            _logIdUpdate = string.Empty;

            try
            {
                using (var userInfor = new ObjectDb(criteria.SpName))
                {
                    DataSet dataSetUserinfo = userInfor.ExecuteDataSet(criteria.GetSpParams());
                    if (dataSetUserinfo != null
                         && dataSetUserinfo.Tables.Count > 0)
                    {
                        List<CustomerBalanceInfo> customerList = new List<CustomerBalanceInfo>();
                        int index = 1;
                        DataTable userTable = dataSetUserinfo.Tables[0];

                        foreach (DataRow row in userTable.Rows)
                        {
                            _logIdUpdate = _logIdUpdate + (index == 1 ? string.Empty : ",") + Converter.To<string>(row["LogID"]);
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

                                using (var matchInfo = new ObjectDb(matchLogCriteria.SpName))
                                {
                                    DataSet dataSetMatch = matchInfo.ExecuteDataSet(matchLogCriteria.GetSpParams());
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

        private void AssignMatchSettleLogInfo(Match match, int groupId, DataTable matchSettleTable)
        {
            foreach (DataRow row in matchSettleTable.Rows)
            {
                if (groupId == 3)
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
                    && c.TotalBalance == id.Key.TotalBalance).FirstOrDefault();

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