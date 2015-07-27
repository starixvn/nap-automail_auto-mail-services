// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalanceExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.SuperYesterdayBalance
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalanceExportService : ISuperYesterdayBalanceExportService
    {
        private Color _headerColor;
        private Color _evenRowColor;
        private Color _grandTotalColor;
        private Color _grandTotalBorderColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;
        private INpoiAlignmentService _alignmentService;
        private XSSFDataFormat _dataFormat;

        public SuperYesterdayBalanceExportService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            this.CreateColor();
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            _fontService = new NpoiNewFormatFontService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);            
        }

        public void ExportReport(SuperYesterdayBalance balance, DateTime currentTime)
        {
            try
            {
                this.CreateSheet();
                this.InitColumnWidth();
                this.ExportReportTitle();
                this.ExportReportHeader();
                this.ExportReportContent(balance.ReportRows);
                this.ExportGrandTotal(balance.BaseBalanceTotal);
                string parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDataDirectorySuperYesterdayBalance);
                string reportName = string.Format(
                    ReportingServicesResource.SuperYesterdayBalance_ReportName,
                    currentTime.AddDays(-1).ToShortFormatedDateWithSlash().Replace('/', '_'));
                string reportNameWithExtension = string.Format("{0}.xlsx", reportName);
                FileName = Path.Combine(parentDirectory, reportNameWithExtension);
                SaveFile.SaveExcelFile(parentDirectory, reportNameWithExtension, _workbook, AppSettings.ExcelPassword);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void CreateSheet()
        {
            string sheetName = ReportingServicesResource.SuperYesterdayBalance_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            for (var i = 0; i < 5; i++)
            {
                _workSheet.SetColumnWidth(i, 170 * NPOIConstants.FontWidth);
            }

            _workSheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 120 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
        }

        private void ExportReportTitle()
        {
            var headerStyle = this.CreateHeaderStyle();
            _cell.CellValue = ReportingServicesResource.SuperYesterdayBalance_ReportTitle;
            _cell.EndColumn = _cell.CurrentColumn + 4;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            this.RenderCell(Labels.No, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Super, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Curency, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Balance, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.BaseRM, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportContent(List<SuperYesterdayBalanceDetail> balanceDetails)
        {
            var evenRowNumberStyle = CreateEvenRowNumberStyle();
            var evenRowTextStyle = CreateEvenRowTextStyle();
            var evenRowDecimalNumberStyle = CreateEvenRowDecimalNumberStyle();
            for (int i = 0; i < balanceDetails.Count; i++)
            {
                var balanceDetail = balanceDetails[i];
                var numberStyle = i % 2 == 0 ? _bodyCellStyleService.RightNumberStyle : evenRowNumberStyle;
                var textStyle = i % 2 == 0 ? _bodyCellStyleService.LeftTextStyle : evenRowTextStyle;
                var decimalNumberStyle = i % 2 == 0 ? _bodyCellStyleService.DecimalNumberStyle : evenRowDecimalNumberStyle;
                this.RenderCell(i + 1, numberStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceDetail.Username, textStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceDetail.Currency, textStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceDetail.Balance, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceDetail.BaseBalance, decimalNumberStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportGrandTotal(SuperYesterdayBalanceDetail balanceGrandTotal)
        {
            var textStyle = this.CreateGrandTotalTextStyle();
            var decimalNumberStyle = this.CreateGrandTotalDecimalNumberStyle();
            _cell.CellValue = Labels.Grand_Total;
            _cell.EndColumn = _cell.CurrentColumn + 3;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, textStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(balanceGrandTotal.BaseBalance, decimalNumberStyle);
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _evenRowColor = Color.FromArgb(228, 242, 253);
            _grandTotalColor = Color.FromArgb(219, 225, 187);
            _grandTotalBorderColor = Color.FromArgb(255, 102, 0);
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private XSSFCellStyle CreateEvenRowNumberStyle()
        {
            XSSFCellStyle evenRowNumberStyle = _customCellStyleService.CreateRightNumberStyle(_evenRowColor, _regularFont);
            return evenRowNumberStyle;
        }

        private XSSFCellStyle CreateEvenRowDecimalNumberStyle()
        {
            XSSFCellStyle evenRowDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(_evenRowColor, _regularFont);
            return evenRowDecimalNumberStyle;
        }

        private XSSFCellStyle CreateEvenRowTextStyle()
        {
            XSSFCellStyle evenRowTextStyle = _customCellStyleService.CreateLeftTextStyle(_evenRowColor, _regularFont);
            return evenRowTextStyle;
        }

        private XSSFCellStyle CreateGrandTotalTextStyle()
        {
            var borderInformation = new BorderInformation(
               (short)BorderStyle.Thin,
               (short)BorderStyle.Medium,
               (short)BorderStyle.Thin,
               (short)BorderStyle.Thin,
               Color.Gray,
               _grandTotalBorderColor,
               Color.Gray,
               Color.Gray);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, _grandTotalColor);
            XSSFCellStyle grandTotalTextStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            return grandTotalTextStyle;
        }

        private XSSFCellStyle CreateGrandTotalDecimalNumberStyle()
        {
            var borderInformation = new BorderInformation(
               (short)BorderStyle.Thin,
               (short)BorderStyle.Medium,
               (short)BorderStyle.Thin,
               (short)BorderStyle.Thin,
               Color.Gray,
               _grandTotalBorderColor,
               Color.Gray,
               Color.Gray);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, _grandTotalColor);
            XSSFCellStyle grandTotalDecimalNumberStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            grandTotalDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);
            return grandTotalDecimalNumberStyle;
        }

        private void RenderCell(object cellValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
        }

        public string FileName
        {
            get;
            set;
        }
    }
}