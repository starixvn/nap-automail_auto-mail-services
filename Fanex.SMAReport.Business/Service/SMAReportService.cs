// -----------------------------------------------------------------------
// <copyright file="SMAReportService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.SMAReport.Business.Model;
    using Fanex.SMAReport.Business.Repositories;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using Resources = Fanex.SMAReport.Business.Resources.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReportService : ISMAReportService
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

        private XSSFCellStyle _specialRowTextStyle;
        private XSSFCellStyle _rightTextStyle;
        private XSSFCellStyle _normalRowTextStyle;
        private XSSFCellStyle _titleTextStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _positiveNumberStyle;
        private XSSFCellStyle _positivePercentStyle;

        private Color _specialBackgroundColor;
        private Color _totalRowBackgroundColor;
        private Color _headerBackgroundColor;

        private AlignmentInfo _centerAlignmentInfo;
        private AlignmentInfo _rightAlignmentInfo;

        private List<string> _reportDirectory;

        public SMAReportService()
        {
            _repository = new SMAReportRepository();
        }

        /// <summary>
        /// Processes the SMA report.
        /// </summary>
        public void ProcessSMAReport()
        {
            if (!AppSettings.OnProcessing)
            {
                AppSettings.OnProcessing = true;
                SMAReportModel model = _repository.GetCurrentTask();

                try
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

                    SMAReport report = _repository.GetSMAReport();

                    this.ExportReportData(report);

                    bool isSuccess = Mailing.SendSMAReportEmail(_reportDirectory, _reportFrom, _reportTo);

                    if (isSuccess)
                    {
                        _repository.UpdateTaskStatus(model.Id, Status.Successful);
                    }
                    else
                    {
                        _repository.UpdateTaskStatus(model.Id, Status.Failed);
                        Mailing.ErrorEmailNotification(_reportFrom, _reportTo);
                    }

                    AppSettings.OnProcessing = false;
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    _repository.UpdateTaskStatus(model.Id, Status.Failed);
                    throw;
                }
            }
        }

        public int InsertTask(SMAReportModel task)
        {
            int id = _repository.InsertTask(task);
            return id;
        }

        public SMAReportModel GetTaskReadyToRun()
        {
            SMAReportModel task = _repository.GetTaskReadyRun();

            return task;
        }

        /// <summary>
        /// Exports the report data.
        /// </summary>
        /// <param name="report">The report.</param>
        private void ExportReportData(SMAReport report)
        {
            _reportDirectory = new List<string>();
            FileStream fileStream = null;
            bool isSuperReport = false;
            DateTime firstMonth = new DateTime(_lastMonth.Year, 1, 1);
            for (int i = 0; i < report.Report.Count; i++)
            {
                _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
                isSuperReport = i == 0 ? true : false;
                _workbook = new XSSFWorkbook();
                _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
                _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
                _redFont = NPOIHighVersion.CreateFont(_workbook, _redFontInfo, true);
                _titleFont = NPOIHighVersion.CreateFont(_workbook, _titleFontInfo, true);

                this.InitCellStyleInfo();

                string sheetName = Resources.SMA_Report_SheetName;
                XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;

                this.InitColumnWidth(worksheet, isSuperReport);
                this.ExportReportHeader(worksheet, isSuperReport);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                this.ExportReportContent(worksheet, report.Report[i]);
                worksheet.CreateFreezePane(0, 3, 0, 3);

                var reportDirectory = this.CreateSMAReportDataDirectory();
                string reportName = string.Format(
                    isSuperReport ? Resources.SMA_Super_Report_Name :
                    Resources.SMA_Master_Report_Name,
                    _reportFrom.Replace(' ', '_'),
                    _reportTo.Replace(' ', '_'));
                
                var reportPath = Path.Combine(reportDirectory, reportName);
                var fullReportPath = string.Format("{0}.{1}", reportPath, "xlsx");

                fileStream = new FileStream(
                        fullReportPath,
                        FileMode.Create,
                        FileAccess.Write);
                _workbook.Write(fileStream);
                _reportDirectory.Add(reportName);
                
                fileStream.Close();
                GC.Collect();
            }
        }

        /// <summary>
        /// Initializes the width of the column.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="isSuperReport">If set to <c>true</c> [is super report].</param>
        private void InitColumnWidth(XSSFSheet worksheet, bool isSuperReport)
        {
            AlignmentInfo titleAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Left,
                (int)VerticalAlignment.Center);
            _titleTextStyle = NPOIHighVersion.CreateCellStyle(_workbook, _borderInfo, _normalRowBackgroundInfo, titleAlignmentInfo, _titleFont);

            int numberColumn = _lastMonth.Month + 6;
            for (int i = 0; i < numberColumn; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(1, 100 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(3, 80 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(4, 110 * NPOIConstants.FontWidth);

            XSSFRow row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            _cellInfo.CellValue = string.Format(
                isSuperReport ? Resources.SMA_Super_Report_Title :
                                Resources.SMA_Master_Report_Title,
                                _reportFrom,
                                _reportTo);
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + numberColumn - 1;
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
        private void ExportReportHeader(
            XSSFSheet worksheet,
            bool isSuperReport)
        {
            int currentYearOfLastMonth = _lastMonth.Year;

            BackgroundStyleInfo headerBackgroundInfo = new BackgroundStyleInfo(
                _headerBackgroundColor,
                (int)FillPattern.SolidForeground);

            AlignmentInfo headerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);

            XSSFCellStyle headerStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                headerBackgroundInfo,
                headerAlignmentInfo,
                _font);

            _cellInfo.CellValue = "NO";
            XSSFRow row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = isSuperReport ? "Super" : "Master";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Virtual Rate %";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Currency";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Data";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            for (int i = 0; i < _lastMonth.Month; i++)
            {
                DateTime tempDateTime = new DateTime(currentYearOfLastMonth, i + 1, 1);
                var tempDateFormat = tempDateTime.ToString("MMM yy");
                _cellInfo.CellValue = tempDateFormat;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            }

            _cellInfo.CellValue = "Total";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
        }

        /// <summary>
        /// Exports the content of the report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportReportContent(
            XSSFSheet worksheet,
            SMAReport report)
        {
            XSSFRow row = null;
            int reportLength = report.Report.Count;
            for (int i = 0; i < reportLength - 1; i++)
            {
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
                row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;
                var currentUser = report.Report[i];

                _cellInfo.CellValue = i + 1;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentUser.UserName;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentUser.VirtualRate;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _rightTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentUser.Currency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

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

                _cellInfo.CellValue = string.Empty;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = string.Empty;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = string.Empty;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = "Margin %";
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);

                for (int j = 0; j < currentUser.ReportByMonths.Count; j++)
                {
                    _cellInfo.CurrentRow = _cellInfo.CurrentRow - 3;
                    _cellInfo.EndRow = _cellInfo.EndRow - 3;
                    var currentMonthData = currentUser.ReportByMonths[j];

                    if (currentMonthData.TurnOver != 0)
                    {
                        _cellInfo.CellValue = currentMonthData.TurnOver;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _numberStyle);
                    }
                    else
                    {
                        _cellInfo.CellValue = string.Empty;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                    }

                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    if (currentMonthData.WinLoss != 0)
                    {
                        _cellInfo.CellValue = currentMonthData.WinLoss;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _positiveNumberStyle);
                    }
                    else
                    {
                        _cellInfo.CellValue = string.Empty;
                        NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _specialRowTextStyle);
                    }

                    NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                    if (currentMonthData.WinLoss == 0 && currentMonthData.TurnOver == 0)
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

            this.ExportTotalSMAReport(_workbook, worksheet, report.Report[reportLength - 1]);
        }

        /// <summary>
        /// Exports the total SMA report.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="totalInfo">The total information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportTotalSMAReport(XSSFWorkbook workbook, XSSFSheet worksheet, SMAReport totalInfo)
        {
            XSSFCellStyle totalRowTextStyle = NPOIHighVersion.CreateCellStyle(
                workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _centerAlignmentInfo,
                _font);

            XSSFCellStyle totalNumberStyle = NPOIHighVersion.CreateCellStyle(
                workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            totalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            XSSFCellStyle totalPositiveNumberStyle = NPOIHighVersion.CreateCellStyle(
                workbook,
                _leftRightBorderInfo,
                _totalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            totalPositiveNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositiveNumberFormat);

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            XSSFRow row = worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 3;
            _cellInfo.CellValue = "Total";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalRowTextStyle);

            _cellInfo.EndRow = _cellInfo.CurrentRow;
            _cellInfo.CurrentColumn = _cellInfo.CurrentColumn + 3;
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Turnover";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalRowTextStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CellValue = "Winloss";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalRowTextStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            NPOIUtility.ResetColumnIndex(ref _cellInfo);
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 3;
            _cellInfo.CellValue = string.Empty;
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Margin %";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _normalRowTextStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            for (int i = 0; i < totalInfo.ReportByMonths.Count; i++)
            {
                _cellInfo.CurrentRow = _cellInfo.CurrentRow - 3;
                _cellInfo.EndRow = _cellInfo.EndRow - 3;
                var currentMonthData = totalInfo.ReportByMonths[i];

                if (currentMonthData.TurnOver != 0)
                {
                    _cellInfo.CellValue = currentMonthData.TurnOver;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalNumberStyle);
                }
                else
                {
                    _cellInfo.CellValue = string.Empty;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalRowTextStyle);
                }

                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
                if (currentMonthData.WinLoss != 0)
                {
                    _cellInfo.CellValue = currentMonthData.WinLoss;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalPositiveNumberStyle);
                }
                else
                {
                    _cellInfo.CellValue = string.Empty;
                    NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, totalRowTextStyle);
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

        private string CreateSMAReportDataDirectory()
        {
            string reportDataDirectory = AppSettings.ExportDataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);
            string directoryReportPath = Path.Combine(reportDataDirectory, _lastMonth.Year.ToString());
            DirectoryExtensions.CheckAndCreateDirectory(directoryReportPath);

            return directoryReportPath;
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
        }

        /// <summary>
        /// Initializes the cell style information.
        /// </summary>
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
                _redFont);
            _rightTextStyle.DataFormat = _dataFormat.GetFormat("0%");
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

        private void InitAlignmentInfo()
        {
            _centerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);
            _rightAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Right,
                (int)VerticalAlignment.Center);
        }
    }
}