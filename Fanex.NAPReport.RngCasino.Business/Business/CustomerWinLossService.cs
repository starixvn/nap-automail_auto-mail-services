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
using Fanex.NAPReport.RngCasino.Business.Entities;
using Fanex.NAPReport.RngCasino.Business.Helper.Implement;
using RazorEngine;
using Robot.Report.RngCasino.Criteria;
using Robot.Report.RngCasino.Helper.Implement;

namespace Fanex.NAPReport.RngCasino.Business.Business
{
	public class CustomerWinLossService : ServiceBase
	{
		private readonly IRngCasinoExcelService _excelService;
		private readonly ExportSettings _exportSettings;

		public CustomerWinLossService(IRngCasinoExcelService excelService, ExportSettings exportSettings)
		{
			_excelService = excelService;
			_exportSettings = exportSettings;
		}

		public bool SendMail(DateTime fromDate, DateTime toDate, decimal winlost, string subject, string title, string mailTo, string cc, string bcc)
		{
			var result = false;
			try
			{
				var userInfos = GetCustomerWinLoss(fromDate, toDate, winlost);
				if (userInfos != null && userInfos.Count > 0)
				{
					var appLocation = AppDomain.CurrentDomain.BaseDirectory;
					var template = File.ReadAllText(appLocation + @"/Templates/EmailTemplate.cshtml");
					var body = Razor.Parse(template, userInfos);


					var fileNamePath = Path.Combine(RngCasinoSettings.ExportDataDirectoryWithYearMonth,
														RngCasinoSettings.ExcelFileName + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx");
					_excelService.GenerateReport(userInfos, _exportSettings, fileNamePath);

					//result = EmailHelper.SendMail(mailTo, cc, bcc, title, subject, content, null);

					result = EmailHelper.SendMail(subject, title, body, fileNamePath);
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
