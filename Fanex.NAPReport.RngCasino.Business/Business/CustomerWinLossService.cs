// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Fanex.Common;
using Fanex.NAPReport.RngCasino.Business.Business.Exports;
using Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.RngCasino.Business.Criteria;
using Fanex.NAPReport.RngCasino.Business.Entities;
using Fanex.NAPReport.RngCasino.Business.Helper.Implement;
using Fanex.Service.Core.MailService;
using RazorEngine;
using Fanex.Service.Core.Constants;
using Fanex.NAPReport.RngCasino.Business.Repositories;
using Fanex.Service.Core.Model;
using Fanex.MailDelivery.Client.Handlers;

namespace Fanex.NAPReport.RngCasino.Business.Business
{
	public class CustomerWinLossService : ServiceBase, IStandaloneTimerFunctionMailService
	{
		private readonly IRngCasinoExcelService _excelService;
		private readonly RngCasinoExportSettings _rngCasinoExportSettings;
		private readonly string _templateBaseDir;
		private string _subject;
        private IRepository _repository;
        private IMailStationService _mailStation;
        private MailInfo _mailInfo;


		public CustomerWinLossService(IRngCasinoExcelService excelService, RngCasinoExportSettings rngCasinoExportSettings, string templateBaseDir = "")
		{
			_excelService = excelService;
			_rngCasinoExportSettings = rngCasinoExportSettings;
			_templateBaseDir = templateBaseDir;
            _repository = new Repository();
            _mailStation = new MailStationService();
            _mailInfo = new MailInfo();
		}

		public override bool IsActive()
		{
            return AppSettings.IsActive;
		}

		public override bool NotifyMailVipGroup()
		{
			bool result = false;
			try
			{
                
				var dateTimeFormat = "{0:yyyy-MM-dd HH:mm}";
                var toDate = DateTime.Now;
                var fromDate = _repository.GetLastTime();

                var userInfos = GetCustomerWinLoss(Converter.ToDateTime(fromDate), Converter.ToDateTime(toDate), AppSettings.WinLoss);
				if (userInfos != null && userInfos.Count > 0)
				{
					var body = GetMailVipGroupBody(userInfos);

                    var fileNamePath = Path.Combine(AppSettings.ExportDataDirectoryWithYearMonthDay,
                                                        "RNG_Casino_WinLoss" + "_" + DateTime.Now.ToString(DateTimeFormatConstants.YEAR_MONTH_DATE_MINUTE) + ".xlsx");                   

					_excelService.GenerateReport(userInfos, _rngCasinoExportSettings, fileNamePath);

                    var fileNamesPath = new List<string>();
                    fileNamesPath.Add(fileNamePath);
                    _mailInfo.FileName = Path.GetFileName(fileNamePath);
                    _mailInfo.FileNamesPath = fileNamesPath;

                    var mail = _mailStation.CreateMailCustomContentWithAttachedFile(_mailInfo, body);
                    mail.MailingListId = AppSettings.FunctionIdVipGroup;
                    MailClientHandler.SendMail(mail);
                    _repository.UpdateLastTime();

                    result = true;
				}           
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return result;
		}

		public override void NotifyMailNormalGroup()
		{
            if (AppSettings.FunctionIdNormalGroup > 0)
			{
                _mailStation.SendFunctionMailWithNotification(AppSettings.FunctionIdNormalGroup, _mailInfo);
			}
		}

		private string GetMailVipGroupBody(IList<UserInfo> userInfos)
		{
            var template = File.ReadAllText(Path.Combine(_templateBaseDir, @"SiteConfig\RNGCasinoWinLoss\Templates\RNGCasinoWinLossEmailTemplate.cshtml"));
			return Razor.Parse(template, userInfos);
		}

		private List<UserInfo> GetCustomerWinLoss(DateTime fromDate, DateTime toDate, decimal winlost)
		{
			var customerList = new List<UserInfo>();
			var customerCriteria = new CustomerWinLossCriteria();
			try
			{
				var currencyList = GetCurrencyList();
				customerCriteria.FromDate = fromDate;
				customerCriteria.ToDate = toDate;
				customerCriteria.WinLoss = winlost;

				var rowData = GetReportData<dynamic>(customerCriteria);
				if (rowData != null)
				{
					foreach (var row in rowData)
					{
						var customerInfo = new UserInfo()
						{
							Member = Converter.ToString2(row.member),
							Agency = Converter.ToString2(row.agent),
							Currency = Converter.ToInt(row.currency),
							CurrencyName = currencyList[Converter.ToInt(row.currency)],
							WinlossBaseCurrency = Converter.ToDecimal(row.winlost_rm),
							Winloss = Converter.ToDecimal(row.winlost)
						};
						customerList.Add(customerInfo);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return customerList;
		}

		private Dictionary<int, string> GetCurrencyList()
		{
			var currencyCriteria = new GetCurrencyListCriteria();
			var currencyList = new Dictionary<int, string>();
			try
			{
				var rowData = GetReportData<dynamic>(currencyCriteria);
				if (rowData != null)
				{
					foreach (var row in rowData)
					{
						currencyList.Add(Converter.ToInt(row.exchangeid), Converter.ToString2(row.currency));
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return currencyList;
		}
	}
}
