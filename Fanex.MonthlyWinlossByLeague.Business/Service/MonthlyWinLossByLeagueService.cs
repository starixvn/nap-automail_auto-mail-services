// -----------------------------------------------------------------------
// <copyright file="MonthlyWinLossByLeagueService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Fanex.MonthlyWinlossByLeague.Business.Model;
    using Fanex.MonthlyWinlossByLeague.Business.Repositories;
    using Fanex.MonthlyWinlossByLeague.Business.Resources;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using OfficeOpenXmlCrypto;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLossByLeagueService : IMonthlyWinLossByLeagueService
    {
        private const int _NormalRow = 0;
        private const int _SubTotalRow = 1;
        private const int _TotalRow = 2;

        private IMonthlyWinLossByLeagueRepository _repository;
        private Dictionary<int, string> _bettypes;
        private DateTime _lastMonth = DateTime.Now.AddMonths(-1);

        private XSSFWorkbook _workbook;
        private BorderInfo _borderInfo;

        private FontAndColorInfo _fontInfo;
        private XSSFFont _font;
        private XSSFFont _boldFont;

        private Color _headerColor;
        private Color _totalRowByGroupBackgroundColor;
        private Color _totalRowBackgroundColor;

        private XSSFCellStyle _decimalNumberStyle;
        private XSSFCellStyle _leftTextStyle;
        private XSSFCellStyle _leftTotalByGroupTextStyle;
        private XSSFCellStyle _leftTotalTextStyle;
        private XSSFCellStyle _intNumberStyle;
        private XSSFCellStyle _decimalTotalNumberStyle;
        private XSSFCellStyle _intTotalNumberStyle;
        private XSSFCellStyle _decimalTotalByGroupNumberStyle;
        private XSSFCellStyle _intTotalByGroupNumberStyle;
        private XSSFCellStyle _headerTextStyle;

        private CellInfo _cellInfo;
        private XSSFDataFormat _dataFormat;

        private BackgroundStyleInfo _headerBackgroundInfo;
        private BackgroundStyleInfo _totalRowByGroupBackgroundInfo;
        private BackgroundStyleInfo _totalRowBackgroundInfo;
        private BackgroundStyleInfo _normalRowBackgroundInfo;

        private AlignmentInfo _leftAlignmentInfo;
        private AlignmentInfo _centerAlignmentInfo;
        private AlignmentInfo _rightAlignmentInfo;

        public MonthlyWinLossByLeagueService()
            : this(null)
        {
        }

        public MonthlyWinLossByLeagueService(IMonthlyWinLossByLeagueRepository repository)
        {
            _repository = repository ?? new MonthlyWinLossByLeagueRepository();
        }

        public Dictionary<int, string> Bettypes
        {
            get
            {
                return _bettypes;
            }
        }

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <returns>Monthly win loss by league.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public List<MonthlyWinLoss> GetReport()
        {
            List<MonthlyWinLoss> report = new List<MonthlyWinLoss>();
            var reportData = _repository.GetMonthlyWinLossByLeague();
            InitBettypes();
            var reportByPeriodAndStatusGroups = reportData.GroupBy(x => new { x.Period, x.LiveStatus });

            foreach (var reportByPeriodAndStatusGroup in reportByPeriodAndStatusGroups)
            {
                var periodAndStatusGroup = new MonthlyWinLoss
                {
                    Period = reportByPeriodAndStatusGroup.Key.Period,
                    LiveStatus = reportByPeriodAndStatusGroup.Key.LiveStatus,
                    Total = InitListBettypesWinLoss()
                };

                var winlossByGroups = reportByPeriodAndStatusGroup.GroupBy(x => x.GroupName);

                foreach (var winlossByGroup in winlossByGroups)
                {
                    var groupLeague = new GroupLeagueWinLoss
                    {
                        GroupName = winlossByGroup.Key,
                        Total = InitListBettypesWinLoss()
                    };

                    var winLossByLeagueGroups = winlossByGroup.GroupBy(x => x.League);
                    foreach (var winLossByLeagueGroup in winLossByLeagueGroups)
                    {
                        var leagueWinLoss = new LeagueWinLoss
                            {
                                LeagueName = winLossByLeagueGroup.Key,
                                BettypeWinLoss = InitListBettypesWinLoss()
                            };

                        foreach (var item in winLossByLeagueGroup)
                        {
                            string bettypeName = item.BetTypeWinLoss.Bettype;
                            int bettypeIndex = GetBettypeIndex(bettypeName);
                            int grandTotalIndex = GetBettypeIndex(Labels.GrandTotal);
                            var bettypeInfo = new BettypeWinLoss
                            {
                                Bettype = bettypeName,
                                WinLoss = item.BetTypeWinLoss.WinLoss,
                                Turnover = item.BetTypeWinLoss.Turnover,
                                BetCount = item.BetTypeWinLoss.BetCount
                            };

                            leagueWinLoss.BettypeWinLoss[bettypeIndex] = bettypeInfo;
                            leagueWinLoss.BettypeWinLoss[grandTotalIndex].CalculateTotal(bettypeInfo);
                            leagueWinLoss.MatchCount = item.BetTypeWinLoss.MatchCount;
                            groupLeague.Total[bettypeIndex].CalculateTotal(bettypeInfo);
                            groupLeague.Total[grandTotalIndex].CalculateTotal(bettypeInfo);
                            periodAndStatusGroup.Total[bettypeIndex].CalculateTotal(bettypeInfo);
                            periodAndStatusGroup.Total[grandTotalIndex].CalculateTotal(bettypeInfo);
                        }

                        groupLeague.LeagueWinLoss.Add(leagueWinLoss);
                    }

                    periodAndStatusGroup.GroupLeagueWinLoss.Add(groupLeague);
                }

                report.Add(periodAndStatusGroup);
            }

            return report;
        }

        public int InsertTask(Task task)
        {
            int id = _repository.InsertTask(task);
            return id;
        }

        public Task GetTaskReadyToRun()
        {
            Task task = _repository.GetTaskReadyRun();

            return task;
        }

        /// <summary>
        /// Processes the monthly win loss by league report.
        /// </summary>
        public void ProcessMonthlyWinLossByLeagueReport()
        {
            _lastMonth = DateTime.Now.AddMonths(-1);
            var report = GetReport();

            if (report.Count > 0)
            {
                this.ExportReport(report);
            }
        }

        /// <summary>
        /// Initializes the bet types.
        /// </summary>
        private void InitBettypes()
        {
            int index = 0;
            var bettypeList = _repository.Bettypes;

            _bettypes = new Dictionary<int, string>();
            _bettypes.Add(index++, Labels.Handicap);
            _bettypes.Add(index++, Labels._1stHandicap);
            _bettypes.Add(index++, Labels._1stOverUnder);
            _bettypes.Add(index++, Labels.OverUnder);

            foreach (string bettype in bettypeList)
            {
                if (!_bettypes.ContainsValue(bettype))
                {
                    _bettypes.Add(index++, bettype);
                }
            }

            _bettypes.Add(index++, Labels.GrandTotal);
        }

        /// <summary>
        /// Initializes the list bet types win loss.
        /// </summary>
        /// <returns>List bet types.</returns>
        private List<BettypeWinLoss> InitListBettypesWinLoss()
        {
            List<BettypeWinLoss> bettypeWinLossList = new List<BettypeWinLoss>();
            foreach (var bettype in _bettypes)
            {
                bettypeWinLossList.Add(new BettypeWinLoss
                {
                    Bettype = bettype.Value,
                    BetCount = 0,
                    Turnover = 0,
                    WinLoss = 0
                });
            }

            return bettypeWinLossList;
        }

        private int GetBettypeIndex(string bettypeName)
        {
            return _bettypes.FirstOrDefault(x => x.Value == bettypeName).Key;
        }

        /// <summary>
        /// Creates the monthly win loss by league report data directory.
        /// </summary>
        /// <returns>Report data directory.</returns>
        private string CreateMonthlyWinLossByLeagueReportDataDirectory()
        {
            string reportDataDirectory = AppSettings.ExportDataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);
            string directoryReportPath = Path.Combine(reportDataDirectory, _lastMonth.Year.ToString());
            DirectoryExtensions.CheckAndCreateDirectory(directoryReportPath);

            return directoryReportPath;
        }

        /// <summary>
        /// Exports the report.
        /// </summary>
        /// <param name="report">The report.</param>
        private void ExportReport(List<MonthlyWinLoss> report)
        {
            FileStream fileStream = null;
            this.InitColor();
            this.InitAlignmentInfo();
            this.InitBorderInfo();
            this.InitFontInfo();
            this.InitBackgroundColorInfo();
            _workbook = new XSSFWorkbook();
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            this.InitFont();
            this.InitCellStyleInfo();

            foreach (var reportByPeriodTimeAndStatus in report)
            {
                _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
                string sheetName = string.Format("Period {0} - {1}", reportByPeriodTimeAndStatus.Period, reportByPeriodTimeAndStatus.LiveStatus);
                XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
                this.InitColumnWidth(worksheet);
                this.ExportReportHeader(worksheet);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                this.ExportReportContent(worksheet, reportByPeriodTimeAndStatus);
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                this.ExportTotal(reportByPeriodTimeAndStatus.Total, worksheet, _TotalRow, string.Empty);
                worksheet.CreateFreezePane(0, 1, 0, 1);
            }

            var reportDirectory = this.CreateMonthlyWinLossByLeagueReportDataDirectory();
            string reportName = Resources.MonthlyWinlossByLeague_ReportName;

            var reportPath = Path.Combine(reportDirectory, reportName);
            var fullReportPath = string.Format("{0}.{1}", reportPath, "xlsx");

            fileStream = new FileStream(
                    fullReportPath,
                    FileMode.Create,
                    FileAccess.Write);
            _workbook.Write(fileStream);

            OfficeCryptoStream stream = OfficeCryptoStream.Open(fullReportPath);
            stream.Password = AppSettings.ExcelPassword;
            stream.SaveAs(fullReportPath);
            stream.Close();

            fileStream.Close();
            GC.Collect();
        }

        /// <summary>
        /// Initializes the width of the column.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void InitColumnWidth(XSSFSheet worksheet)
        {
            int numberColumn = this.Bettypes.Count + 4;
            for (int i = 0; i < numberColumn; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 300 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(1, 350 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(3, 220 * NPOIConstants.FontWidth);
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void ExportReportHeader(XSSFSheet worksheet)
        {
            XSSFRow row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = Labels.Group;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.League;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.NoMatch;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = Labels.Data;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            foreach (var bettype in this.Bettypes)
            {
                _cellInfo.CellValue = bettype.Value;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            }

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the content of the report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportReportContent(XSSFSheet worksheet, MonthlyWinLoss report)
        {
            XSSFRow row = null;
            foreach (var groupLeagueWinLoss in report.GroupLeagueWinLoss)
            {
                int groupIndex = 0;
                row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                int groupLength = groupLeagueWinLoss.LeagueWinLoss.Count;
                _cellInfo.CellValue = groupLeagueWinLoss.GroupName;
                _cellInfo.EndRow = _cellInfo.CurrentRow + (groupLength * 4) - 1;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                foreach (var leagueWinLoss in groupLeagueWinLoss.LeagueWinLoss)
                {
                    _cellInfo.EndRow = groupIndex == 0 ? _cellInfo.EndRow - (groupLength * 4) - 1 : _cellInfo.EndRow;
                    groupIndex++;
                    _cellInfo.EndRow = _cellInfo.CurrentRow + 3;
                    _cellInfo.CellValue = leagueWinLoss.LeagueName;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                    _cellInfo.CellValue = leagueWinLoss.MatchCount;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _intNumberStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                    _cellInfo.EndRow = _cellInfo.EndRow - 3;
                    _cellInfo.CellValue = Labels.Turnover;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = Labels.Winloss;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = Labels.MarginPercent;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = Labels.BetCount;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                    ExportBettypeInfo(leagueWinLoss.BettypeWinLoss, worksheet, _NormalRow);

                    _cellInfo.CurrentColumn = 1;
                    _cellInfo.EndColumn = 1;
                }

                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                ExportTotal(groupLeagueWinLoss.Total, worksheet, _SubTotalRow, groupLeagueWinLoss.GroupName);
            }
        }

        /// <summary>
        /// Exports the bet type information.
        /// </summary>
        /// <param name="bettypeWinLoss">The bet type win loss.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="rowType">Type of the row.</param>
        private void ExportBettypeInfo(List<BettypeWinLoss> bettypeWinLoss, XSSFSheet worksheet, int rowType)
        {
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            XSSFCellStyle decimalNumberStyle = GetDecimalNumberStyleByRowType(rowType);
            XSSFCellStyle intNumberStyle = GetIntNumberStyleByRowType(rowType);
            foreach (var bettype in bettypeWinLoss)
            {
                _cellInfo.CurrentRow = _cellInfo.CurrentRow - 4;
                _cellInfo.EndRow = _cellInfo.EndRow - 4;
                _cellInfo.CellValue = bettype.Turnover;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                _cellInfo.CellValue = bettype.WinLoss;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                _cellInfo.CellValue = bettype.Margin;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, decimalNumberStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                _cellInfo.CellValue = bettype.BetCount;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, intNumberStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            }
        }

        /// <summary>
        /// Exports the total.
        /// </summary>
        /// <param name="total">The total.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="rowType">Type of the row.</param>
        /// <param name="groupName">Name of the group.</param>
        private void ExportTotal(List<BettypeWinLoss> total, XSSFSheet worksheet, int rowType, string groupName)
        {
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            XSSFCellStyle textStyle = rowType == _SubTotalRow ? _leftTotalByGroupTextStyle : _leftTotalTextStyle;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 3;
            _cellInfo.CellValue = string.Format("{0} {1}", rowType == _SubTotalRow ? groupName : Labels.Total, Labels.Turnover);
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, textStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CellValue = string.Format("{0} {1}", rowType == _SubTotalRow ? groupName : Labels.Total, Labels.Winloss);
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, textStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CellValue = string.Format("{0} {1}", rowType == _SubTotalRow ? groupName : Labels.Total, Labels.MarginPercent);
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, textStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CellValue = string.Format("{0} {1}", rowType == _SubTotalRow ? groupName : Labels.Total, Labels.BetCount);
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, textStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CurrentColumn = 4;
            _cellInfo.EndColumn = 4;
            ExportBettypeInfo(total, worksheet, rowType);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Gets the type of the decimal number style by row.
        /// </summary>
        /// <param name="rowType">Type of the row.</param>
        /// <returns>Decimal number style.</returns>
        private XSSFCellStyle GetDecimalNumberStyleByRowType(int rowType)
        {
            XSSFCellStyle cellStyle = null;
            switch (rowType)
            {
                case _NormalRow:
                    cellStyle = _decimalNumberStyle;
                    break;

                case _SubTotalRow:
                    cellStyle = _decimalTotalByGroupNumberStyle;
                    break;

                case _TotalRow:
                    cellStyle = _decimalTotalNumberStyle;
                    break;

                default:
                    break;
            }

            return cellStyle;
        }

        /// <summary>
        /// Gets the type of the integer number style by row.
        /// </summary>
        /// <param name="rowType">Type of the row.</param>
        /// <returns>The number style.</returns>
        private XSSFCellStyle GetIntNumberStyleByRowType(int rowType)
        {
            XSSFCellStyle cellStyle = null;
            switch (rowType)
            {
                case _NormalRow:
                    cellStyle = _intNumberStyle;
                    break;

                case _SubTotalRow:
                    cellStyle = _intTotalByGroupNumberStyle;
                    break;

                case _TotalRow:
                    cellStyle = _intTotalNumberStyle;
                    break;

                default:
                    break;
            }

            return cellStyle;
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
            _headerColor = Color.FromArgb(189, 215, 238);
            _totalRowByGroupBackgroundColor = Color.FromArgb(155, 193, 230);
            _totalRowBackgroundColor = Color.FromArgb(146, 209, 79);
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

            _totalRowBackgroundInfo = new BackgroundStyleInfo(
                _totalRowBackgroundColor,
                (int)FillPattern.SolidForeground);

            _totalRowByGroupBackgroundInfo = new BackgroundStyleInfo(
                _totalRowByGroupBackgroundColor,
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
            _leftTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _leftAlignmentInfo,
                _font);

            _leftTotalByGroupTextStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _totalRowByGroupBackgroundInfo,
               _leftAlignmentInfo,
               _boldFont);

            _leftTotalTextStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _totalRowBackgroundInfo,
               _leftAlignmentInfo,
               _boldFont);

            _intNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _intNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _decimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _intTotalByGroupNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _totalRowByGroupBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _intTotalByGroupNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _decimalTotalByGroupNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _totalRowByGroupBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _decimalTotalByGroupNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _intTotalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _intTotalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _decimalTotalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _decimalTotalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _headerTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _headerBackgroundInfo,
                _centerAlignmentInfo,
                _boldFont);
        }

        /// <summary>
        /// Initializes the font.
        /// </summary>
        private void InitFont()
        {
            _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, false);
            _boldFont = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
        }
    }
}