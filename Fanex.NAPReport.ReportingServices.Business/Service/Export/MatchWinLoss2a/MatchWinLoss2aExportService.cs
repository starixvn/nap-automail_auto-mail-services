// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.MatchWinLoss2a
{
    using System;
    using System.Drawing;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;
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
    public class MatchWinLoss2aExportService : IMatchWinLoss2aExportService
    {
        private Color _headerColor;
        private Color _bettypeTotalColor;
        private Color _subTotalColor;
        private Color _grandTotalColor;
        private Color _grandTotalBorderColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;
        private XSSFFont _regularFont;
        private XSSFDataFormat _dataFormat;

        private XSSFCellStyle _bettypeTotalTextStyle;
        private XSSFCellStyle _bettypeTotalDecimalStyle;
        private XSSFCellStyle _bettypeTotalUnsignedNegativeDecimalStyle;
        private XSSFCellStyle _sportTotalTextStyle;
        private XSSFCellStyle _sportTotalDecimalStyle;
        private XSSFCellStyle _sportTotalUnsignedNegativeDecimalStyle;
        private XSSFCellStyle _grandTotalTextStyle;
        private XSSFCellStyle _grandTotalDecimalStyle;
        private XSSFCellStyle _grandTotalUnsignedNegativeDecimalStyle;
        private XSSFCellStyle _boldParenthesesUnsignedNegativeDecimalStyle;
        private XSSFCellStyle _boldTextStyle;

        public MatchWinLoss2aExportService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);            
            _alignmentService = new NpoiNewFormatAlignmentService();
        }

        public void ExportReport(BettypesAndSportsAndThirdPartyProductsWinLoss fullReport, DateTime winlossDate)
        {
            try
            {
                this.CreateSheet();
                this.InitColumnWidth();
                this.CreateBoldTextStyle();
                this.CreateBoldUnsignedNegativeDecimalStyle();
                this.CreateBettypeTotalStyle();
                this.CreateSportTotalStyle();
                this.CreateGrandTotalStyle();
                this.ExportReportTitle(winlossDate);
                this.ExportSummaryBettypesReport(fullReport.SummaryBettypesWinLoss);
                this.ExportSportsAndThirdPartyProductsWinLossReport(fullReport.SportsAndThirdPartyProductsWinLoss);
                string parentDirectory = AppSettings.DataDirectory(AppSettings.ExportDataDirectoryMatchWinLoss2a);
                string reportName = string.Format(
                    ReportingServicesResource.MatchWinLoss2aAll_ReportName,
                    winlossDate.ToShortFormatedDateWithSlash().Replace('/', '_'));
                string reportNameWithExtension = string.Format("{0}.xlsx", reportName);
                FileName = Path.Combine(parentDirectory, reportNameWithExtension);
                SaveFile.SaveExcelFile(parentDirectory, reportNameWithExtension, _workbook, AppSettings.ExcelPassword);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
            _bettypeTotalColor = Color.FromArgb(194, 217, 236);
            _subTotalColor = Color.FromArgb(244, 244, 190);
            _grandTotalColor = Color.FromArgb(219, 225, 187);
            _grandTotalBorderColor = Color.FromArgb(255, 102, 0);
        }

        private void CreateSheet()
        {
            string sheetName = ReportingServicesResource.MatchWinLoss2a_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            for (var i = 0; i < 14; i++)
            {
                _workSheet.SetColumnWidth(i, 140 * NPOIConstants.FontWidth);
            }

            _workSheet.SetColumnWidth(0, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 200 * NPOIConstants.FontWidth);
        }

        private void ExportReportTitle(DateTime currentTime)
        {
            var headerStyle = this.CreateHeaderStyle();
            _cell.CellValue = string.Format(
                    ReportingServicesResource.MatchWinLoss2aAll_ReportTitle,
                    currentTime.AddDays(-1).ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 13;
            _cell.EndRow = _cell.CurrentRow + 1;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow = _cell.EndRow + 2;
            _cell.EndRow = _cell.CurrentRow;
        }

        private void ExportSummaryBettypesReport(SummaryBettypesWinLoss summaryBettypesWinLoss)
        {
            this.ExportSummaryBettypesHeader();
            this.ExportSummaryBettypesContent(summaryBettypesWinLoss);
            this.ExportSummaryBettypesGrandTotal(summaryBettypesWinLoss);
        }

        private void ExportSportsAndThirdPartyProductsWinLossReport(SportsAndThirdPartyProductsWinLoss sportsAndThirdPartyProductsWinLoss)
        {
            this.ExportSportsAndThirdPartyProductsHeader();
            this.ExportSportsWinLoss(sportsAndThirdPartyProductsWinLoss.SportsWinLoss);
            this.ExportSportsGrandTotal(sportsAndThirdPartyProductsWinLoss.SportsWinLoss.GrandTotal);
            this.ExportRMTotalRow(sportsAndThirdPartyProductsWinLoss.SportsWinLoss.EightRMTotal, "Total 8RM");
            this.ExportRMTotalRow(sportsAndThirdPartyProductsWinLoss.SportsWinLoss.BARMTotal, "Total BARM");
            this.ExportThirdPartyProductsWinLoss(sportsAndThirdPartyProductsWinLoss.ThirdPartyProductsWinLoss);
            this.ExportGrandTotal(sportsAndThirdPartyProductsWinLoss.GrandTotal);
        }

        private void ExportSummaryBettypesHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            _cell.CellValue = Labels.Bet_Type;
            _cell.EndRow = _cell.CurrentRow + 1;
            this.RenderMergeCell(_workSheet, headerStyle);
            _cell.EndRow = _cell.CurrentRow;
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 1;
            this.RenderMergeCell(Labels.Soccer_Live, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 1;
            this.RenderMergeCell(Labels.Soccer_Non_Live, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 1;
            this.RenderMergeCell(Labels.Other_Live, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 1;
            this.RenderMergeCell(Labels.Other_Non_Live, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 1;
            this.RenderMergeCell(Labels.Line_Total, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndRow = _cell.CurrentRow + 1;
            this.RenderMergeCell(Labels.Variance, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderMergeCell(Labels.Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderMergeCell(Labels.Margin_Percent, headerStyle);
            _cell.CurrentColumn = 1;
            _cell.EndColumn = 1;
            _cell.CurrentRow += 1;
            this.RenderCell(Labels.Real, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Real, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Real, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Real, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Real, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportSummaryBettypesContent(SummaryBettypesWinLoss summaryBettypesWinLoss)
        {
            foreach (var bettypeWinLoss in summaryBettypesWinLoss.ReportRows)
            {
                if (bettypeWinLoss.IsExcludeVirtualLineTotal)
                {
                    this.ExportExcludeVirtualLineTotalBettypeRow(bettypeWinLoss);
                }
                else
                {
                    this.ExportBettypeRow(bettypeWinLoss);
                }

                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportSummaryBettypesGrandTotal(SummaryBettypesWinLoss summaryBettypesWinLoss)
        {
            var grandTotal = summaryBettypesWinLoss.GrandTotal;
            this.RenderCell(Labels.Grand_Total, _grandTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal_Soccer_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal_Soccer_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal_Soccer_Non_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal_Soccer_Non_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal_Others_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal_Others_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal_Others_Non_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal_Others_Non_Live, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotalReal, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotalVirtual, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Variance, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Turnover, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Margin, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow += 2;
            _cell.EndRow += 2;
        }

        private void ExportExcludeVirtualLineTotalBettypeRow(SummaryBettypesWinLoss bettypeRow)
        {
            string bettypeTitle = string.Format(
                "{0}{1}{2}",
                bettypeRow.BettypeName,
                string.IsNullOrEmpty(bettypeRow.Name_Suffix) ? string.Empty : string.Format("({0})", bettypeRow.Name_Suffix),
                bettypeRow.Hedge);
            this.RenderCell(bettypeTitle, _bodyCellStyleService.CenterTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Soccer_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Soccer_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Others_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Others_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotalReal, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.Turnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.Margin == 0 ? (object)Labels.NotAvailable : bettypeRow.Margin, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
        }

        private void ExportBettypeRow(SummaryBettypesWinLoss bettypeRow)
        {
            string bettypeTitle = string.Format(
                "{0}{1}{2}",
                bettypeRow.BettypeName,
                string.IsNullOrEmpty(bettypeRow.Name_Suffix) ? string.Empty : string.Format("({0})", bettypeRow.Name_Suffix),
                bettypeRow.Hedge);
            this.RenderCell(bettypeTitle, _bodyCellStyleService.CenterTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Soccer_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.VirtualLineTotal_Soccer_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Soccer_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.VirtualLineTotal_Soccer_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Others_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.VirtualLineTotal_Others_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotal_Others_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.VirtualLineTotal_Others_Non_Live, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotalReal, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.LineTotalVirtual, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.Variance, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.Turnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettypeRow.Margin == 0 ? (object)Labels.NotAvailable : bettypeRow.Margin, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
        }

        private void ExportSportsAndThirdPartyProductsHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            this.RenderCell(Labels.Bet_Type, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Date, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 2;
            this.RenderMergeCell(Labels.Match, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Win_Loss, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Position, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Comm, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual_Comm, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Line_Total, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Virtual_Line_Total, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Variance, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Turnover, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Margin_Percent, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportSportsWinLoss(SportsWinLoss sportsWinLoss)
        {
            foreach (var sport in sportsWinLoss.WinLossBySports)
            {
                foreach (var bettype in sport.BettypesWinLossBySport)
                {
                    foreach (var match in bettype.MatchWinLoss)
                    {
                        this.ExportMatchRow(match);
                        _cell.IncreaseRowIndex();
                        _cell.ResetColumnIndex();
                    }

                    this.ExportBettypeTotalRow(bettype);
                    _cell.IncreaseRowIndex();
                    _cell.ResetColumnIndex();
                }

                this.ExportSportTotalRow(sport);
                _cell.IncreaseRowIndex();
                _cell.ResetColumnIndex();
            }
        }

        private void ExportSportsGrandTotal(MatchWinLoss grandTotal)
        {
            _cell.EndColumn = _cell.CurrentColumn + 6;
            this.RenderMergeCell(Labels.Grand_Total, _grandTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Commission, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualCommission, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Variance, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Turnover, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Margin, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportRMTotalRow(MatchWinLoss totalRMRow, string title)
        {
            _cell.EndColumn = _cell.CurrentColumn + 8;
            this.RenderMergeCell(title, _sportTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(totalRMRow.LineTotal, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(totalRMRow.VirtualLineTotal, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(totalRMRow.Variance, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(totalRMRow.Turnover, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(totalRMRow.Margin, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportMatchRow(MatchWinLoss match)
        {
            string bettypeByLiveIndicatorAndMatchCode = string.Format(
                            "{0}{1} {2}",
                            match.LiveIndicator ? "!Live - " : string.Empty,
                            match.BettypeName,
                            match.MatchCode);
            this.RenderCell(bettypeByLiveIndicatorAndMatchCode, _bodyCellStyleService.LeftTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.EventDate, _bodyCellStyleService.LeftTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 2;
            string matchInformation = string.Format("{0}-vs-{1}", match.HomeTeamName, match.AwayTeamName);
            this.RenderMergeCell(matchInformation, _bodyCellStyleService.CenterTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.WinLoss, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.Position, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.Commission, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.VirtualCommission, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.LineTotal, _boldParenthesesUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.VirtualLineTotal, _boldParenthesesUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.Variance, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.Turnover, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(match.Margin, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
        }

        private void ExportBettypeTotalRow(BettypeWinLoss bettype)
        {
            _cell.EndColumn = _cell.CurrentColumn + 6;
            string totalTitle = string.Format(
                "{0}{1} TOTAL",
                bettype.LiveIndicator ? "!LIVE - " : string.Empty,
                bettype.BettypeName);
            this.RenderMergeCell(totalTitle, _bettypeTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.Commission, _bettypeTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.VirtualCommission, _bettypeTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.LineTotal, _bettypeTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.VirtualLineTotal, _bettypeTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.Variance, _bettypeTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.Turnover, _bettypeTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(bettype.BettypesWinLossTotal.Margin, _bettypeTotalUnsignedNegativeDecimalStyle);
        }

        private void ExportSportTotalRow(WinLossBySport sport)
        {
            _cell.EndColumn = _cell.CurrentColumn + 6;
            string totalTitle = string.Format(
                "{0} TOTAL",
                sport.SportName);
            this.RenderMergeCell(totalTitle, _sportTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.Commission, _sportTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.VirtualCommission, _sportTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.LineTotal, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.VirtualLineTotal, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.Variance, _sportTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.Turnover, _sportTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(sport.SportWinLossTotal.Margin, _sportTotalUnsignedNegativeDecimalStyle);
        }

        private void ExportThirdPartyProductsWinLoss(SummarySportsWinLoss thirdPartyProducts)
        {
            foreach (var product in thirdPartyProducts.SummaryWinLossBySports)
            {
                this.ExportThirdPartyProductRow(product);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void ExportGrandTotal(SummarySportsWinLoss grandTotal)
        {
            _cell.EndColumn = _cell.CurrentColumn + 6;
            this.RenderMergeCell(Labels.Grand_Total, _grandTotalTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Commission, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualCommission, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.LineTotal, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.VirtualLineTotal, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Variance, _grandTotalUnsignedNegativeDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.IBCTurnover, _grandTotalDecimalStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(grandTotal.Margin, _grandTotalUnsignedNegativeDecimalStyle);
        }

        private void ExportThirdPartyProductRow(SummarySportsWinLoss product)
        {
            this.RenderCell(product.SportName, _bodyCellStyleService.CenterTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(product.WinLossDate, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            _cell.EndColumn = _cell.CurrentColumn + 2;
            this.RenderMergeCell(string.Empty, _bodyCellStyleService.RightTextStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(product.WinLoss, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(product.Position, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(product.Commission, _bodyCellStyleService.DecimalNumberStyle);
            _cell.IncreaseColumnIndex();
            if (product.IsExcludeVirtualLineTotal)
            {
                this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            }
            else
            {
                this.RenderCell(product.VirtualCommission, _bodyCellStyleService.DecimalNumberStyle);
            }

            _cell.IncreaseColumnIndex();
            this.RenderCell(product.LineTotal, _boldParenthesesUnsignedNegativeDecimalStyle);

            _cell.IncreaseColumnIndex();
            if (product.IsExcludeVirtualLineTotal)
            {
                this.RenderCell(Labels.NotAvailable, _boldTextStyle);
            }
            else
            {
                this.RenderCell(product.VirtualLineTotal, _boldParenthesesUnsignedNegativeDecimalStyle);
            }

            _cell.IncreaseColumnIndex();
            if (product.IsExcludeVirtualLineTotal)
            {
                this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            }
            else
            {
                this.RenderCell(product.Variance, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            }

            _cell.IncreaseColumnIndex();
            this.RenderCell(product.IBCTurnover, _bodyCellStyleService.DecimalNumberStyle);

            _cell.IncreaseColumnIndex();
            if (product.IsExcludeVirtualLineTotal)
            {
                this.RenderCell(Labels.NotAvailable, _bodyCellStyleService.RightTextStyle);
            }
            else
            {
                this.RenderCell(product.Margin, _bodyCellStyleService.ParenthesesUnsignedNegativeDecimalNumberStyle);
            }
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

        private void RenderMergeCell(object cellValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, cellStyle);
        }

        private void CreateBoldTextStyle()
        {
            var borderInformation = new BorderInformation(
               (short)BorderStyle.Thin,
               (short)BorderStyle.Thin,
               (short)BorderStyle.Thin,
               (short)BorderStyle.Thin,
               Color.Gray,
               Color.Gray,
               Color.Gray,
               Color.Gray);
            _boldTextStyle = _bodyCellStyleService.CreateCellStyle(
                _workbook,
                borderInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
        }

        private void CreateBoldUnsignedNegativeDecimalStyle()
        {
            var borderInformation = new BorderInformation(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                Color.Gray,
                Color.Gray,
                Color.Gray);
            _boldParenthesesUnsignedNegativeDecimalStyle = _bodyCellStyleService.CreateCellStyle(
                _workbook,
                borderInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _boldParenthesesUnsignedNegativeDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesUnsignedNegativeDecimalNumberFormat);
        }

        private void CreateBettypeTotalStyle()
        {
            var borderInformation = new BorderInformation(
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 Color.Gray,
                 Color.Gray,
                 Color.Gray,
                 Color.Gray);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, _bettypeTotalColor);

            _bettypeTotalTextStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);

            _bettypeTotalDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _bettypeTotalDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _bettypeTotalUnsignedNegativeDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _bettypeTotalUnsignedNegativeDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesUnsignedNegativeDecimalNumberFormat);
        }

        private void CreateSportTotalStyle()
        {
            var borderInformation = new BorderInformation(
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 (short)BorderStyle.Thin,
                 Color.Gray,
                 Color.Gray,
                 Color.Gray,
                 Color.Gray);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, _subTotalColor);

            _sportTotalTextStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);

            _sportTotalDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _sportTotalDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _sportTotalUnsignedNegativeDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _sportTotalUnsignedNegativeDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesUnsignedNegativeDecimalNumberFormat);
        }

        private void CreateGrandTotalStyle()
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

            _grandTotalTextStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);

            _grandTotalDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _grandTotalDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _grandTotalUnsignedNegativeDecimalStyle = _customCellStyleService.CreateCellStyle(
                borderInformation,
                backgroundInformation,
                _alignmentService.RightAlignment,
                _fontService.BlackBoldFont);
            _grandTotalUnsignedNegativeDecimalStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesUnsignedNegativeDecimalNumberFormat);
        }


        public string FileName
        {
            get;
            set;
        }
    }
}