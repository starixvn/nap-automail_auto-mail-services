// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyBalance
{
    using System;
    using System.Drawing;
    using System.IO;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;
    using Fanex.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalanceExportService : ICompanyBalanceExportService
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
        private bool _isYesterdayBalance;

        public CompanyBalanceExportService(bool isYesterdayBalance)
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _alignmentService = new NpoiNewFormatAlignmentService();
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _isYesterdayBalance = isYesterdayBalance;
        }

        public string FileName
        {
            get;
            set;
        }

        public void ExportReport(CompanyBalance balance, DateTime winlossDate)
        {
            try
            {
                this.CreateSheet();
                this.InitColumnWidth();
                this.ExportReportTitle(winlossDate);
                this.ExportReportHeader();
                this.ExportReportContent(balance);
                this.ExportGrandTotal(balance.GrandTotal);

                string parentDirectory = string.Empty;
                string reportName = string.Empty;
                string rootDataDirectory = AppSettings.ExportDataDirectoryCompanyBalance;
                if (_isYesterdayBalance)
                {
                    parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDataDirectoryCompanyYesterdayBalance);
                    reportName = string.Format(ReportingServicesResource.CompanyYesterdayBalance_ReportName, winlossDate.ToString("yyyy_MM_dd"));
                }
                else
                {
                    parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDataDirectoryCompanyBalance);
                    reportName = string.Format(ReportingServicesResource.CompanyBalance_ReportName, winlossDate.ToString("yyyy_MM_dd"));
                }

                FileName = Path.Combine(parentDirectory, reportName);
                SaveFile.SaveExcelFile(rootDataDirectory, parentDirectory, reportName, _workbook, AppSettings.ExcelPassword);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _evenRowColor = Color.FromArgb(228, 242, 253);
            _grandTotalColor = Color.FromArgb(219, 225, 187);
            _grandTotalBorderColor = Color.FromArgb(255, 102, 0);
        }

        private void CreateSheet()
        {
            string sheetName = ReportingServicesResource.CompanyBalance_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 80 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 150 * NPOIConstants.FontWidth);
        }

        private void ExportReportTitle(DateTime currentTime)
        {
            var headerStyle = this.CreateHeaderStyle();
            string title = _isYesterdayBalance ? ReportingServicesResource.CompanyYesterdayBalance_ReportTitle : ReportingServicesResource.CompanyBalance_ReportTitle;
            _cell.CellValue = string.Format(title, currentTime.ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 4;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            this.RenderCell(Labels.Curency, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Plus_Minus_With_Slash, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Plus_Minus_With_Slash_RM, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Today_Rate, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Yesterday_Rate, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportContent(CompanyBalance companyBalance)
        {
            var evenRowTextStyle = CreateEvenRowTextStyle();
            var evenRowDecimalNumberStyle = CreateEvenRowDecimalNumberStyle();

            for (int i = 0; i < companyBalance.ReportRows.Count; i++)
            {
                var balanceItem = companyBalance.ReportRows[i];
                var textStyle = i % 2 == 0 ? _bodyCellStyleService.CenterTextStyle : evenRowTextStyle;
                var decimalNumberStyle = i % 2 == 0 ? _bodyCellStyleService.DecimalNumberStyle : evenRowDecimalNumberStyle;
                this.RenderCell(balanceItem.CurrencyName, textStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceItem.TotalAmountLocal, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceItem.TotalAmount, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceItem.TodayRate, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                this.RenderCell(balanceItem.YesterdayRate, decimalNumberStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportGrandTotal(CompanyBalance balanceGrandTotal)
        {
            var textStyle = this.CreateGrandTotalTextStyle();
            var decimalNumberStyle = this.CreateGrandTotalDecimalNumberStyle();
            this.RenderCell(string.Empty, textStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Total, textStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(balanceGrandTotal.TotalAmount, decimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(string.Empty, textStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(string.Empty, textStyle);
            _cell.ResetColumnIndex();
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private XSSFCellStyle CreateEvenRowDecimalNumberStyle()
        {
            XSSFCellStyle evenRowDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(_evenRowColor, _regularFont);
            return evenRowDecimalNumberStyle;
        }

        private XSSFCellStyle CreateEvenRowTextStyle()
        {
            XSSFCellStyle evenRowTextStyle = _customCellStyleService.CreateCenterTextStyle(_evenRowColor, _regularFont);
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
                _alignmentService.CenterAlignment,
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
    }
}