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
using Fanex.NAPReport.RngCasino.Business.Configuration;
using Fanex.NAPReport.RngCasino.Business.Criteria;
using Fanex.NAPReport.RngCasino.Business.Entities;
using Fanex.NAPReport.RngCasino.Business.Helper.Implement;
using Fanex.Service.Core.Utilities;
using RazorEngine;

namespace Fanex.NAPReport.RngCasino.Business.Business
{
	public class CustomerWinLossService : ServiceBase
	{
		private readonly IRngCasinoExcelService _excelService;
		private readonly ExportSettings _exportSettings;
		private readonly string _templateBaseDir;

		public CustomerWinLossService(IRngCasinoExcelService excelService, ExportSettings exportSettings, string templateBaseDir = "")
		{
			_excelService = excelService;
			_exportSettings = exportSettings;
			_templateBaseDir = templateBaseDir;
		}

		public bool SendMail()
		{
			if (RngCasinoSettings.IsActive == false)
				return false;

			var result = false;
			try
			{
				var dateTimeFormat = "{0:yyyy-MM-dd HH:mm}";
				var toDate = string.Format(dateTimeFormat, DateTime.Now);
				var fromDate = string.Format(dateTimeFormat, DateTime.Now.AddMinutes(-AppSettingBase.Interval / (60 * 1000) - RngCasinoSettings.ConsoleTestAddedDeltaMinute));

				var userInfos = GetCustomerWinLoss(Converter.ToDateTime(fromDate), Converter.ToDateTime(toDate), RngCasinoSettings.WinLoss);
				if (userInfos != null && userInfos.Count > 0)
				{
					var template = File.ReadAllText(Path.Combine(_templateBaseDir, @"Templates\EmailTemplate.cshtml"));
					var body = Razor.Parse(template, userInfos);

					var fileNamePath = Path.Combine(RngCasinoSettings.ExportDataDirectoryWithYearMonth,
														RngCasinoSettings.ExcelFileName + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx");
					_excelService.GenerateReport(userInfos, _exportSettings, fileNamePath);

					var subject = string.Format(RngCasinoSettings.Subject, RngCasinoSettings.BaseCurrency,
													NumberExtension.WinLossFormat(RngCasinoSettings.WinLoss, 0), fromDate, toDate);
					result = EmailHelper.SendMail(subject, RngCasinoSettings.Title, body, fileNamePath);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return result;
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
