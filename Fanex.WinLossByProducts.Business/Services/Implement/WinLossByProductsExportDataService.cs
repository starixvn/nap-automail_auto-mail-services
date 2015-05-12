// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsExportDataService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.WinLossByProducts.Business.Services.Implement
{
    using System;
    using System.IO;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.WinLossByProducts.Business.Criteria;
    using Fanex.WinLossByProducts.Business.Model;
    using Fanex.WinLossByProducts.Business.Resources;
    using Fanex.WinLossByProducts.Business.Services.Interface;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsExportDataService : IWinLossByProductsExportDataService
    {
        private const short HeaderColorIndex = 45;
        private const short SubTotalColorIndex = 46;
        private const short GrandTotalColorIndex = 47;
        private const short GrandTotalBorderColorIndex = 48;
        private const short RacingDueToPartnersColorIndex = 49;
        private const string PartnersGroupName = "partners";

        private HSSFWorkbook _workbook;
        private ISheet _workSheet;
        private Cell _cell;
        private INpoiOldFormatFontService _fontService;
        private INpoiOldFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiOldFormatCustomCellStyleService _customCellStyleService;
        private IFont _regularFont;

        public WinLossByProductsExportDataService()
        {
            _cell = new Cell();
            _workbook = new HSSFWorkbook();
            this.SetColorIndex();
            _fontService = new NpoiOldFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiOldFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiOldFormatCustomCellStyleService(_workbook);
        }

        public string ExportReportToExcel(WinLossByProducts report, GetWinLossByProductsCriteria criteria)
        {
            FileStream fileStream = null;
            CreateSheet();
            InitColumnWidth();
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

            var reportDirectory = this.CreateWinLossByProductsReportDirectory(criteria);
            string reportName = string.Format(
                Resources.WinLossByProducts_ReportName,
                criteria.WinLossDate.ToShortFormatedDateWithSlash().Replace('/', '_'));
            var reportPath = Path.Combine(reportDirectory, reportName);
            var fullReportPath = string.Format("{0}.{1}", reportPath, "xls");

            fileStream = new FileStream(
                    fullReportPath,
                    FileMode.Create,
                    FileAccess.Write);
            _workbook.Write(fileStream);
            fileStream.Close();
            fileStream.Dispose();
            return fullReportPath;
        }

        private void ExportReportHeader()
        {
            IRow currentRow = _workSheet.CreateRow(_cell.CurrentRow);
            ICellStyle headerStyle = CreateHeaderStyle();
            string reportTile = string.Format(Resources.WinLossByProducts_ReportTitle, DateTime.Now.AddDays(-1).ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 2;
            _cell.CellValue = reportTile;
            NpoiOldFormatExcelExtension.RenderMergeCell(_workSheet, currentRow, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow += 2;
            _cell.EndRow = _cell.CurrentRow;
            currentRow = _workSheet.CreateRow(_cell.CurrentRow);
            _cell.CellValue = Labels.Product;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Win_Loss;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.MonthtoDate;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportSportBookContent(WinLossByProducts report)
        {
            ICellStyle textStyle = _bodyCellStyleService.LeftTextStyle;
            ICellStyle decimalNumberStyle = _bodyCellStyleService.DecimalNumberStyle;
            IRow currentRow = null;
            foreach (var sportbook in report.Sportbook)
            {
                currentRow = _workSheet.CreateRow(_cell.CurrentRow);
                _cell.CellValue = sportbook.ProductName;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.WinLoss;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.MonthToDateWinLoss;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, decimalNumberStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportThirdPartySport(WinLossByProducts report)
        {
            ICellStyle textStyle = _bodyCellStyleService.LeftTextStyle;
            ICellStyle decimalNumberStyle = _bodyCellStyleService.DecimalNumberStyle;
            IFont racingDueToPartnerFont = CreateRacingDueToPartnerFont();
            ICellStyle racingDueToPartnersTextStyle = _customCellStyleService.CreateLeftTextStyle(HSSFColor.White.Index, racingDueToPartnerFont);
            ICellStyle racingDueToPartnersDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(HSSFColor.White.Index, racingDueToPartnerFont);
            IRow currentRow = null;
            ICellStyle thirdPartySportTextStyle = null;
            ICellStyle thirdPartySportDecimalNumberStyle = null;

            foreach (var thirdPartySport in report.ThirdPartySport)
            {
                bool isRacingDueToPartners = IsRacingDueToPartnersSport(thirdPartySport);
                thirdPartySportTextStyle = GetThirdPartySportTextStyle(isRacingDueToPartners, textStyle, racingDueToPartnersTextStyle);
                thirdPartySportDecimalNumberStyle = GetThirdPartySportDecimalNumberStyle(
                    isRacingDueToPartners,
                    decimalNumberStyle,
                    racingDueToPartnersDecimalNumberStyle);
                currentRow = _workSheet.CreateRow(_cell.CurrentRow);
                _cell.CellValue = thirdPartySport.ProductName;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, thirdPartySportTextStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = thirdPartySport.WinLoss;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, thirdPartySportDecimalNumberStyle);

                _cell.IncreaseColumnIndex();
                _cell.CellValue = thirdPartySport.MonthToDateWinLoss;
                NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, thirdPartySportDecimalNumberStyle);
                _cell.IncreaseRowIndex();
                _cell.ResetColumnIndex();
            }
        }

        private void ExportReportTotal(WinLossByProducts report)
        {
            ICellStyle grandTotalTextStyle = _customCellStyleService.CreateLeftTextStyle(GrandTotalColorIndex, _fontService.BlackBoldFont);
            grandTotalTextStyle.BorderTop = BorderStyle.Medium;
            grandTotalTextStyle.TopBorderColor = GrandTotalBorderColorIndex;
            ICellStyle grandTotalDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(GrandTotalColorIndex, _fontService.BlackBoldFont);
            grandTotalDecimalNumberStyle.BorderTop = BorderStyle.Medium;
            grandTotalDecimalNumberStyle.TopBorderColor = GrandTotalBorderColorIndex;
            IRow currentRow = _workSheet.CreateRow(_cell.CurrentRow);
            _cell.CellValue = report.Total.ProductName;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, grandTotalTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.Total.WinLoss;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, grandTotalDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.Total.MonthToDateWinLoss;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, grandTotalDecimalNumberStyle);
            _cell.ResetColumnIndex();
        }

        private void ExportSportbookTotal(WinLossByProducts report)
        {
            ICellStyle totalTextStyle = _customCellStyleService.CreateLeftTextStyle(SubTotalColorIndex, _fontService.BlackBoldFont);
            ICellStyle totalDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(SubTotalColorIndex, _fontService.BlackBoldFont);
            IRow currentRow = _workSheet.CreateRow(_cell.CurrentRow);
            _cell.CellValue = report.SportbookTotal.ProductName;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, totalTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.SportbookTotal.WinLoss;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, totalDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = report.SportbookTotal.MonthToDateWinLoss;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, totalDecimalNumberStyle);
            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex();
        }

        private void SetColorIndex()
        {
            HSSFPalette palette = _workbook.GetCustomPalette();
            palette.SetColorAtIndex(HeaderColorIndex, (byte)60, (byte)94, (byte)165);
            palette.SetColorAtIndex(SubTotalColorIndex, (byte)244, (byte)244, (byte)190);
            palette.SetColorAtIndex(GrandTotalColorIndex, (byte)219, (byte)225, (byte)187);
            palette.SetColorAtIndex(GrandTotalBorderColorIndex, (byte)255, (byte)102, (byte)0);
            palette.SetColorAtIndex(RacingDueToPartnersColorIndex, (byte)192, (byte)192, (byte)192);
        }

        private string CreateWinLossByProductsReportDirectory(GetWinLossByProductsCriteria criteria)
        {
            string reportDataDirectory = AppSettings.ExportDataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);
            string directoryByYearAndMonth = string.Format(
                "{0}{1}",
                criteria.WinLossDate.ToString("yyyy"),
                criteria.WinLossDate.ToString("MM"));
            string directoryByYearAndMonthPath = Path.Combine(reportDataDirectory, directoryByYearAndMonth);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByYearAndMonthPath);
            string directoryByReport = "WinLossByProducts";
            string directoryByReportPath = Path.Combine(directoryByYearAndMonthPath, directoryByReport);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByReportPath);
            return directoryByReportPath;
        }

        private void CreateSheet()
        {
            string sheetName = Resources.WinLossByProducts_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName);
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 220 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 220 * NPOIConstants.FontWidth);
        }

        private IFont CreateRacingDueToPartnerFont()
        {
            var racingDueToPartnerFontInformation = new FontInformation(NPOIConstants.FontName, NPOIConstants.FontSize, RacingDueToPartnersColorIndex);
            var font = _fontService.CreateFont(_workbook, racingDueToPartnerFontInformation);
            return font;
        }

        private ICellStyle CreateHeaderStyle()
        {
            ICellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(HeaderColorIndex, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private ICellStyle GetThirdPartySportTextStyle(
            bool isRacingDueToPartner,
            ICellStyle normalTextStyle,
            ICellStyle racingDueToPartnersTextStyle)
        {
            ICellStyle textStyle = isRacingDueToPartner ? racingDueToPartnersTextStyle : normalTextStyle;
            return textStyle;
        }

        private ICellStyle GetThirdPartySportDecimalNumberStyle(
            bool isRacingDueToPartner,
            ICellStyle normalDecimalNumberStyle,
            ICellStyle racingDueToPartnersDecimalNumberStyle)
        {
            ICellStyle decimalNumberStyle = isRacingDueToPartner ? racingDueToPartnersDecimalNumberStyle : normalDecimalNumberStyle;
            return decimalNumberStyle;
        }

        private bool IsRacingDueToPartnersSport(WinLossByProducts thirdPartySport)
        {
            return thirdPartySport.GroupName == PartnersGroupName;
        }
    }
}