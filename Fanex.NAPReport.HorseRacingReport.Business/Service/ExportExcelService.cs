// -----------------------------------------------------------------------
// <copyright file="HorseRacingReportService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
    using Fanex.NAPReport.HorseRacingReport.Business.Enums;
    using Fanex.NAPReport.HorseRacingReport.Business.Model;
    using Fanex.NAPReport.HorseRacingReport.Business.Resources;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Service;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExportExcelService : IExportExcelService
    {
        private const string RMCurrency = "RM2";
        private Dictionary<int, string> _currencies;

        private XSSFWorkbook _workbook;
        private BorderInfo _borderInfo;

        private FontAndColorInfo _fontInfo;
        private FontAndColorInfo _blueFontInfo;
        private FontAndColorInfo _headerFontInfo;

        private XSSFFont _font;
        private XSSFFont _blueFont;
        private XSSFFont _boldFont;
        private XSSFFont _headerFont;

        private Color _headerColor;
        private Color _subTotalBackgroundColor;
        private Color _subTotalByCurrencyBackgroundColor;
        private Color _subTotalByPoolCurrencyBackgroundColor;
        private Color _grandTotalBackgroundColor;

        private XSSFCellStyle _decimalNumberStyle;
        private XSSFCellStyle _intNumberStyle;
        private XSSFCellStyle _centerTextStyle;
        private XSSFCellStyle _leftTextStyle;

        private XSSFCellStyle _subTotalCenterTextStyle;
        private XSSFCellStyle _subTotalDecimalNumberStyle;

        private XSSFCellStyle _subTotalByCurrencyCenterTextStyle;
        private XSSFCellStyle _subTotalByCurrencyDecimalNumberStyle;

        private XSSFCellStyle _subTotalByPoolCurrencyCenterTextStyle;
        private XSSFCellStyle _subTotalByPoolCurrencyDecimalNumberStyle;

        private XSSFCellStyle _grandTotalCenterTextStyle;
        private XSSFCellStyle _grandTotalDecimalNumberStyle;

        private XSSFCellStyle _headerTextStyle;

        private CellInfo _cellInfo;
        private XSSFDataFormat _dataFormat;

        private BackgroundStyleInfo _headerBackgroundInfo;
        private BackgroundStyleInfo _subTotalBackgroundInfo;
        private BackgroundStyleInfo _subTotalByCurrencyBackgroundInfo;
        private BackgroundStyleInfo _subTotalByPoolCurrencyBackgroundInfo;
        private BackgroundStyleInfo _grandTotalBackgroundInfo;
        private BackgroundStyleInfo _normalRowBackgroundInfo;

        private AlignmentInfo _leftAlignmentInfo;
        private AlignmentInfo _centerAlignmentInfo;
        private AlignmentInfo _rightAlignmentInfo;

        private string _reportDataDirectory;

        public ExportExcelService()
            : this(null)
        {
        }

        public ExportExcelService(TimeRangeCriteria criteria)
        {
            _currencies = BaseService.ExchangeCurrencies;
            this.InitColor();
            this.InitAlignmentInfo();
            this.InitBorderInfo();
            this.InitFontInfo();
            this.InitBackgroundColorInfo();
            _reportDataDirectory = this.CreateHorseRacingReportDataDirectory(criteria);            
        }

        public string GetReportDirectory
        {
            get
            {
                return _reportDataDirectory;
            }
        }

        /// <summary>
        /// Gets the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main racing report data.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public RacingWinLoss GetMainReport(GetMainRacingReportCriteria criteria)
        {
            Dictionary<int, int> grandTotalByCurrency = new Dictionary<int, int>();
            string previousPartner = string.Empty;
            RacingWinLoss report = new RacingWinLoss
            {
                Total = CreateRacingWinLossWithPartnerName(Labels.Grand_Total)
            };
            RacingWinLoss racingWinLossTotalBase = CreateRacingWinLossWithCurrencyName(Labels.Base_RM);

            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                RacingWinLoss racingByGroup = null;
                RacingWinLoss winLossTotalPoolByPartner = null;
                RacingWinLoss winLossTotalBaseByPartner = null;
                var reportParams = criteria.GetSpParams();
                IDataReader reader = objectDb.ExecuteReader(reportParams);
                while (reader.Read())
                {
                    string partnerName = Converter.ToString2(reader["PartnerName"]);
                    int poolCurrency = Converter.ToInt(reader["PoolCurrency"]);
                    if (!CheckIsSamePartner(reader, previousPartner))
                    {
                        if (racingByGroup != null)
                        {
                            racingByGroup.Total.ReportRows.Add(winLossTotalPoolByPartner);
                            racingByGroup.Total.ReportRows.Add(winLossTotalBaseByPartner);
                            report.ReportRows.Add(racingByGroup);
                        }

                        racingByGroup = new RacingWinLoss
                        {
                            PartnerName = partnerName,
                            Total = CreateRacingWinLossWithPartnerName(string.Format("{0} Total", partnerName))
                        };
                        winLossTotalPoolByPartner = new RacingWinLoss
                        {
                            CurrencyName = _currencies[poolCurrency]
                        };
                        winLossTotalBaseByPartner = CreateRacingWinLossWithCurrencyName(Labels.Base_RM);
                        previousPartner = GetPartnerAndPoolCurrencyGroupName(reader);
                    }

                    racingByGroup.ReportRows.Add(MappingDataByCurrency(reader, "_Origin"));
                    RacingWinLoss racingWinLossByPoolCurrency = MappingDataByCurrency(reader, "_PoolCurr");
                    RacingWinLoss racingWinLossByBaseCurrency = MappingDataByCurrency(reader, string.Empty);
                    winLossTotalPoolByPartner.CalculateTotal(racingWinLossByPoolCurrency);
                    winLossTotalBaseByPartner.CalculateTotal(racingWinLossByBaseCurrency);
                    racingWinLossTotalBase.CalculateTotal(racingWinLossByBaseCurrency);
                    int grandTotalIndex;
                    if (grandTotalByCurrency.ContainsKey(poolCurrency))
                    {
                        grandTotalIndex = grandTotalByCurrency[poolCurrency];
                    }
                    else
                    {
                        grandTotalIndex = report.Total.ReportRows.Count;
                        grandTotalByCurrency.Add(poolCurrency, grandTotalIndex);
                        report.Total.ReportRows.Add(new RacingWinLoss
                        {
                            CurrencyName = _currencies[poolCurrency]
                        });
                    }

                    report.Total.ReportRows[grandTotalIndex].CalculateTotal(racingWinLossByPoolCurrency);
                }

                reader.Close();
                if (racingByGroup != null)
                {
                    racingByGroup.Total.ReportRows.Add(winLossTotalPoolByPartner);
                    racingByGroup.Total.ReportRows.Add(winLossTotalBaseByPartner);
                    report.ReportRows.Add(racingByGroup);
                }
            }

            report.Total.ReportRows.Add(racingWinLossTotalBase);
            return report;
        }

        /// <summary>
        /// Gets the detail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The detail racing report data.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public RacingWinLoss GetDetailReport(GetDetailRacingReportCriteria criteria)
        {
            Dictionary<int, int> totalDictionary = new Dictionary<int, int>();
            string previousCurrency = string.Empty;
            int previousMeeting = 0;
            int totalIndex = 0;
            string currencyNamebyPool = _currencies[criteria.PoolCurrency];
            RacingWinLoss report = new RacingWinLoss
            {
                Total = CreateRacingWinLossWithPartnerName(Labels.Grand_Total)
            };
            RacingWinLoss grandTotal = CreateRacingWinLossWithPartnerName(Labels.Grand_Total);

            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                RacingWinLoss grandTotalByPoolCurrency = new RacingWinLoss
                {
                    CurrencyName = currencyNamebyPool
                };
                RacingWinLoss grandTotalBaseCurrency = new RacingWinLoss
                {
                    CurrencyName = Labels.Base_RM
                };
                RacingWinLoss racingWinLossByCurrencyGroup = null;
                RacingWinLoss racingWinLossByMeetingGroup = null;
                RacingWinLoss racingWinLossBaseCurrencyTotal = null;
                RacingWinLoss racingWinLossByCurrencyTotal = null;
                IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    int meetingId = Converter.ToInt(reader["MeetingID"]);
                    int currencyId = Converter.ToInt(reader["currency"]);
                    string currencyName = Converter.ToString2(reader["currencyName"]);
                    string leagueName = CacheRef.GetLeagueName(meetingId);
                    if (!CheckIsSameCurrency(reader, previousCurrency))
                    {
                        if (racingWinLossByCurrencyGroup != null)
                        {
                            if (previousCurrency != RMCurrency)
                            {
                                racingWinLossByCurrencyGroup.Total.ReportRows.Add(racingWinLossByCurrencyTotal);
                            }

                            racingWinLossByCurrencyGroup.Total.ReportRows.Add(racingWinLossBaseCurrencyTotal);
                            racingWinLossByCurrencyGroup.ReportRows.Add(racingWinLossByMeetingGroup);
                            report.ReportRows.Add(racingWinLossByCurrencyGroup);
                        }

                        racingWinLossByCurrencyGroup = new RacingWinLoss
                        {
                            Total = CreateRacingWinLossWithPartnerName(string.Format("{0} Total", currencyName))
                        };
                        racingWinLossByMeetingGroup = new RacingWinLoss
                        {
                            PartnerName = leagueName,
                            Total = CreateRacingWinLossWithPartnerName(string.Format("{0} - {1} Total", leagueName, currencyName))
                        };
                        racingWinLossBaseCurrencyTotal = CreateRacingWinLossWithCurrencyName(Labels.Base_RM);
                        racingWinLossByCurrencyTotal = CreateRacingWinLossWithCurrencyName(currencyName);
                        previousCurrency = GetCurrencyGroupName(reader);
                    }

                    if (meetingId != previousMeeting)
                    {
                        if (previousMeeting != 0 && racingWinLossByMeetingGroup.ReportRows.Count != 0)
                        {
                            racingWinLossByCurrencyGroup.ReportRows.Add(racingWinLossByMeetingGroup);
                        }

                        racingWinLossByMeetingGroup = new RacingWinLoss()
                        {
                            PartnerName = leagueName,
                            Total = CreateRacingWinLossWithPartnerName(string.Format("{0} - {1} Total", leagueName, currencyName))
                        };
                        previousMeeting = meetingId;
                    }

                    RacingWinLoss racingWinLossInfo = MappingDataByCurrency(reader, "_Origin");
                    racingWinLossInfo.PartnerName = leagueName;
                    racingWinLossInfo.Race = Converter.ToInt(reader["RaceNo"]);
                    RacingWinLoss racingWinLossByCurrencyInfo = MappingDataByCurrency(reader, "_PoolCurr");
                    RacingWinLoss racingWinLossByBaseCurrencyInfo = MappingDataByCurrency(reader, string.Empty);
                    racingWinLossByMeetingGroup.ReportRows.Add(racingWinLossInfo);
                    racingWinLossByMeetingGroup.Total.CalculateTotal(racingWinLossInfo);
                    racingWinLossByCurrencyTotal.CalculateTotal(racingWinLossInfo);
                    racingWinLossBaseCurrencyTotal.CalculateTotal(racingWinLossByBaseCurrencyInfo);
                    grandTotalByPoolCurrency.CalculateTotal(racingWinLossByCurrencyInfo);
                    grandTotalBaseCurrency.CalculateTotal(racingWinLossByBaseCurrencyInfo);

                    if (totalDictionary.ContainsKey(meetingId))
                    {
                        totalIndex = totalDictionary[meetingId];
                    }
                    else
                    {
                        totalIndex = report.Total.ReportRows.Count;
                        totalDictionary.Add(meetingId, totalIndex);
                        report.Total.ReportRows.Add(new RacingWinLoss
                        {
                            PartnerName = leagueName,
                            CurrencyName = currencyNamebyPool
                        });
                    }

                    report.Total.ReportRows[totalIndex].CalculateTotal(racingWinLossByCurrencyInfo);
                }

                reader.Close();
                grandTotal.ReportRows.Add(grandTotalByPoolCurrency);
                grandTotal.ReportRows.Add(grandTotalBaseCurrency);
                if (racingWinLossByMeetingGroup != null)
                {
                    racingWinLossByCurrencyGroup.ReportRows.Add(racingWinLossByMeetingGroup);
                    racingWinLossByCurrencyGroup.Total.ReportRows.Add(racingWinLossByCurrencyTotal);
                    racingWinLossByCurrencyGroup.Total.ReportRows.Add(racingWinLossBaseCurrencyTotal);
                }

                if (racingWinLossByCurrencyGroup != null)
                {
                    report.ReportRows.Add(racingWinLossByCurrencyGroup);
                }
            }

            report.Total.ReportRows.Add(grandTotal);
            return report;
        }

        /// <summary>
        /// Exports the main report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="winlostDate">The win lost date.</param>
        /// <param name="criteria">The criteria.</param>
        public void ExportMainReport(RacingWinLoss report, int reportType, DateTime winlostDate, TimeRangeCriteria criteria)
        {
            string reportName = string.Empty;
            string sheetName = string.Empty;
            string reportTitle = string.Empty;
            _workbook = new XSSFWorkbook();
            GetExportMainReportInfo(reportType, winlostDate, out reportName, out sheetName, out reportTitle);
            XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            this.InitColumnWidth(worksheet);
            this.InitFont();
            this.InitCellStyleInfo();
            _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
            this.ExportReportTitle(worksheet, reportTitle);
            this.ExportReportHeader(worksheet, Labels.Pool_Currency, Labels.Currency);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            if (report.ReportRows.Count > 0)
            {
                this.ExportMainReportContent(worksheet, report);
                this.ExportMainReportGrandTotal(worksheet, report.Total);
            }
            else
            {
                this.ExportNoDataReport(worksheet);
            }

            worksheet.CreateFreezePane(2, 4, 2, 4);
            reportName = string.Format("{0}.{1}", reportName, "xlsx");
            SaveFile.SaveExcelFile(_reportDataDirectory, reportName, _workbook);

            GC.Collect();
        }

        /// <summary>
        /// Exports the detail report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="partnerId">The partner identifier.</param>
        /// <param name="winlostDate">The win lost date.</param>
        /// <param name="criteria">The criteria.</param>
        public void ExportDetailReport(RacingWinLoss report, int partnerId, DateTime winlostDate, TimeRangeCriteria criteria)
        {
            string reportName = string.Empty;
            string sheetName = string.Empty;
            string reportTitle = string.Empty;
            _workbook = new XSSFWorkbook();
            GetExportDetailReportInfo(partnerId, winlostDate, out reportName, out sheetName, out reportTitle);
            XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            this.InitColumnWidth(worksheet);
            this.InitFont();
            this.InitCellStyleInfo();
            _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
            this.ExportReportTitle(worksheet, reportTitle);
            this.ExportReportHeader(worksheet, Labels.Meeting_Currency, Labels.Race);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            if (report.ReportRows.Count > 0)
            {
                this.ExportDetailReportContent(worksheet, report);
                this.ExportDetailReportGrandTotal(worksheet, report.Total);
            }
            else
            {
                this.ExportNoDataReport(worksheet);
            }

            worksheet.CreateFreezePane(2, 4, 2, 4);

            reportName = string.Format("{0}.{1}", reportName, "xlsx");
            SaveFile.SaveExcelFile(_reportDataDirectory, reportName, _workbook);

            GC.Collect();
        }

        private void InitColumnWidth(XSSFSheet worksheet)
        {
            for (int i = 0; i < 25; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 300 * NPOIConstants.FontWidth);
        }

        private void ExportReportTitle(XSSFSheet worksheet, string reportTitle)
        {
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = reportTitle;
            _cellInfo.EndColumn = _cellInfo.EndColumn + 24;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            _cellInfo.CurrentRow = 2;
            _cellInfo.EndRow = 2;
        }

        private void ExportNoDataReport(XSSFSheet worksheet)
        {
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = "There is no data.";
            _cellInfo.EndColumn = _cellInfo.EndColumn + 24;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="firstColumnName">First name of the column.</param>
        /// <param name="secondColumnName">Name of the second column.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportReportHeader(XSSFSheet worksheet, string firstColumnName, string secondColumnName)
        {
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

            _cellInfo.CellValue = firstColumnName;
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = secondColumnName;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.EndRow = _cellInfo.CurrentRow;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 5;
            _cellInfo.CellValue = Labels.Agency;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);

            _cellInfo.CurrentColumn = _cellInfo.EndColumn + 1;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 8;
            _cellInfo.CellValue = Labels.IBC;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);

            _cellInfo.CurrentColumn = _cellInfo.EndColumn + 1;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 7;
            _cellInfo.CellValue = Labels.Partner;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CurrentColumn = 2;
            _cellInfo.EndColumn = 2;

            //// Agency header group
            _cellInfo.CellValue = Labels.Win_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Win_Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Total_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Comm_Income;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.PT_Win_Loss;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            //// IBC header group
            _cellInfo.CellValue = Labels.Win_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Win_Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Total_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Comm_Income;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Tote_Fee;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Comm_Expense;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.PT_Win_Loss;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Net_Income;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            //// Partner header group.
            _cellInfo.CellValue = Labels.Win_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Win_Place_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Total_TO;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Dividend_Won;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Comm_Expense;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Tote_Fee;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Amount_Due;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the content of the main report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        private void ExportMainReportContent(XSSFSheet worksheet, RacingWinLoss report)
        {
            XSSFRow row = null;
            int groupLength = 0;
            foreach (RacingWinLoss racingWinLossByPartner in report.ReportRows)
            {
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                groupLength = racingWinLossByPartner.ReportRows.Count;

                _cellInfo.CellValue = racingWinLossByPartner.PartnerName;
                _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                _cellInfo.EndRow = _cellInfo.CurrentRow;

                foreach (var racingWinLossByCurrency in racingWinLossByPartner.ReportRows)
                {
                    row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                    _cellInfo.CellValue = racingWinLossByCurrency.CurrencyName;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                    ExportRacingWinLossReportRow(racingWinLossByCurrency, worksheet, row, RowType.NormalRow);
                    _cellInfo.CurrentColumn = 1;
                    _cellInfo.EndColumn = 1;
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                }

                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                groupLength = racingWinLossByPartner.Total.ReportRows.Count;
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                _cellInfo.CellValue = racingWinLossByPartner.Total.PartnerName;
                _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _subTotalByCurrencyCenterTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                _cellInfo.EndRow = _cellInfo.CurrentRow;

                foreach (var racingWinLossByPartnerTotal in racingWinLossByPartner.Total.ReportRows)
                {
                    row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                    _cellInfo.CellValue = racingWinLossByPartnerTotal.CurrencyName;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _subTotalByCurrencyCenterTextStyle);
                    ExportRacingWinLossReportRow(racingWinLossByPartnerTotal, worksheet, row, RowType.SubTotalByCurrency);
                    _cellInfo.CurrentColumn = 1;
                    _cellInfo.EndColumn = 1;
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                }

                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }
        }

        /// <summary>
        /// Exports the main report grand total.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="grandTotal">The grand total.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportMainReportGrandTotal(XSSFSheet worksheet, RacingWinLoss grandTotal)
        {
            int groupLength = 0;
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            groupLength = grandTotal.ReportRows.Count;

            _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
            _cellInfo.CellValue = grandTotal.PartnerName;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _grandTotalCenterTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.EndRow = _cellInfo.CurrentRow;

            foreach (var grandTotalByCurrency in grandTotal.ReportRows)
            {
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                _cellInfo.CellValue = grandTotalByCurrency.CurrencyName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _grandTotalCenterTextStyle);
                ExportRacingWinLossReportRow(grandTotalByCurrency, worksheet, row, RowType.GrandTotal);
                _cellInfo.CurrentColumn = 1;
                _cellInfo.EndColumn = 1;
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }
        }

        /// <summary>
        /// Exports the content of the detail report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportDetailReportContent(XSSFSheet worksheet, RacingWinLoss report)
        {
            int groupLength = 0;
            XSSFRow row = null;

            foreach (RacingWinLoss racingWinLossByCurrency in report.ReportRows)
            {
                foreach (RacingWinLoss racingWinLossByLeague in racingWinLossByCurrency.ReportRows)
                {
                    groupLength = racingWinLossByLeague.ReportRows.Count;
                    row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                    _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
                    _cellInfo.CellValue = racingWinLossByLeague.PartnerName;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                    _cellInfo.EndRow = _cellInfo.CurrentRow;

                    foreach (RacingWinLoss racingWinLossInfo in racingWinLossByLeague.ReportRows)
                    {
                        row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                        _cellInfo.CellValue = racingWinLossInfo.Race;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _intNumberStyle);
                        ExportRacingWinLossReportRow(racingWinLossInfo, worksheet, row, RowType.NormalRow);
                        _cellInfo.CurrentColumn = 1;
                        _cellInfo.EndColumn = 1;
                        NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    }

                    NPOIUtility.ResetColumnIndex(ref _cellInfo);
                    _cellInfo.CellValue = racingWinLossByLeague.Total.PartnerName;
                    _cellInfo.EndColumn = _cellInfo.CurrentColumn + 1;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _subTotalCenterTextStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                    ExportRacingWinLossReportRow(racingWinLossByLeague.Total, worksheet, row, RowType.SubTotal);
                    NPOIUtility.ResetColumnIndex(ref _cellInfo);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                }

                groupLength = racingWinLossByCurrency.Total.ReportRows.Count;
                _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
                _cellInfo.CellValue = racingWinLossByCurrency.Total.PartnerName;
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _subTotalByCurrencyCenterTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                _cellInfo.EndRow = _cellInfo.CurrentRow;

                foreach (RacingWinLoss racingWinLossByCurrencyTotal in racingWinLossByCurrency.Total.ReportRows)
                {
                    row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                    _cellInfo.CellValue = racingWinLossByCurrencyTotal.CurrencyName;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _subTotalByCurrencyCenterTextStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                    ExportRacingWinLossReportRow(racingWinLossByCurrencyTotal, worksheet, row, RowType.SubTotalByCurrency);
                    _cellInfo.CurrentColumn = 1;
                    _cellInfo.EndColumn = 1;
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                }

                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }
        }

        /// <summary>
        /// Exports the detail report grand total.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="grandTotal">The grand total.</param>
        private void ExportDetailReportGrandTotal(XSSFSheet worksheet, RacingWinLoss grandTotal)
        {
            XSSFRow row = null;
            RacingWinLoss grandTotalInfo = null;
            int groupLength = 0;
            int grandTotalLength = grandTotal.ReportRows.Count;
            for (int i = 0; i < grandTotalLength - 1; i++)
            {
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                grandTotalInfo = grandTotal.ReportRows[i];
                _cellInfo.CellValue = grandTotalInfo.PartnerName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _subTotalByPoolCurrencyCenterTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                _cellInfo.CellValue = grandTotalInfo.CurrencyName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _subTotalByPoolCurrencyCenterTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                ExportRacingWinLossReportRow(grandTotalInfo, worksheet, row, RowType.SubTotalByPoolCurrency);
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }

            grandTotalInfo = grandTotal.ReportRows[grandTotalLength - 1];
            groupLength = grandTotalInfo.ReportRows.Count;
            _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength - 1;
            _cellInfo.CellValue = grandTotalInfo.PartnerName;
            row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _grandTotalCenterTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.EndRow = _cellInfo.CurrentRow;

            foreach (var grandTotalByCurrency in grandTotalInfo.ReportRows)
            {
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                _cellInfo.CellValue = grandTotalByCurrency.CurrencyName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _grandTotalCenterTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                ExportRacingWinLossReportRow(grandTotalByCurrency, worksheet, row, RowType.GrandTotal);
                _cellInfo.CurrentColumn = 1;
                _cellInfo.EndColumn = 1;
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the racing win loss report row.
        /// </summary>
        /// <param name="rowData">The row data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="row">The row of sheet.</param>
        /// <param name="rowType">Type of the row.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportRacingWinLossReportRow(
            RacingWinLoss rowData,
            XSSFSheet worksheet,
            XSSFRow row,
            RowType rowType)
        {
            XSSFCellStyle decimalNumberStyle = GetDecimalCellStyleByRowType(rowType);
            _cellInfo.CurrentColumn = 2;
            _cellInfo.EndColumn = 2;

            //// Agency group data.
            _cellInfo.CellValue = rowData.WinTOAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.PlaceTOAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.WinPlaceTOAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.TotalTOAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.CommIncomeAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.PTWinLossAgency;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            //// IBC group data.
            _cellInfo.CellValue = rowData.WinTurnoverIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.PlaceTurnoverIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.WinPlaceTurnoverIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.TotalTurnoverIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.CommIncomeIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.ToteFeeIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.CommExpenseIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.PTWinLossIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.NetIncomeIBC;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            //// Partner group data.
            _cellInfo.CellValue = rowData.WinTOPartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.PlaceTOPartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.WinPlaceTOPartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.TotalTOPartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.DividendWonPartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.CommExpensePartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.ToteFeePartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = rowData.AmountDuePartner;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Mappings the data by currency.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>Racing win loss data.</returns>
        private RacingWinLoss MappingDataByCurrency(IDataReader reader, string suffix)
        {
            return new RacingWinLoss
            {
                CurrencyName = Converter.ToString2(reader["currencyName"]),
                WinTOAgency = Converter.ToDecimal(reader["winTO_Agency" + suffix]),
                PlaceTOAgency = Converter.ToDecimal(reader["placeTO_Agency" + suffix]),
                WinPlaceTOAgency = Converter.ToDecimal(reader["winplaceTO_Agency" + suffix]),
                TotalTOAgency = Converter.ToDecimal(reader["totalTO_Agency" + suffix]),
                CommIncomeAgency = Converter.ToDecimal(reader["commIncome_Agency" + suffix]),
                PTWinLossAgency = Converter.ToDecimal(reader["PTWL_Agency" + suffix]),
                WinTurnoverIBC = Converter.ToDecimal(reader["winTO_IBC" + suffix]),
                PlaceTurnoverIBC = Converter.ToDecimal(reader["placeTO_IBC" + suffix]),
                WinPlaceTurnoverIBC = Converter.ToDecimal(reader["winplaceTO_IBC" + suffix]),
                TotalTurnoverIBC = Converter.ToDecimal(reader["totalTO_IBC" + suffix]),
                CommIncomeIBC = Converter.ToDecimal(reader["commIncome_IBC" + suffix]),
                ToteFeeIBC = Converter.ToDecimal(reader["FeeDue2" + suffix]),
                CommExpenseIBC = Converter.ToDecimal(reader["commIncome_Agency" + suffix]),
                PTWinLossIBC = Converter.ToDecimal(reader["PTWL_IBC" + suffix]),
                NetIncomeIBC = Converter.ToDecimal(reader["netIncome_IBC" + suffix]),
                WinTOPartner = Converter.ToDecimal(reader["winTO_Partner" + suffix]),
                PlaceTOPartner = Converter.ToDecimal(reader["placeTO_Partner" + suffix]),
                WinPlaceTOPartner = Converter.ToDecimal(reader["winplaceTO_Partner" + suffix]),
                TotalTOPartner = Converter.ToDecimal(reader["totalTO_Partner" + suffix]),
                DividendWonPartner = Converter.ToDecimal(reader["dividendWon" + suffix]),
                CommExpensePartner = Converter.ToDecimal(reader["commIncome_IBC" + suffix]),
                ToteFeePartner = Converter.ToDecimal(reader["FeeDue2" + suffix]),
                AmountDuePartner = Converter.ToDecimal(reader["AmountDue" + suffix])
            };
        }

        private bool CheckIsSamePartner(IDataReader reader, string previousPartner)
        {
            string currentPartner = GetPartnerAndPoolCurrencyGroupName(reader);
            return string.Equals(currentPartner, previousPartner);
        }

        private string GetPartnerAndPoolCurrencyGroupName(IDataReader reader)
        {
            string partnerId = Converter.ToString2(reader["partnerID"]);
            string partnerName = Converter.ToString2(reader["PartnerName"]);
            string poolCurrency = Converter.ToString2(reader["PoolCurrency"]);
            return string.Join(string.Empty, new string[] { partnerId, partnerName, poolCurrency });
        }

        private bool CheckIsSameCurrency(IDataReader reader, string previousCurrency)
        {
            string currentCurrency = GetCurrencyGroupName(reader);
            return string.Equals(currentCurrency, previousCurrency);
        }

        private string GetCurrencyGroupName(IDataReader reader)
        {
            string currencyName = Converter.ToString2(reader["currencyName"]);
            string currencyId = Converter.ToString2(reader["currency"]);
            return string.Join(string.Empty, new string[] { currencyName, currencyId });
        }

        private RacingWinLoss CreateRacingWinLossWithCurrencyName(string currencyName)
        {
            return new RacingWinLoss
            {
                CurrencyName = currencyName
            };
        }

        private RacingWinLoss CreateRacingWinLossWithPartnerName(string partnerName)
        {
            return new RacingWinLoss
            {
                PartnerName = partnerName
            };
        }

        /// <summary>
        /// Initializes the border information.
        /// </summary>
        private void InitBorderInfo()
        {
            _borderInfo = new BorderInfo(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                Color.Gray,
                Color.Gray,
                Color.Gray);
        }

        /// <summary>
        /// Initializes the color.
        /// </summary>
        private void InitColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _subTotalBackgroundColor = Color.FromArgb(255, 238, 221);
            _subTotalByCurrencyBackgroundColor = Color.FromArgb(211, 255, 200);
            _subTotalByPoolCurrencyBackgroundColor = Color.FromArgb(244, 244, 190);
            _grandTotalBackgroundColor = Color.FromArgb(219, 225, 187);
        }

        /// <summary>
        /// Initializes the font information.
        /// </summary>
        private void InitFontInfo()
        {
            _fontInfo = new FontAndColorInfo(
                    NPOIConstants.FontName,
                    NPOIConstants.FontSize,
                    Color.Black);
            _blueFontInfo = new FontAndColorInfo(
                    NPOIConstants.FontName,
                    NPOIConstants.FontSize,
                    Color.Blue);
            _headerFontInfo = new FontAndColorInfo(
                   NPOIConstants.FontName,
                   NPOIConstants.FontSize,
                   Color.White);
        }

        /// <summary>
        /// Initializes the alignment information.
        /// </summary>
        private void InitAlignmentInfo()
        {
            _leftAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Left,
                (int)VerticalAlignment.Center);
            _centerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);
            _rightAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Right,
                (int)VerticalAlignment.Center);
        }

        /// <summary>
        /// Initializes the background color information.
        /// </summary>
        private void InitBackgroundColorInfo()
        {
            _normalRowBackgroundInfo = new BackgroundStyleInfo(
                Color.White,
                (int)FillPattern.SolidForeground);

            _headerBackgroundInfo = new BackgroundStyleInfo(
                _headerColor,
                (int)FillPattern.SolidForeground);

            _subTotalBackgroundInfo = new BackgroundStyleInfo(
                _subTotalBackgroundColor,
                (int)FillPattern.SolidForeground);

            _subTotalByCurrencyBackgroundInfo = new BackgroundStyleInfo(
                _subTotalByCurrencyBackgroundColor,
                (int)FillPattern.SolidForeground);

            _subTotalByPoolCurrencyBackgroundInfo = new BackgroundStyleInfo(
                _subTotalByPoolCurrencyBackgroundColor,
                (int)FillPattern.SolidForeground);

            _grandTotalBackgroundInfo = new BackgroundStyleInfo(
                _grandTotalBackgroundColor,
                (int)FillPattern.SolidForeground);
        }

        /// <summary>
        /// Initializes the cell style information.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void InitCellStyleInfo()
        {
            _centerTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);
            _leftTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _leftAlignmentInfo,
                _font);

            _intNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _centerAlignmentInfo,
                _blueFont);
            _intNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _decimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _subTotalCenterTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _subTotalBackgroundInfo,
                _centerAlignmentInfo,
                _boldFont);
            _subTotalDecimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _subTotalBackgroundInfo,
                _rightAlignmentInfo,
                _boldFont);
            _subTotalDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _subTotalByCurrencyCenterTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _subTotalByCurrencyBackgroundInfo,
                _centerAlignmentInfo,
                _boldFont);
            _subTotalByCurrencyDecimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _subTotalByCurrencyBackgroundInfo,
                _rightAlignmentInfo,
                _boldFont);
            _subTotalByCurrencyDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _subTotalByPoolCurrencyCenterTextStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _subTotalByPoolCurrencyBackgroundInfo,
               _centerAlignmentInfo,
               _boldFont);
            _subTotalByPoolCurrencyDecimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _subTotalByPoolCurrencyBackgroundInfo,
                _rightAlignmentInfo,
                _boldFont);
            _subTotalByPoolCurrencyDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _grandTotalCenterTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _grandTotalBackgroundInfo,
                _centerAlignmentInfo,
                _boldFont);
            _grandTotalDecimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _grandTotalBackgroundInfo,
                _rightAlignmentInfo,
                _boldFont);
            _grandTotalDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _headerTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _headerBackgroundInfo,
                _centerAlignmentInfo,
                _headerFont);
        }

        /// <summary>
        /// Initializes the font.
        /// </summary>
        private void InitFont()
        {
            _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, false);
            _blueFont = NPOIHighVersion.CreateFont(_workbook, _blueFontInfo, false);
            _boldFont = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
            _headerFont = NPOIHighVersion.CreateFont(_workbook, _headerFontInfo, true);
        }

        /// <summary>
        /// Gets the type of the decimal cell style by row.
        /// </summary>
        /// <param name="rowType">Type of the row.</param>
        /// <returns>Decimal number style.</returns>
        private XSSFCellStyle GetDecimalCellStyleByRowType(RowType rowType)
        {
            XSSFCellStyle cellStyle = null;
            switch (rowType)
            {
                case RowType.NormalRow:
                    cellStyle = _decimalNumberStyle;
                    break;

                case RowType.SubTotal:
                    cellStyle = _subTotalDecimalNumberStyle;
                    break;

                case RowType.SubTotalByCurrency:
                    cellStyle = _subTotalByCurrencyDecimalNumberStyle;
                    break;

                case RowType.SubTotalByPoolCurrency:
                    cellStyle = _subTotalByPoolCurrencyDecimalNumberStyle;
                    break;

                case RowType.GrandTotal:
                    cellStyle = _grandTotalDecimalNumberStyle;
                    break;

                default:
                    cellStyle = _decimalNumberStyle;
                    break;
            }

            return cellStyle;
        }

        /// <summary>
        /// Creates the horse racing report data directory.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Report data directory.</returns>
        private string CreateHorseRacingReportDataDirectory(TimeRangeCriteria criteria)
        {
            string reportFrom = criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            string reportTo = criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            string reportByPeriodTime = string.Format(RacingResource.Racing_Report_Export_Directory, reportFrom, reportTo);
            string reportDataDirectory = AppSettings.DataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);
            string directoryReportPath = Path.Combine(reportDataDirectory, reportByPeriodTime);
            DirectoryExtensions.CheckAndCreateDirectory(directoryReportPath);

            return directoryReportPath;
        }

        /// <summary>
        /// Gets the export main report information.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="winlostDate">The win lost date.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="reportTitle">The report title.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void GetExportMainReportInfo(
            int reportType,
            DateTime winlostDate,
            out string reportName,
            out string sheetName,
            out string reportTitle)
        {
            switch (reportType)
            {
                case 1:
                    reportName = string.Format(
                        RacingResource.Horse_Racing_UK_Tote_ReportName,
                        winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"));
                    sheetName = RacingResource.Horse_Racing_UK_Tote_SheetName;
                    reportTitle = string.Format(RacingResource.Horse_Racing_UK_Tote_ReportTitle, winlostDate.ToShortFormatedDate());
                    break;

                case 6:
                    reportName = string.Format(
                       RacingResource.Horse_Racing_SA_Tote_ReportName,
                       winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"));
                    sheetName = RacingResource.Horse_Racing_SA_Tote_SheetName;
                    reportTitle = string.Format(RacingResource.Horse_Racing_SA_Tote_ReportTitle, winlostDate.ToShortFormatedDate());
                    break;

                default:
                    reportName = string.Empty;
                    sheetName = string.Empty;
                    reportTitle = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Gets the export detail report information.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <param name="winlostDate">The win lost date.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="reportTitle">The report title.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void GetExportDetailReportInfo(
            int partnerId,
            DateTime winlostDate,
            out string reportName,
            out string sheetName,
            out string reportTitle)
        {
            switch (partnerId)
            {
                case 5:
                    reportName = string.Format(
                        RacingResource.HorseRacingForAustTote_ReportName,
                        winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                        "TABCORP");
                    sheetName = RacingResource.HorseRacingForAustTote_SheetName;
                    reportTitle = string.Format(
                        RacingResource.HorseRacingForAustTote_ReportTitle,
                        "TABCORP",
                        winlostDate.ToShortFormatedDate());
                    break;

                case 6:
                    reportName = string.Format(
                        RacingResource.HorseRacingForAustTote_ReportName,
                        winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                        "TABCORPNSW");
                    sheetName = RacingResource.HorseRacingForAustTote_SheetName;
                    reportTitle = string.Format(
                        RacingResource.HorseRacingForAustTote_ReportTitle,
                        "TABCORPNSW",
                        winlostDate.ToShortFormatedDate());
                    break;

                case 103:
                    reportName = string.Format(
                        RacingResource.GreyhoundForAustTote_ReportName,
                        winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                        "TABCORP");
                    sheetName = RacingResource.GreyhoundForAustTote_SheetName;
                    reportTitle = string.Format(
                       RacingResource.GreyhoundForAustTote_ReportTitle,
                       "TABCORP",
                       winlostDate.ToShortFormatedDate());
                    break;

                case 104:
                    reportName = string.Format(
                       RacingResource.GreyhoundForAustTote_ReportName,
                       winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                       "TABCORPNSW");
                    sheetName = RacingResource.GreyhoundForAustTote_SheetName;
                    reportTitle = string.Format(
                      RacingResource.GreyhoundForAustTote_ReportTitle,
                      "TABCORPNSW",
                      winlostDate.ToShortFormatedDate());
                    break;

                case 201:
                    reportName = string.Format(
                        RacingResource.HarnessForAustTote_ReportName,
                        winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                        "TABCORP");
                    sheetName = RacingResource.HarnessForAustTote_SheetName;
                    reportTitle = string.Format(
                      RacingResource.HarnessForAustTote_ReportTitle,
                      "TABCORP",
                      winlostDate.ToShortFormatedDate());
                    break;

                case 202:
                    reportName = string.Format(
                       RacingResource.HarnessForAustTote_ReportName,
                       winlostDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                       "TABCORPNSW");
                    sheetName = RacingResource.HarnessForAustTote_SheetName;
                    reportTitle = string.Format(
                      RacingResource.HarnessForAustTote_ReportTitle,
                      "TABCORPNSW",
                      winlostDate.ToShortFormatedDate());
                    break;

                default:
                    reportName = string.Empty;
                    sheetName = string.Empty;
                    reportTitle = string.Empty;
                    break;
            }
        }
    }
}