// -----------------------------------------------------------------------
// <copyright file="MonthlyWinLossByLeagueService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Fanex.MonthlyWinlossByLeague.Business.Model;
    using Fanex.MonthlyWinlossByLeague.Business.Repositories;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Extensions;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using Fanex.Service.Core.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLossByLeagueService : IMonthlyWinLossByLeagueService
    {
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
                            var bettypeInfo = new BettypeWinLoss
                            {
                                Bettype = bettypeName,
                                WinLoss = item.BetTypeWinLoss.WinLoss,
                                Turnover = item.BetTypeWinLoss.Turnover,
                                BetCount = item.BetTypeWinLoss.BetCount
                            };

                            leagueWinLoss.BettypeWinLoss[bettypeIndex] = bettypeInfo;
                            leagueWinLoss.MatchCount += item.BetTypeWinLoss.MatchCount;
                            groupLeague.Total[bettypeIndex].CalculateTotal(bettypeInfo);
                            periodAndStatusGroup.Total[bettypeIndex].CalculateTotal(bettypeInfo);
                        }

                        groupLeague.LeagueWinLoss.Add(leagueWinLoss);
                    }

                    periodAndStatusGroup.GroupLeagueWinLoss.Add(groupLeague);
                }

                report.Add(periodAndStatusGroup);
            }

            return report;
        }

        public Dictionary<int, string> Bettypes
        {
            get
            {
                return _bettypes;
            }
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

        public void ProcessMonthlyWinLossByLeagueReport()
        {
            _lastMonth = DateTime.Now.AddMonths(-1);
            var report = GetReport();

            if (report.Count > 0)
            {
                this.ExportReport(report);
            }
        }

        private void InitBettypes()
        {
            int index = 0;
            var bettypeList = _repository.Bettypes;

            _bettypes = new Dictionary<int, string>();
            _bettypes.Add(index++, "Handicap");
            _bettypes.Add(index++, "1H Handicap");
            _bettypes.Add(index++, "1H Over/Under");
            _bettypes.Add(index++, "Over/Under");

            foreach (string bettype in bettypeList)
            {
                if (!_bettypes.ContainsValue(bettype))
                {
                    _bettypes.Add(index++, bettype);
                }
            }
        }

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

        private string CreateMonthlyWinLossByLeagueReportDataDirectory()
        {
            string reportDataDirectory = AppSettings.ExportDataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);
            string directoryReportPath = Path.Combine(reportDataDirectory, _lastMonth.Year.ToString());
            DirectoryExtensions.CheckAndCreateDirectory(directoryReportPath);

            return directoryReportPath;
        }

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
            }

            var reportDirectory = this.CreateMonthlyWinLossByLeagueReportDataDirectory();
            string reportName = "Monthly_Winloss_Report_by_League";

            var reportPath = Path.Combine(reportDirectory, reportName);
            var fullReportPath = string.Format("{0}.{1}", reportPath, "xlsx");

            fileStream = new FileStream(
                    fullReportPath,
                    FileMode.Create,
                    FileAccess.Write);
            _workbook.Write(fileStream);

            fileStream.Close();
            GC.Collect();
        }

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

        private void ExportReportHeader(XSSFSheet worksheet)
        {
            XSSFRow row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = "Group";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "League";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "No of Match";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Data";
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

        private void ExportReportContent(XSSFSheet worksheet, MonthlyWinLoss report)
        {
            XSSFRow row = null;
            foreach (var groupLeagueWinLoss in report.GroupLeagueWinLoss)
            {
                int groupIndex = 0;
                row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                int groupLength = groupLeagueWinLoss.LeagueWinLoss.Count;
                _cellInfo.CellValue = groupLeagueWinLoss.GroupName;
                _cellInfo.EndRow = _cellInfo.CurrentRow + groupLength * 4 - 1;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                foreach (var leagueWinLoss in groupLeagueWinLoss.LeagueWinLoss)
                {
                    _cellInfo.EndRow = groupIndex == 0 ? (_cellInfo.EndRow - groupLength * 4 - 1) : _cellInfo.EndRow;
                    groupIndex++;
                    _cellInfo.EndRow = _cellInfo.CurrentRow + 3;
                    _cellInfo.CellValue = leagueWinLoss.LeagueName;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                    _cellInfo.CellValue = leagueWinLoss.MatchCount;
                    NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _intNumberStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                    
                    _cellInfo.EndRow = _cellInfo.EndRow - 3;
                    _cellInfo.CellValue = "Turnover Sunbet";
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = "Winloss Sunbet";
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = "Margin %";
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                    _cellInfo.CellValue = "BetCount";
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _leftTextStyle);
                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                    foreach (var bettype in leagueWinLoss.BettypeWinLoss)
                    {
                        _cellInfo.CurrentRow = _cellInfo.CurrentRow - 4;
                        _cellInfo.EndRow = _cellInfo.EndRow - 4;
                        _cellInfo.CellValue = bettype.Turnover;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                        NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                        _cellInfo.CellValue = bettype.WinLoss;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                        NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                        _cellInfo.CellValue = bettype.Margin;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                        NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                        _cellInfo.CellValue = bettype.BetCount;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _intNumberStyle);
                        NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                        NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                    }

                    _cellInfo.CurrentColumn = 1;
                    _cellInfo.EndColumn = 1;
                }

                _cellInfo.CurrentColumn = 0;
                _cellInfo.EndColumn = 0;
            }
        }

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

        private void InitColor()
        {
            _headerColor = Color.FromArgb(189, 215, 238);
            _totalRowByGroupBackgroundColor = Color.FromArgb(155, 193, 230);
            _totalRowBackgroundColor = Color.FromArgb(146, 209, 79);
        }

        private void InitFontInfo()
        {
            _fontInfo = new FontAndColorInfo(
                    NPOIConstants.FontName,
                    NPOIConstants.FontSize,
                    Color.Black);
        }

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
        private void InitCellStyleInfo()
        {
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

        private void InitFont()
        {
            _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, false);
            _boldFont = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
        }
    }
}