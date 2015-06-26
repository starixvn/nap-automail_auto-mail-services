// -----------------------------------------------------------------------
// <copyright file="SMAReportService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Fanex.Common;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.NAPReport.SMAReport.Business.Constants;
    using Fanex.NAPReport.SMAReport.Business.Enums;
    using Fanex.NAPReport.SMAReport.Business.Model;
    using Fanex.NAPReport.SMAReport.Business.Repositories;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using OfficeOpenXmlCrypto;
    using Resources = Fanex.NAPReport.SMAReport.Business.Resources.Resources;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAExportExcelService : ISMAExportExcelService
    {
        private readonly ISMAReportRepository _repository = null;
        private DateTime _lastMonth = DateTime.Now.AddMonths(-1);
        private XSSFWorkbook _workbook;
        private BorderInfo _borderInfo;
        private BorderInfo _leftRightBorderInfo;
        private BorderInfo _noneTopBorderInfo;

        private FontAndColorInfo _fontInfo;
        private XSSFFont _font;
        private FontAndColorInfo _redFontInfo;
        private XSSFFont _redFont;
        private FontAndColorInfo _titleFontInfo;
        private XSSFFont _titleFont;

        private CellInfo _cellInfo;
        private XSSFDataFormat _dataFormat;

        private string _reportFrom = string.Empty;
        private string _reportTo = string.Empty;

        private BackgroundStyleInfo _normalRowBackgroundInfo;
        private BackgroundStyleInfo _specialRowBackgroundInfo;
        private BackgroundStyleInfo _totalRowBackgroundInfo;
        private BackgroundStyleInfo _headerBackgroundInfo;

        private XSSFCellStyle _specialRowTextStyle;
        private XSSFCellStyle _rightTextStyle;
        private XSSFCellStyle _normalRowTextStyle;
        private XSSFCellStyle _normalRowRightTextStyle;
        private XSSFCellStyle _specialRowRedTextStyle;
        private XSSFCellStyle _titleTextStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _positiveNumberStyle;
        private XSSFCellStyle _positivePercentStyle;
        private XSSFCellStyle _totalRowTextStyle;
        private XSSFCellStyle _totalNumberStyle;
        private XSSFCellStyle _totalPositiveNumberStyle;
        private XSSFCellStyle _headerStyle;
        private XSSFCellStyle _evenRowNumberStyle;
        private XSSFCellStyle _oddRowNumberStyle;
        private XSSFCellStyle _evenRowTextStyle;
        private XSSFCellStyle _oddRowTextStyle;
        private XSSFCellStyle _evenRowPercentStyle;
        private XSSFCellStyle _oddRowPercentStyle;

        private Color _specialBackgroundColor;
        private Color _totalRowBackgroundColor;
        private Color _headerBackgroundColor;

        private AlignmentInfo _centerAlignmentInfo;
        private AlignmentInfo _rightAlignmentInfo;
        private AlignmentInfo _titleAlignmentInfo;

        public SMAExportExcelService()
        {
            _repository = new SMAReportRepository();
            Init();
        }

        private void Init()
        {
            this.InitColor();
            this.InitAlignmentInfo();
            this.InitBorderStyleInfo();
            this.InitFontInfo();
            this.InitBackgroundColorInfo();
            _lastMonth = DateTime.Now.AddMonths(-1);
            DateTime firstMonth = new DateTime(_lastMonth.Year, 1, 1);
            _reportFrom = firstMonth.ToString("MMM yyyy");
            _reportTo = _lastMonth.ToString("MMM yyyy");
            FileNames = new List<string>();
            DirectoryOfFileNames = new List<string>();
        }
        
        public void ExportReport()
        {
            SMAReport report = _repository.GetSMAReport();

            string fileName = string.Empty;
            string directoryOfFileName = string.Empty;
                        
            bool isSuperReport = false;
            DateTime firstMonth = new DateTime(_lastMonth.Year, 1, 1);
            for (int i = 0; i < report.ReportRows.Count; i++)
            {
                _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
                isSuperReport = i == 0;
                _workbook = new XSSFWorkbook();
                _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
                _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
                _redFont = NPOIHighVersion.CreateFont(_workbook, _redFontInfo, true);
                _titleFont = NPOIHighVersion.CreateFont(_workbook, _titleFontInfo, true);

                this.InitCellStyleInfo();
                this.ExportReportByLevel(report.ReportRows[i], isSuperReport);

                fileName = string.Format(
                    isSuperReport ? Resources.SMA_Super_Report_Name :
                    Resources.SMA_Master_Report_Name,
                    _reportFrom.Replace(' ', '_'),
                    _reportTo.Replace(' ', '_')) + ".xlsx";

                directoryOfFileName = AppSettings.DataDirectoryWithYear + @"\" + fileName;
                SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYear, fileName, this._workbook, AppSettings.ExcelPasswordProtected);

                FileNames.Add(fileName);
                DirectoryOfFileNames.Add(directoryOfFileName); 
                
                GC.Collect();
            }
        }

        private void ExportReportByLevel(SMAReport report, bool isSuperReport)
        {
            this.ExportWinLossReport(report, isSuperReport);
            this.ExportVirtualRateReport(report, isSuperReport);
        }

        /// <summary>
        /// Exports the win loss report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        private void ExportWinLossReport(SMAReport report, bool isSuperReport)
        {
            _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
            string sheetName = Resources.SMA_Report_SheetName;
            XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            this.InitWinLossReportColumnWidthAndTitle(worksheet, isSuperReport);
            this.ExportWinLossReportHeader(worksheet, isSuperReport);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            this.ExportWinLossReportContent(worksheet, report);
            worksheet.CreateFreezePane(5, 3, 5, 3);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the virtual rate report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        private void ExportVirtualRateReport(SMAReport report, bool isSuperReport)
        {
            int maxUserPerSheet = ExcelConstants.MaxUserPerSheet;
            int redundancyUsers = report.NumberUserEnabledVirtualRate % maxUserPerSheet;
            int numberSheetBeforeRounding = Converter.ToInt(report.NumberUserEnabledVirtualRate / maxUserPerSheet);
            int numberSheet = redundancyUsers == 0 ? numberSheetBeforeRounding : numberSheetBeforeRounding + 1;

            for (int sheetIndex = 0; sheetIndex < numberSheet; sheetIndex++)
            {
                var virtualRateByUserInSheet = report.SMAVirtualRate.ReportRows.Skip(sheetIndex * maxUserPerSheet).Take(maxUserPerSheet).ToList();
                _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
                string sheetName = string.Format("{0} {1}", Resources.Virtual_Rate, sheetIndex + 1);
                XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
                this.InitVirtualRateReportColumnWidthAndTitle(worksheet, isSuperReport);
                this.ExportVirtualRateReportHeader(worksheet, isSuperReport);
                for (int userIndex = 0; userIndex < virtualRateByUserInSheet.Count; userIndex++)
                {
                    var virtualRateByUser = virtualRateByUserInSheet[userIndex];
                    this.ExportVirtualRateByUser(worksheet, userIndex, virtualRateByUser);
                }

                worksheet.CreateFreezePane(3, 4, 3, 4);
            }
        }

        /// <summary>
        /// Initializes the width of the column.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        private void InitWinLossReportColumnWidthAndTitle(XSSFSheet worksheet, bool isSuperReport)
        {
            int numberColumn = _lastMonth.Month + 6;
            for (int i = 0; i < numberColumn; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(1, 200 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(3, 80 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(4, 110 * NPOIConstants.FontWidth);

            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = this.GetReportTitle(isSuperReport);
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + numberColumn - 1;
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _titleTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
        }

        /// <summary>
        /// Initializes the virtual rate report column width and title.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        private void InitVirtualRateReportColumnWidthAndTitle(XSSFSheet worksheet, bool isSuperReport)
        {
            for (int i = 0; i < 13; i++)
            {
                worksheet.SetColumnWidth(i, 100 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(1, 200 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(2, 180 * NPOIConstants.FontWidth);

            worksheet.SetColumnWidth(9, 180 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(10, 180 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(11, 180 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(12, 180 * NPOIConstants.FontWidth);

            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = this.GetReportTitle(isSuperReport);
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 8;
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _titleTextStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportWinLossReportHeader(
            XSSFSheet worksheet,
            bool isSuperReport)
        {
            int currentYearOfLastMonth = _lastMonth.Year;
            _cellInfo.CellValue = "NO";
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = isSuperReport ? "Super" : "Master";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Virtual Rate";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Currency";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Data";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            for (int i = 0; i < _lastMonth.Month; i++)
            {
                DateTime tempDateTime = new DateTime(currentYearOfLastMonth, i + 1, 1);
                var tempDateFormat = tempDateTime.ToString("MMM yy");
                _cellInfo.CellValue = tempDateFormat;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            }

            _cellInfo.CellValue = "Total";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
        }

        /// <summary>
        /// Exports the virtual rate report header.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportVirtualRateReportHeader(
            XSSFSheet worksheet,
            bool isSuperReport)
        {
            _cellInfo.CellValue = "NO";
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = isSuperReport ? "Super" : "Master";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Sport";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.EndRow = _cellInfo.EndRow - 1;

            _cellInfo.CellValue = "Virtual Rate %";
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 9;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            _cellInfo.CurrentColumn = 3;
            _cellInfo.EndColumn = 3;

            _cellInfo.CellValue = "HDP";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "OU";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "OE";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "1st HDP";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "1st OU";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "1st OE";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "Moneyline / \n Match Winner";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "1st Moneyline";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "Set x Winner / \n Set x Winner(3rd)";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            _cellInfo.CellValue = "Set x Game y Winner";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _headerStyle);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the content of the report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportWinLossReportContent(
            XSSFSheet worksheet,
            SMAReport report)
        {
            XSSFRow row = null;
            int reportLength = report.SMAWinLoss.ReportRows.Count;
            for (int i = 0; i < reportLength - 1; i++)
            {
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
                row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                var currentUser = report.SMAWinLoss.ReportRows[i];

                _cellInfo.CellValue = i + 1;
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.EndRow = _cellInfo.EndRow - 1;
                _cellInfo.CellValue = currentUser.UserName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                bool isEnabledVirtualRate = report.EnabledVirtualRateUser.ContainsKey(currentUser.UserName);
                _cellInfo.CurrentRow = _cellInfo.CurrentRow - 1;
                _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
                _cellInfo.CellValue = currentUser.VirtualRate == 1 ? 
                    GetVirtualRateColumnValue(report.EnabledVirtualRateUser[currentUser.UserName]) :
                    "Disabled";
                NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, isEnabledVirtualRate ? _rightTextStyle : _specialRowRedTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.EndRow = _cellInfo.EndRow - 1;
                _cellInfo.CellValue = currentUser.Currency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CurrentRow = _cellInfo.CurrentRow - 1;
                _cellInfo.EndRow = _cellInfo.EndRow - 1;
                _cellInfo.CellValue = "Turnover";
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                _cellInfo.CellValue = "Winloss";
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                NPOIUtility.ResetColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = string.Empty;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentUser.UserName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = string.Empty;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowRightTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentUser.Currency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = "Margin %";
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                ExportWinLossInfoByMonth(worksheet, currentUser, RowType.Normal);
            }

            this.ExportWinLossTotalReport(worksheet, report.SMAWinLoss.ReportRows[reportLength - 1]);
        }

        /// <summary>
        /// Exports the virtual rate by user.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="userIndex">Index of the user.</param>
        /// <param name="virtualRateByUser">The virtual rate by user.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportVirtualRateByUser(XSSFSheet worksheet, int userIndex, SMAVirtualRate virtualRateByUser)
        {
            int numberMergeRow = virtualRateByUser.ReportRows.Count;
            bool evenRow = userIndex % 2 == 0;
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = userIndex + 1;
            _cellInfo.EndRow = _cellInfo.CurrentRow + numberMergeRow - 1;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowNumberStyle : _evenRowNumberStyle);
            _cellInfo.EndRow = _cellInfo.EndRow - numberMergeRow + 1;

            foreach (var virtualRateBySportAndLiveStatus in virtualRateByUser.ReportRows)
            {
                _cellInfo.CurrentColumn = 1;
                _cellInfo.EndColumn = 1;

                _cellInfo.CellValue = virtualRateByUser.UserName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowTextStyle : _evenRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = virtualRateBySportAndLiveStatus.Sport;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowTextStyle : _evenRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                VirtualRateByBettype virtualRateByBettype = null;
                for (int i = 0; i < 6; i++)
                {
                    virtualRateByBettype = virtualRateBySportAndLiveStatus.VirtualRateByBettype[i];
                    SetCellValue(virtualRateByBettype.VirtualRate, ref _cellInfo);
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowPercentStyle : _evenRowPercentStyle);
                    NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                }

                virtualRateByBettype = virtualRateBySportAndLiveStatus.VirtualRateByBettype[6];
                SetCellValue(virtualRateByBettype.VirtualRate, ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowPercentStyle : _evenRowPercentStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                virtualRateByBettype = virtualRateBySportAndLiveStatus.VirtualRateByBettype[7];
                SetCellValue(virtualRateByBettype.VirtualRate, ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowPercentStyle : _evenRowPercentStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                virtualRateByBettype = virtualRateBySportAndLiveStatus.VirtualRateByBettype[8];
                SetCellValue(virtualRateByBettype.VirtualRate, ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowPercentStyle : _evenRowPercentStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                virtualRateByBettype = virtualRateBySportAndLiveStatus.VirtualRateByBettype[11];
                SetCellValue(virtualRateByBettype.VirtualRate, ref _cellInfo);
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, evenRow ? _oddRowPercentStyle : _evenRowPercentStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the win loss total report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="totalInfo">The total information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportWinLossTotalReport(XSSFSheet worksheet, SMAWinLoss totalInfo)
        {
            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 3;
            _cellInfo.CellValue = "Total";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _totalRowTextStyle);

            _cellInfo.EndRow = _cellInfo.CurrentRow;
            _cellInfo.CurrentColumn = _cellInfo.CurrentColumn + 3;
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Turnover";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _totalRowTextStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CellValue = "Winloss";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _totalRowTextStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 3;
            _cellInfo.CellValue = string.Empty;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Margin %";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            this.ExportWinLossInfoByMonth(worksheet, totalInfo, RowType.Total);
        }

        /// <summary>
        /// Exports the win loss information by month.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="winLossInfo">The win loss information.</param>
        /// <param name="rowType">Type of the row.</param>
        private void ExportWinLossInfoByMonth(XSSFSheet worksheet, SMAWinLoss winLossInfo, RowType rowType)
        {
            for (int i = 0; i < winLossInfo.ReportByMonths.Count; i++)
            {
                _cellInfo.CurrentRow = _cellInfo.CurrentRow - 3;
                _cellInfo.EndRow = _cellInfo.EndRow - 3;
                XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                var currentMonthData = winLossInfo.ReportByMonths[i];

                if (currentMonthData.TurnOver != 0)
                {
                    _cellInfo.CellValue = currentMonthData.TurnOver;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, rowType == RowType.Total ? _totalNumberStyle : _numberStyle);
                }
                else
                {
                    _cellInfo.CellValue = string.Empty;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, rowType == RowType.Total ? _totalRowTextStyle : _specialRowTextStyle);
                }

                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                if (currentMonthData.WinLoss != 0)
                {
                    _cellInfo.CellValue = currentMonthData.WinLoss;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, rowType == RowType.Total ? _totalPositiveNumberStyle : _positiveNumberStyle);
                }
                else
                {
                    _cellInfo.CellValue = string.Empty;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, rowType == RowType.Total ? _totalRowTextStyle : _specialRowTextStyle);
                }

                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                if (currentMonthData.TurnOver == 0 && currentMonthData.WinLoss == 0)
                {
                    _cellInfo.CellValue = string.Empty;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                }
                else
                {
                    _cellInfo.CellValue = currentMonthData.Margin;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _positivePercentStyle);
                }

                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            }
        }

        private void InitColor()
        {
            _specialBackgroundColor = Color.FromArgb(189, 215, 238);
            _totalRowBackgroundColor = Color.FromArgb(155, 193, 230);
            _headerBackgroundColor = Color.FromArgb(146, 209, 79);
        }

        /// <summary>
        /// Initializes the background color information.
        /// </summary>
        private void InitBackgroundColorInfo()
        {
            _normalRowBackgroundInfo = new BackgroundStyleInfo(
                Color.White,
                (int)FillPattern.SolidForeground);

            _specialRowBackgroundInfo = new BackgroundStyleInfo(
                _specialBackgroundColor,
                (int)FillPattern.SolidForeground);

            _totalRowBackgroundInfo = new BackgroundStyleInfo(
                _totalRowBackgroundColor,
                (int)FillPattern.SolidForeground);

            _headerBackgroundInfo = new BackgroundStyleInfo(
                _headerBackgroundColor,
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
            _specialRowTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _specialRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);

            _normalRowTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _noneTopBorderInfo,
                _normalRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);

            _numberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _specialRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _numberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _positiveNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _specialRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _positiveNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositiveNumberFormat);

            _positivePercentStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _noneTopBorderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _positivePercentStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositivePercentFormat);

            _rightTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _specialRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);

            _specialRowRedTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _specialRowBackgroundInfo,
                _rightAlignmentInfo,
                _redFont);

            _normalRowRightTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _noneTopBorderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);

            _totalRowTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);

            _totalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _totalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _totalPositiveNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _totalPositiveNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositiveNumberFormat);

            _titleTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _titleAlignmentInfo,
                _titleFont);

            _headerStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _headerBackgroundInfo,
               _centerAlignmentInfo,
               _font);

            _evenRowNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _specialRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);
            _evenRowNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _oddRowNumberStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _normalRowBackgroundInfo,
               _centerAlignmentInfo,
               _font);
            _oddRowNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _evenRowTextStyle = NPOIHighVersion.CreateCellStyle(
                    _workbook,
                    _borderInfo,
                    _specialRowBackgroundInfo,
                    _centerAlignmentInfo,
                    _font);

            _oddRowTextStyle = NPOIHighVersion.CreateCellStyle(
                    _workbook,
                    _borderInfo,
                    _normalRowBackgroundInfo,
                    _centerAlignmentInfo,
                    _font);

            _evenRowPercentStyle = NPOIHighVersion.CreateCellStyle(
                       _workbook,
                       _borderInfo,
                       _specialRowBackgroundInfo,
                       _centerAlignmentInfo,
                       _font);
            _evenRowPercentStyle.DataFormat = _dataFormat.GetFormat("0%");

            _oddRowPercentStyle = NPOIHighVersion.CreateCellStyle(
                       _workbook,
                       _borderInfo,
                       _normalRowBackgroundInfo,
                       _centerAlignmentInfo,
                       _font);
            _oddRowPercentStyle.DataFormat = _dataFormat.GetFormat("0%");
        }

        /// <summary>
        /// Initializes the border style information.
        /// </summary>
        private void InitBorderStyleInfo()
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

            _leftRightBorderInfo = new BorderInfo(
                (short)BorderStyle.Thin,
                (short)BorderStyle.None,
                (short)BorderStyle.Thin,
                (short)BorderStyle.None,
                Color.Gray,
                Color.White,
                Color.Gray,
                Color.White);

            _noneTopBorderInfo = new BorderInfo(
                (short)BorderStyle.Thin,
                (short)BorderStyle.None,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                Color.White,
                Color.Gray,
                Color.Gray);
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

            _redFontInfo = new FontAndColorInfo(
                NPOIConstants.FontName,
                NPOIConstants.FontSize,
                Color.Red);

            _titleFontInfo = new FontAndColorInfo(
                NPOIConstants.FontName,
                13,
                Color.Black);
        }

        /// <summary>
        /// Initializes the alignment information.
        /// </summary>
        private void InitAlignmentInfo()
        {
            _centerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);
            _rightAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Right,
                (int)VerticalAlignment.Center);
            _titleAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Left,
                (int)VerticalAlignment.Center);
        }

        private string GetReportTitle(bool isSuperReport)
        {
            return string.Format(
                isSuperReport ? Resources.SMA_Super_Report_Title : Resources.SMA_Master_Report_Title,
                _reportFrom,
                _reportTo);
        }

        private string GetVirtualRateColumnValue(int userIndex)
        {
            int sheetIndexBeforRounding = Converter.ToInt((userIndex + 1) / ExcelConstants.MaxUserPerSheet);
            int sheetIndex = (userIndex + 1) % ExcelConstants.MaxUserPerSheet == 0 ?
                sheetIndexBeforRounding :
                sheetIndexBeforRounding + 1;
            return string.Format("Virtual Rate {0}", sheetIndex);
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="virtualRate">The virtual rate.</param>
        /// <param name="cellInfo">The cell information.</param>
        private void SetCellValue(decimal? virtualRate, ref CellInfo cellInfo)
        {
            decimal? virtualRateValue = virtualRate / 100;
            
            if (virtualRateValue == null)
            {
                cellInfo.CellValue = string.Empty;
            }
            else
            {
                cellInfo.CellValue = virtualRateValue;
            }
        }


        public List<string> DirectoryOfFileNames
        {
            get;
            set;
        }

        public List<string> FileNames
        {
            get;
            set;
        }
    }
}