using Fanex.NAPReport.TraderPerformance.Business.Model;
using Fanex.NAPReport.TraderPerformance.Business.Resources;
using Fanex.Service.Core.Constants;
using Fanex.Service.Core.Extensions;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Model.NpoiNewFormat;
using Fanex.Service.Core.Service.Implement.Npoi;
using Fanex.Service.Core.Service.Interface.Npoi;
using Fanex.Service.Core.Utilities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public class ExportService
    {
        private Color headerColor;
        private Color winLossGroupColor;
        private Color turnoverGroupColor;
        private Color liveBetCountGroupColor;
        private Color marginGroupColor;
        private Color commissionGroupColor;
        private Color subTotalColor;
        private Color grandTotalColor;
        private Color grandTotalBorderColor;

        private XSSFWorkbook workBook;
        private XSSFSheet workSheet;
        private Cell cell;
        private INpoiNewFormatFontService fontService;
        private INpoiNewFormatBodyCellStyleService bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService customCellStyleService;
        private INpoiAlignmentService alignmentService;
        private XSSFFont regularFont;
        private XSSFDataFormat dataFormat;

        private XSSFCellStyle winLossGroupParenthesesNegativeDecimalStyle;
        private XSSFCellStyle turnOverGroupParenthesesNegativeDecimalStyle;
        private XSSFCellStyle liveBetCountGroupNumberStyle;
        private XSSFCellStyle marginGroupParenthesesNegativePercentStyle;
        private XSSFCellStyle commissionGroupParenthesesNegativeDecimalStyle;

        private XSSFCellStyle headerStyle;
        private XSSFCellStyle subTotalTextStyle;
        private XSSFCellStyle subTotalNumberStyle;
        private XSSFCellStyle subTotalParenthesesNegativeDecimalStyle;
        private XSSFCellStyle subTotalParenthesesNegativePercentStyle;

        private XSSFCellStyle grandTotalTextStyle;
        private XSSFCellStyle grandTotalNumberStyle;
        private XSSFCellStyle grandTotalParenthesesNegativeDecimalStyle;
        private XSSFCellStyle grandTotalParenthesesNegativePercentStyle;

        public void Export(TraderPerformanceModel traderModel, TraderPerformanceDetailModel traderDetailModel, DateTime reportDate)
        {
            workBook = new XSSFWorkbook();
            dataFormat = workBook.CreateDataFormat() as XSSFDataFormat;
            fontService = new NpoiNewFormatFontService(workBook);
            regularFont = fontService.RegularFont;
            bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(workBook, regularFont);
            customCellStyleService = new NpoiNewFormatCustomCellStyleService(workBook);
            alignmentService = new NpoiNewFormatAlignmentService();

            CreateColor();
            CreateHeaderStyle();
            CreateBodyStyle();
            CreateSubTotalStyle();
            CreateGrandTotalStyle();

            ExportTraderSheet(traderModel, reportDate.ToString("MMM dd yyyy"));
            ExportTraderDetailSheet(traderDetailModel, reportDate.ToString("MMM dd yyyy"));

            string parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDirectory);
            string reportName = $"TraderPerformance_on_{ reportDate.ToString("yyyy_MM_dd")}.xlsx";
            SaveFile.SaveExcelFile(parentDirectory, reportName, workBook, AppSettings.ExcelPassword);
        }

        private void ExportTraderSheet(TraderPerformanceModel traderModel, string reportDate)
        {
            string sheetName = "Main Sheet";
            workSheet = workBook.CreateSheet(sheetName) as XSSFSheet;
            InitTraderSheetColumnWidth(workSheet);
            cell = new Cell();
            ExportTraderSheetTitle(reportDate);
            ExportTraderSheetHeader();
            if (traderModel.Traders.Count > 0)
            {
                ExportTraderSheetContent(traderModel.Traders);
                ExportTraderSheetGrandTotal(traderModel.GrandTotal);
            }
            else
            {
                cell.EndColumn = cell.CurrentColumn + 127;
                RenderMergeCell("There is no data.", bodyCellStyleService.LeftTextStyle);
                cell.ResetColumnIndex();
            }
        }

        private void InitTraderSheetColumnWidth(XSSFSheet workSheet)
        {
            for (var i = 0; i < 128; i++)
            {
                workSheet.SetColumnWidth(i, 140 * NPOIConstants.FontWidth);
            }

            workSheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(2, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(3, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(4, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(5, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(6, 60 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(7, 60 * NPOIConstants.FontWidth);
        }

        private void ExportTraderSheetTitle(string reportDate)
        {
            string title = $"Trader Performance on {reportDate}";
            cell.EndColumn = cell.CurrentColumn + 127;
            cell.EndRow = cell.CurrentRow + 1;
            this.RenderMergeCell(title, headerStyle);
            cell.ResetColumnIndex();
            cell.CurrentRow = cell.EndRow + 2;
            cell.EndRow = cell.CurrentRow;
        }

        private void ExportTraderSheetHeader()
        {
            int startBetTypeColumnIndex = 8;
            int numberColumnOfBetType = 24;
            cell.EndRow = cell.CurrentRow + 1;
            RenderMergeAndInCreaseCellIndex(Labels.No, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Trader, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Match, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Major, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.FirstMinor, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.SecondMinor, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.ThirdMinor, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Others, headerStyle);

            cell.EndRow = cell.CurrentRow;
            ExportBetTypeHeader(Labels.Win_Loss, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Turnover, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Live_BetCount, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Margin_Percent, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Commission, startBetTypeColumnIndex);
            cell.CurrentRow = cell.CurrentRow + 1;
            cell.EndRow = cell.CurrentRow;
            cell.ResetColumnIndex();

        }

        private void ExportTraderSheetContent(List<RawTrader> traders)
        {
            for (int i = 0; i < traders.Count; i++)
            {
                var trader = traders[i];
                ExportTraderSheetDataRow(trader, i);
            }
        }

        private void ExportTraderSheetDataRow(RawTrader trader, int rowIndex)
        {
            RenderAndInCreaseCellIndex(rowIndex + 1, bodyCellStyleService.CenterNumberStyle);
            RenderAndInCreaseCellIndex(trader.Name, bodyCellStyleService.CenterTextStyle);
            RenderAndInCreaseCellIndex(trader.MatchNumber, bodyCellStyleService.RightNumberStyle);
            RenderAndInCreaseCellIndex(trader.Major, bodyCellStyleService.RightNumberStyle);
            RenderAndInCreaseCellIndex(trader.Minor1, bodyCellStyleService.RightNumberStyle);
            RenderAndInCreaseCellIndex(trader.Minor2, bodyCellStyleService.RightNumberStyle);
            RenderAndInCreaseCellIndex(trader.Minor3, bodyCellStyleService.RightNumberStyle);
            RenderAndInCreaseCellIndex(trader.Others, bodyCellStyleService.RightNumberStyle);
            ExportBetTypeContent(trader.WinLoss, winLossGroupParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(trader.Turnover,turnOverGroupParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(trader.LiveBetCount, liveBetCountGroupNumberStyle);
            ExportBetTypeContent(trader.Margin, marginGroupParenthesesNegativePercentStyle);
            ExportBetTypeContent(trader.Commission, commissionGroupParenthesesNegativeDecimalStyle);
            cell.ResetColumnIndex();
            cell.IncreaseRowIndex();
        }

        private void ExportTraderSheetGrandTotal(RawTrader grandTotal)
        {
            cell.EndColumn = cell.CurrentColumn + 1;
            RenderMergeCell(Labels.Grand_Total, grandTotalTextStyle);
            cell.IncreaseColumnIndex();

            RenderAndInCreaseCellIndex(grandTotal.MatchNumber, grandTotalNumberStyle);
            RenderAndInCreaseCellIndex(grandTotal.Major, grandTotalNumberStyle);
            RenderAndInCreaseCellIndex(grandTotal.Minor1, grandTotalNumberStyle);
            RenderAndInCreaseCellIndex(grandTotal.Minor2, grandTotalNumberStyle);
            RenderAndInCreaseCellIndex(grandTotal.Minor3, grandTotalNumberStyle);
            RenderAndInCreaseCellIndex(grandTotal.Others, grandTotalNumberStyle);

            ExportBetTypeContent(grandTotal.WinLoss, grandTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(grandTotal.Turnover, grandTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(grandTotal.LiveBetCount, grandTotalNumberStyle);
            ExportBetTypeContent(grandTotal.Margin, grandTotalParenthesesNegativePercentStyle);
            ExportBetTypeContent(grandTotal.Commission, grandTotalParenthesesNegativeDecimalStyle);

            cell.ResetColumnIndex();
        }

        private void ExportTraderDetailSheet(TraderPerformanceDetailModel traderDetailModel, string reportDate)
        {
            string sheetName = "Detail Sheet";
            workSheet = workBook.CreateSheet(sheetName) as XSSFSheet;
            InitTraderDetailSheetColumnWidth(workSheet);
            cell = new Cell();
            ExportTraderSheetTitle(reportDate);
            ExportTraderDetailSheetHeader();
            if (traderDetailModel.TraderDetailByUserNames.Count > 0)
            {
                ExportTraderDetailSheetContent(traderDetailModel.TraderDetailByUserNames);
                ExportTraderDetailSheetGrandTotal(traderDetailModel.GrandTotal);
            }
            else
            {
                cell.EndColumn = cell.CurrentColumn + 127;
                RenderMergeCell("There is no data.", bodyCellStyleService.LeftTextStyle);
                cell.ResetColumnIndex();
            }
        }

        private void InitTraderDetailSheetColumnWidth(XSSFSheet workSheet)
        {
            for (var i = 0; i < 128; i++)
            {
                workSheet.SetColumnWidth(i, 140 * NPOIConstants.FontWidth);
            }

            workSheet.SetColumnWidth(0, 80 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(6, 80 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(7, 80 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(3, 500 * NPOIConstants.FontWidth);
            workSheet.SetColumnWidth(4, 500 * NPOIConstants.FontWidth);
        }

        private void ExportTraderDetailSheetHeader()
        {
            int startBetTypeColumnIndex = 8;
            int numberColumnOfBetType = 24;
            cell.EndRow = cell.CurrentRow + 1;
            RenderMergeAndInCreaseCellIndex(Labels.No, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Trader, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Date, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.League, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Match, headerStyle);
            RenderMergeAndInCreaseCellIndex(Labels.Maxbet, headerStyle);

            cell.EndRow = cell.CurrentRow;
            cell.EndColumn = cell.CurrentColumn + 1;
            RenderMergeCell(Labels.Score, headerStyle);

            cell.IncreaseRowIndex();
            cell.EndColumn = cell.CurrentColumn;
            RenderAndInCreaseCellIndex(Labels.HT, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FT, headerStyle);

            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Win_Loss, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Turnover, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Live_BetCount, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Margin_Percent, startBetTypeColumnIndex);
            startBetTypeColumnIndex += numberColumnOfBetType;
            cell.CurrentRow = cell.CurrentRow - 1;
            cell.EndRow = cell.CurrentRow;

            ExportBetTypeHeader(Labels.Commission, startBetTypeColumnIndex);
            cell.CurrentRow = cell.CurrentRow + 1;
            cell.EndRow = cell.CurrentRow;
            cell.ResetColumnIndex();
        }

        private void ExportTraderDetailSheetContent(List<TraderDetailByUserName> traderByUserNames)
        {
            for (int i = 0; i < traderByUserNames.Count; i++)
            {
                var item = traderByUserNames[i];
                ExportTraderDetailSheetDataRow(item, i);
            }
        }

        private void ExportTraderDetailSheetDataRow(TraderDetailByUserName trader, int rowIndex)
        {
            var numberRows = trader.TraderDetails.Count;
            cell.EndRow = cell.CurrentRow + numberRows - 1;

            RenderMergeAndInCreaseCellIndex(rowIndex + 1, bodyCellStyleService.CenterTextStyle);
            RenderMergeAndInCreaseCellIndex(trader.Name, bodyCellStyleService.CenterTextStyle);

            cell.EndRow = cell.CurrentRow;

            foreach (var item in trader.TraderDetails)
            {
                int eventDateColumnIndex = 2;
                cell.CurrentColumn = eventDateColumnIndex;
                cell.EndColumn = eventDateColumnIndex;
                RenderAndInCreaseCellIndex(item.EventDate, bodyCellStyleService.CenterTextStyle);
                RenderAndInCreaseCellIndex(item.League, bodyCellStyleService.CenterTextStyle);
                RenderAndInCreaseCellIndex(item.Match, bodyCellStyleService.CenterTextStyle);
                RenderCheckCellValueAndIncreareCellIndex(item.MaxBet, bodyCellStyleService.DecimalNumberStyle);
                RenderAndInCreaseCellIndex(item.HaftTimeScore, bodyCellStyleService.CenterTextStyle);
                RenderAndInCreaseCellIndex(item.FullTimeScore, bodyCellStyleService.CenterTextStyle);

                ExportBetTypeContent(item.WinLoss, winLossGroupParenthesesNegativeDecimalStyle);
                ExportBetTypeContent(item.Turnover, turnOverGroupParenthesesNegativeDecimalStyle);
                ExportBetTypeContent(item.LiveBetCount, liveBetCountGroupNumberStyle);
                ExportBetTypeContent(item.Margin, marginGroupParenthesesNegativePercentStyle);
                ExportBetTypeContent(item.Commission, commissionGroupParenthesesNegativeDecimalStyle);
                cell.IncreaseRowIndex();
            }

            cell.ResetColumnIndex();
            ExportExportTraderDetailSheetTotal(trader.Total, trader.Name);
            cell.IncreaseRowIndex();
        }

        private void ExportExportTraderDetailSheetTotal(TraderDetailItem total, string userName)
        {
            cell.EndColumn = cell.CurrentColumn + 5;
            RenderMergeCell(userName, subTotalTextStyle);
            cell.IncreaseColumnIndex();

            RenderAndInCreaseCellIndex(string.Empty, subTotalTextStyle);
            RenderAndInCreaseCellIndex(string.Empty, subTotalTextStyle);

            ExportBetTypeContent(total.WinLoss, subTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(total.Turnover, subTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(total.LiveBetCount, subTotalNumberStyle);
            ExportBetTypeContent(total.Margin, subTotalParenthesesNegativePercentStyle);
            ExportBetTypeContent(total.Commission, grandTotalParenthesesNegativeDecimalStyle);

            cell.ResetColumnIndex();
        }

        private void ExportTraderDetailSheetGrandTotal(TraderDetailItem grandTotal)
        {
            cell.EndColumn = cell.CurrentColumn + 5;
            RenderMergeCell(Labels.Grand_Total, grandTotalTextStyle);
            cell.IncreaseColumnIndex();

            RenderAndInCreaseCellIndex(string.Empty, grandTotalTextStyle);
            RenderAndInCreaseCellIndex(string.Empty, grandTotalTextStyle);

            ExportBetTypeContent(grandTotal.WinLoss, grandTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(grandTotal.Turnover, grandTotalParenthesesNegativeDecimalStyle);
            ExportBetTypeContent(grandTotal.LiveBetCount, grandTotalNumberStyle);
            ExportBetTypeContent(grandTotal.Margin, grandTotalParenthesesNegativePercentStyle);
            ExportBetTypeContent(grandTotal.Commission, grandTotalParenthesesNegativeDecimalStyle);

            cell.ResetColumnIndex();
        }

        private void ExportBetTypeHeader(string groupName, int startColumnIndex)
        {
            cell.CurrentColumn = startColumnIndex;
            cell.EndColumn = cell.CurrentColumn + 23;
            RenderMergeCell(groupName, headerStyle);

            cell.IncreaseRowIndex();
            cell.EndColumn = cell.CurrentColumn;

            RenderAndInCreaseCellIndex(Labels.FirstHalfOneXTwo, headerStyle);
            RenderAndInCreaseCellIndex(Labels.OneXTwo, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FifteenMinOneXTwo, headerStyle);
            RenderAndInCreaseCellIndex(Labels.TenMinOneXTwo, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FirstHalfHdp, headerStyle);
            RenderAndInCreaseCellIndex(Labels.Hdp, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FifteenMinHdp, headerStyle);
            RenderAndInCreaseCellIndex(Labels.TenMinHdp, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FirstHalfOU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.OU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FifteenMinOU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.TenMinOU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.SingleTeamOU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FirstHalfNoOfCorners, headerStyle);
            RenderAndInCreaseCellIndex(Labels.NoOfCorners, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FifteenMinNoOfCorners, headerStyle);
            RenderAndInCreaseCellIndex(Labels.NextCorner, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FirstHalfTotalBookings, headerStyle);
            RenderAndInCreaseCellIndex(Labels.TotalBookings, headerStyle);
            RenderAndInCreaseCellIndex(Labels.TotalBookingsFifteenMinOU, headerStyle);
            RenderAndInCreaseCellIndex(Labels.NextBookings, headerStyle);
            RenderAndInCreaseCellIndex(Labels.FirstHalfOE, headerStyle);
            RenderAndInCreaseCellIndex(Labels.OE, headerStyle);
            RenderAndInCreaseCellIndex(Labels.Overall, headerStyle);
        }

        private void ExportBetTypeContent(TraderBetType bettypeGroup, XSSFCellStyle cellStyle)
        {
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfOneXTwo, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.OneXTwo, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FifteenMinOneXTwo, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.TenMinOneXTwo, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfHdp, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.Hdp, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FifteenMinHdp, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.TenMinHdp, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfOU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.OU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FifteenMinOU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.TenMinOU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.SingleTeamOU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfNoOfCorners, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.NoOfCorners, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FifteenMinNoOfCorners, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.NextCorner, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfTotalBookings, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.TotalBookings, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.TotalBookingsFifteenMinOU, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.NextBookings, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.FirstHalfOE, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.OE, cellStyle);
            RenderCheckCellValueAndIncreareCellIndex(bettypeGroup.Overall, cellStyle);
        }

        private void CreateColor()
        {
            headerColor = Color.FromArgb(60, 94, 165);
            winLossGroupColor = Color.FromArgb(240, 245, 255);
            turnoverGroupColor = Color.FromArgb(233, 250, 239);
            liveBetCountGroupColor = Color.FromArgb(248, 248, 210);
            marginGroupColor = Color.FromArgb(254, 241, 236);
            commissionGroupColor = Color.FromArgb(246, 246, 239);
            subTotalColor = Color.FromArgb(244, 244, 190);
            grandTotalColor = Color.FromArgb(219, 225, 187);
            grandTotalBorderColor = Color.FromArgb(255, 102, 0);
        }

        private void CreateHeaderStyle()
        {
            headerStyle = customCellStyleService.CreateCenterTextStyle(headerColor, fontService.WhiteBoldFont);
        }

        private void CreateBodyStyle()
        {
            winLossGroupParenthesesNegativeDecimalStyle = customCellStyleService.CreateParenthesesNegativeDecimalNumberStyle(winLossGroupColor, fontService.RegularFont);
            turnOverGroupParenthesesNegativeDecimalStyle = customCellStyleService.CreateParenthesesNegativeDecimalNumberStyle(turnoverGroupColor, fontService.RegularFont);
            liveBetCountGroupNumberStyle = customCellStyleService.CreateRightNumberStyle(liveBetCountGroupColor, fontService.RegularFont);
            marginGroupParenthesesNegativePercentStyle = customCellStyleService.CreateParenthesesNegativePercentageStyle(marginGroupColor, fontService.RegularFont);
            commissionGroupParenthesesNegativeDecimalStyle = customCellStyleService.CreateParenthesesNegativeDecimalNumberStyle(commissionGroupColor, fontService.RegularFont);
        }

        private void CreateGrandTotalStyle()
        {
            var borderInformation = new BorderInformation(
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Medium,
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 Color.Gray,
                 grandTotalBorderColor,
                 Color.Gray,
                 Color.Gray);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, grandTotalColor);

            grandTotalTextStyle = customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                alignmentService.RightAlignment,
                fontService.BlackBoldFont);

            grandTotalNumberStyle = customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                alignmentService.RightAlignment,
                fontService.BlackBoldFont);
            grandTotalNumberStyle.DataFormat = dataFormat.GetFormat(NPOIConstants.NumberFormat);

            grandTotalParenthesesNegativeDecimalStyle = customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                alignmentService.RightAlignment,
                fontService.BlackBoldFont);
            grandTotalParenthesesNegativeDecimalStyle.DataFormat = dataFormat.GetFormat(NPOIConstants.ParenthesesNegativeDecimalNumberFormat);

            grandTotalParenthesesNegativePercentStyle = customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                alignmentService.RightAlignment,
                fontService.BlackBoldFont);
            grandTotalParenthesesNegativePercentStyle.DataFormat = dataFormat.GetFormat(NPOIConstants.ParenthesesNegativePercentFormat);
        }

        private void CreateSubTotalStyle()
        {
            subTotalTextStyle = customCellStyleService.CreateRightTextStyle(subTotalColor, fontService.BlackBoldFont);
            subTotalNumberStyle = customCellStyleService.CreateRightNumberStyle(subTotalColor, fontService.BlackBoldFont);
            subTotalParenthesesNegativeDecimalStyle = customCellStyleService.CreateParenthesesNegativeDecimalNumberStyle(subTotalColor, fontService.BlackBoldFont);
            subTotalParenthesesNegativePercentStyle = customCellStyleService.CreateParenthesesNegativePercentageStyle(subTotalColor, fontService.BlackBoldFont);
        }

        private void RenderCell(object cellValue, XSSFCellStyle cellStyle)
        {
            cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(workSheet, cell, cellStyle);
        }

        private void RenderMergeCell(object cellValue, XSSFCellStyle cellStyle)
        {
            cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderMergeCell(workSheet, cell, cellStyle);
        }

        private void RenderMergeAndInCreaseCellIndex(object cellValue, XSSFCellStyle cellStyle)
        {
            RenderMergeCell(cellValue, cellStyle);
            cell.IncreaseColumnIndex();
        }

        private void RenderAndInCreaseCellIndex(object cellValue, XSSFCellStyle cellStyle)
        {
            RenderCell(cellValue, cellStyle);
            cell.IncreaseColumnIndex();
        }

        private void RenderCheckCellValueAndIncreareCellIndex(object cellValue, XSSFCellStyle cellStyle)
        {
            var value = GetCellValue(cellValue);
            RenderCell(value, cellStyle);
            cell.IncreaseColumnIndex();
        }

        private object GetCellValue(object cellValue)
        {
            object value = string.Empty;
            if (cellValue is int || cellValue is decimal)
            {
                if (Convert.ToDecimal(cellValue) == 0)
                {
                    cellValue = string.Empty;
                }
            }

            value = cellValue;
            return value;
        }
    }
}
