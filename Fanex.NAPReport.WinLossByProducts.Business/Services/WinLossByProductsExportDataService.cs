// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsExportDataService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System;
    using System.Drawing;
    using System.IO;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Resources;
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
    public class WinLossByProductsExportDataService : IWinLossByProductsExportDataService
    {
        private const string PartnersGroupName = "partners";
        private Color _headerColor;
        private Color _subTotalColor;
        private Color _grandTotalColor;
        private Color _grandTotalBorderColor;
        private Color _racingDueToPartnersColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;
        private XSSFFont _regularFont;
        private string _reportPath;

        public WinLossByProductsExportDataService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();
        }

        public string DirectoryOfFileName
        {
            get
            {
                return _reportPath;
            }
        }

        public string FileName { get; set; }

        public void ExportReportToExcel(WinLossByProducts report, GetWinLossByProductsCriteria criteria)
        {
            CreateSheet();
            InitColumnWidth();
            ExportReport(report);
            FileName = CreateReportNameWithExtension(criteria);
            _reportPath = AppSettings.DataDirectoryWithYearMonth + @"\" + FileName;
            SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYearMonth, FileName, this._workbook, AppSettings.ExcelPasswordProtected); 
        }

        private void ExportReport(WinLossByProducts report)
        {
            if (report.Sportbook.Count > 0 || report.ThirdPartySport.Count > 0)
            {
                this.ExportReportHeader();
                this.ExportSportBookContent(report);
                this.ExportSportbookTotal(report);
                this.ExportThirdPartySport(report);
            }

            if (report.Total != null)
            {
                this.ExportReportTotal(report);
            }
        }

        private void ExportReportHeader()
        {
            XSSFCellStyle headerStyle = CreateHeaderStyle();
            string reportTile = string.Format(Resources.WinLossByProducts_ReportTitle, DateTime.Now.AddDays(-1).ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 2;
            _cell.CellValue = reportTile;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow += 2;
            _cell.EndRow = _cell.CurrentRow;
            _cell.CellValue = Labels.Product;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Win_Loss;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.MonthtoDate;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportSportBookContent(WinLossByProducts report)
        {
            XSSFCellStyle textStyle = _bodyCellStyleService.LeftTextStyle;
            XSSFCellStyle decimalNumberStyle = _bodyCellStyleService.DecimalNumberStyle;
            foreach (var sportbook in report.Sportbook)
            {
                _cell.CellValue = sportbook.ProductName;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.WinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.MonthToDateWinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportThirdPartySport(WinLossByProducts report)
        {
            XSSFCellStyle textStyle = _bodyCellStyleService.LeftTextStyle;
            XSSFCellStyle decimalNumberStyle = _bodyCellStyleService.DecimalNumberStyle;
            XSSFFont racingDueToPartnerFont = CreateRacingDueToPartnerFont();
            XSSFCellStyle racingDueToPartnersTextStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, racingDueToPartnerFont);
            XSSFCellStyle racingDueToPartnersDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, racingDueToPartnerFont);
            XSSFCellStyle thirdPartySportTextStyle = null;
            XSSFCellStyle thirdPartySportDecimalNumberStyle = null;

            foreach (var thirdPartySport in report.ThirdPartySport)
            {
                bool isRacingDueToPartners = IsRacingDueToPartnersSport(thirdPartySport);
                thirdPartySportTextStyle = GetThirdPartySportTextStyle(isRacingDueToPartners, textStyle, racingDueToPartnersTextStyle);
                thirdPartySportDecimalNumberStyle = GetThirdPartySportDecimalNumberStyle(
                    isRacingDueToPartners,
                    decimalNumberStyle,
                    racingDueToPartnersDecimalNumberStyle);
                _cell.CellValue = thirdPartySport.ProductName;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, thirdPartySportTextStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = thirdPartySport.WinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, thirdPartySportDecimalNumberStyle);

                _cell.IncreaseColumnIndex();
                _cell.CellValue = thirdPartySport.MonthToDateWinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, thirdPartySportDecimalNumberStyle);
                _cell.IncreaseRowIndex();
                _cell.ResetColumnIndex();
            }
        }

        private void ExportReportTotal(WinLossByProducts report)
        {
            XSSFDataFormat dataFormat = this._workbook.CreateDataFormat() as XSSFDataFormat;
            BorderInformation borderInformation = new BorderInformation(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Medium,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                _grandTotalBorderColor,
                Color.Gray,
                Color.Gray);
            BackgroundInformation backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, _grandTotalColor);
            XSSFCellStyle grandTotalTextStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.LeftAlignment,
                _fontService.BlackBoldFont);
            XSSFCellStyle grandTotalDecimalNumberStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            grandTotalDecimalNumberStyle.DataFormat = dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);
            _cell.CellValue = report.Total.ProductName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, grandTotalTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.Total.WinLoss;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, grandTotalDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.Total.MonthToDateWinLoss;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, grandTotalDecimalNumberStyle);
            _cell.ResetColumnIndex();
        }

        private void ExportSportbookTotal(WinLossByProducts report)
        {
            XSSFCellStyle totalTextStyle = _customCellStyleService.CreateLeftTextStyle(_subTotalColor, _fontService.BlackBoldFont);
            XSSFCellStyle totalDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(_subTotalColor, _fontService.BlackBoldFont);
            _cell.CellValue = report.SportbookTotal.ProductName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, totalTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.SportbookTotal.WinLoss;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, totalDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.SportbookTotal.MonthToDateWinLoss;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, totalDecimalNumberStyle);
            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex();
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _subTotalColor = Color.FromArgb(244, 244, 190);
            _grandTotalColor = Color.FromArgb(219, 225, 187);
            _grandTotalBorderColor = Color.FromArgb(255, 102, 0);
            _racingDueToPartnersColor = Color.FromArgb(192, 192, 192);
        }

        private void CreateSheet()
        {
            string sheetName = Resources.WinLossByProducts_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 220 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 220 * NPOIConstants.FontWidth);
        }

        private XSSFFont CreateRacingDueToPartnerFont()
        {
            var racingDueToPartnerFontInformation = new FontInformation(NPOIConstants.FontName, NPOIConstants.FontSize, _racingDueToPartnersColor);
            var font = _fontService.CreateFont(racingDueToPartnerFontInformation);
            return font;
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private XSSFCellStyle GetThirdPartySportTextStyle(
           bool isRacingDueToPartner,
           XSSFCellStyle normalTextStyle,
           XSSFCellStyle racingDueToPartnersTextStyle)
        {
            XSSFCellStyle textStyle = isRacingDueToPartner ? racingDueToPartnersTextStyle : normalTextStyle;
            return textStyle;
        }

        private XSSFCellStyle GetThirdPartySportDecimalNumberStyle(
            bool isRacingDueToPartner,
            XSSFCellStyle normalDecimalNumberStyle,
            XSSFCellStyle racingDueToPartnersDecimalNumberStyle)
        {
            XSSFCellStyle decimalNumberStyle = isRacingDueToPartner ? racingDueToPartnersDecimalNumberStyle : normalDecimalNumberStyle;
            return decimalNumberStyle;
        }

        private bool IsRacingDueToPartnersSport(WinLossByProducts thirdPartySport)
        {
            return thirdPartySport.GroupName == PartnersGroupName;
        }

        private string CreateReportNameWithExtension(GetWinLossByProductsCriteria criteria)
        {
            string reportName = string.Format(Resources.WinLossByProducts_ReportName, criteria.WinLossDate.ToString("yyyy_MM_dd"));

            return reportName;
        }        
    }
}