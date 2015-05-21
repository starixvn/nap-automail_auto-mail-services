using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Fanex.Common;
using Fanex.Data;
using Fanex.NAPReport.Business.Criteria;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Criteria;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Implement;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Repositories;
using Fanex.Service.Core.Enums;
using Fanex.Service.Core.Extensions;
using Newtonsoft.Json;
using RazorEngine;
using RemotingRef;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business
{
	public class NegativeBalanceByMatchService : ServiceBase
	{
		private readonly INegativeBalanceByMatchRepository _repository;
		private string _logIdUpdate = string.Empty;
		private readonly List<object> _logData = new List<object>();
		private readonly ILicenseeService _licenseeService;
		private readonly MailService _mailService;

		public NegativeBalanceByMatchService(ILicenseeService licenseeService, MailService mailService)
		{
			_licenseeService = licenseeService;
			_mailService = mailService;
			_repository = new NegativeBalanceByMatchRepository();
		}

		public void ProcessBusiness()
		{
			if (Settings.IsActive == false)
			{
				return;
			}

			DateTime curDateTime = DateTime.UtcNow;
			ExportSettings exportSettings = new ExportSettings();

			foreach (LicenseeFunction licenseeFunction in _licenseeService.GetLicenseeFunctions())
			{
				string licenseeName = licenseeFunction.LicenseeName;

				int timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
				string subject = licenseeName.ToUpper() + " - " +
								string.Format(Settings.Subject, DateTime.Now, timezone > 0 ? ("+" + timezone) : timezone.ToString());

				int taskId = 0;
				try
				{
					List<Match> matches = GetMatchList(new Sel_BySiteCriteria { Site = licenseeName });

					if (matches != null && matches.Count > 0)
					{
						taskId = _repository.InsertTask(new NegativeBalanceTaskInfo { TaskStatus = Status.Running });

						_logData.Add(new Dictionary<object, object> { { "DataSendEmail", matches } });

						string body = GetMailBody(licenseeName, matches);
						var result = SendEmail(licenseeFunction, subject, Settings.Title, body, Settings.AttachReport,
												matches, curDateTime, exportSettings, taskId);

						if (result)
						{
							UpdateLog(taskId);
						}
					}
				}
				catch (Exception ex)
				{
					string log = JsonConvert.SerializeObject(_logData);
					_repository.UpdateTaskStatus(taskId, Status.Failed, log);
					LogExtensions.ExceptionHandler(ex);
				}
			}
		}

		private string GetMailBody(string licenseeName, IList<Match> matches)
		{
			string appLocation = AppDomain.CurrentDomain.BaseDirectory;
			string template = licenseeName.ToLower().Equals("haifa")
									? File.ReadAllText(appLocation + @"/SiteConfig/NegativeBalanceByMatch/Templates/EmailTemplateDualLanguage.cshtml")
									: File.ReadAllText(appLocation + @"/SiteConfig/NegativeBalanceByMatch/Templates/EmailTemplateSingleLanguage.cshtml");
									//? File.ReadAllText(appLocation + @"/Templates/EmailTemplateDualLanguage.cshtml")
									//: File.ReadAllText(appLocation + @"/Templates/EmailTemplateSingleLanguage.cshtml");
			return Razor.Parse(template, matches);
		}

		private void UpdateLog(int taskId)
		{
			Upd_StatusIDCriteria updateLogCriteria = new Upd_StatusIDCriteria { LogIDs = _logIdUpdate };

			using (ObjectDb db = new ObjectDb(updateLogCriteria.SpName))
			{
				db.ExecuteNonQuery(updateLogCriteria.GetSpParams());
				var hashResult = db.GetOuputParams();
				updateLogCriteria.Result = new Result()
				{
					ErrorCode = Converter.To<int>(hashResult["ErrCode"]),
					ErrorMessage = Converter.ToString2(hashResult["ErrMsg"])
				};

				_logData.Add(updateLogCriteria);
			}

			string log = JsonConvert.SerializeObject(_logData);

			_repository.UpdateTaskStatus(taskId, Status.Successful, log);
		}

		private bool SendEmail(LicenseeFunction licenseeFunction, string subject, string title, string body, bool reportAttached,
			IList<Match> matches, DateTime dateTime, ExportSettings exportSettings, int taskId)
		{
			bool result = true;
			try
			{
				_mailService.SendMail(licenseeFunction, subject, title, body, reportAttached, matches, dateTime, exportSettings);
			}
			catch (Exception ex)
			{
				_repository.UpdateTaskStatus(taskId, Status.Failed);
				LogExtensions.ExceptionHandler(ex);
				result = false;
			}
			return result;
		}

		private List<Match> GetMatchList(Sel_BySiteCriteria criteria)
		{
			List<Match> matchList = new List<Match>();
			_logIdUpdate = string.Empty;

			try
			{
				using (var dbUserInfor = new ObjectDb(criteria.SpName))
				{
					DataSet dataSetUserinfo = dbUserInfor.ExecuteDataSet(criteria.GetSpParams());
					if (dataSetUserinfo != null
						 && dataSetUserinfo.Tables.Count > 0)
					{
						List<CustomerBalanceInfo> customerList = new List<CustomerBalanceInfo>();
						int index = 1;
						DataTable userTable = dataSetUserinfo.Tables[0];

						criteria.Result = userTable;
						_logData.Add(criteria);

						foreach (DataRow row in userTable.Rows)
						{
							_logIdUpdate = _logIdUpdate + (index == 1 ? "" : ",") + Converter.To<string>(row["LogID"]);
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