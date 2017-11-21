// -----------------------------------------------------------------------
// <copyright file="DailyTurnoverExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.DailyTurnover
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;
    using Fanex.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyTurnoverExportService : IDailyTurnoverExportService
    {
        private readonly XSSFWorkbook _workbook;
        private readonly Cell _cell;
        private readonly INpoiNewFormatFontService _fontService;
        private readonly INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private readonly INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private readonly XSSFFont _regularFont;

        private Color _headerColor;
        private Color _currencyTypeColor;
        private XSSFSheet _workSheet;

        public DailyTurnoverExportService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
        }

        public string FileName
        {
            get;
            set;
        }

        public void ExportReport(List<DailyTurnover> dailyTurnover, DateTime winlossDate)
        {
            try
            {
                this.CreateSheet();
                this.InitColumnWidth();
                this.ExportReportTitle(winlossDate);
                this.ExportReportHeader();
                this.ExportReportContent(dailyTurnover);
                string parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDataDirectoryDailyTurnover);
                string reportName = string.Format(ReportingServicesResource.DailyTurnover_ReportName, winlossDate.ToString("yyyy_MM_dd"));
                FileName = Path.Combine(parentDirectory, reportName);
                SaveFile.SaveExcelFile(AppSettings.ExportDataDirectoryDailyTurnover, parentDirectory, reportName, _workbook, AppSettings.ExcelPassword);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _currencyTypeColor = Color.FromArgb(244, 244, 190);
        }

        private void CreateSheet()
        {
            string sheetName = ReportingServicesResource.DailyTurnover_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            for (var i = 0; i < 7; i++)
            {
                _workSheet.SetColumnWidth(i, 170 * NpoiConstants.FontWidth);
            }

            _workSheet.SetColumnWidth(6, 300 * NpoiConstants.FontWidth);
        }

        private void ExportReportTitle(DateTime currentTime)
        {
            var headerStyle = this.CreateHeaderStyle();
            _cell.CellValue = string.Format(
                    ReportingServicesResource.DailyTurnover_ReportTitle,
                    currentTime.ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 6;
            _cell.EndRow = _cell.CurrentRow + 1;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow = _cell.EndRow + 2;
            _cell.EndRow = _cell.CurrentRow;
        }

        private void ExportReportHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            this.RenderCell(Labels.Date, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Total_Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Super_Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Master_Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Agent_Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.IBC_Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.CNT, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportContent(List<DailyTurnover> dailyTurnover)
        {
            foreach (var dailyTurnoverByCurrency in dailyTurnover)
            {
                this.ExportReportByCurrency(dailyTurnoverByCurrency);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportReportByCurrency(DailyTurnover dailyTurnoverByCurrency)
        {
            var currencyTypeCellStye = _customCellStyleService.CreateLeftTextStyle(_currencyTypeColor, _fontService.BlackBoldFont);
            _cell.CellValue = dailyTurnoverByCurrency.CurrencyType;
            _cell.EndColumn = _cell.CurrentColumn + 6;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, currencyTypeCellStye);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
            this.RenderCell(dailyTurnoverByCurrency.Date, _bodyCellStyleService.CenterTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TurnoverDetail.TotalTurnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TurnoverDetail.SuperTurnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TurnoverDetail.MasterTurnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TurnoverDetail.AgentTurnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TurnoverDetail.IBCTurnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(dailyTurnoverByCurrency.TicketNormalRejectCount, _bodyCellStyleService.CenterTextStyle);
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private void RenderCell(object cellValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
        }
    }
}