// <copyright file="ExportExcelWinLossSummaryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Export excel for virtual sport's win loss.
    /// </summary>
    public class ExportExcelWinLossSummaryService : ExportExcelBase
    {
        private HSSFWorkbook _workbook;
        private ICellStyle _summaryStyleBase;
        private int _startRowNumber = 4;

        private short _intergerFormat;
        private short _decimalFormat;
        private short _pecentFormat;

        public ExportExcelWinLossSummaryService()
        {
        }

        /// <summary>
        /// Exports the excel win loss summary.
        /// </summary>
        /// <param name="workbook">The work book.</param>
        public void ExportExcelWinlossSummary(ref HSSFWorkbook workbook)
        {
            var service = new WinLossSummaryService();
            var model = service.GetWinLossSummary();
            _workbook = workbook;
            this.SetColorAtIndex(_workbook);
            this._summaryStyleBase = this.CreateStyle(ref _workbook, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderColorIndex);

            this._intergerFormat = _workbook.CreateDataFormat().GetFormat("#,##0;[RED]-#,##0");
            this._decimalFormat = _workbook.CreateDataFormat().GetFormat("#,##0.00;[RED]-#,##0.00;\"NA\"");
            this._pecentFormat = _workbook.CreateDataFormat().GetFormat("#,##0.00%;[RED]-#,##0.00%;\"NA\"");
            var worksheet = _workbook.CreateSheet(WinLossVirtualSportResoure.Summary);

            if (model.WinlostTotalList.Count > 0)
            {
                RenderTitle(ref worksheet);
                RenderHeader(ref worksheet);
                RenderBody(ref worksheet, model.WinlostTotalList);
                RenderFooter(ref worksheet, model.GrandTotal);
            }
            else
            {
                var nodataRow = worksheet.CreateRow(0);
                var yesterday = DateTime.Now.AddDays(-1);
                DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                string nodataTitle = string.Format(
                    WinLossVirtualSportResoure.NoData,
                    startMonth.ToShortFormatedDate(),
                    yesterday.ToShortFormatedDate());
                nodataRow.CreateCell(0).SetCellValue(nodataTitle);
            }
        }

        /// <summary>
        /// Renders the title.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        internal void RenderTitle(ref ISheet worksheet)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 11, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            IRow row = null;
            ICell cell = null;
            var titleStyle = _workbook.CreateCellStyle();
            titleStyle.CloneStyleFrom(this._summaryStyleBase);
            titleStyle.FillForegroundColor = TitleColorIndex;
            titleStyle.SetFont(font);
            titleStyle.VerticalAlignment = VerticalAlignment.Justify;

            var yesterday = DateTime.Now.AddDays(-1);
            DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);

            string periodTimeTitle = string.Format(
                WinLossVirtualSportResoure.SummaryTitleThird,
                startMonth.ToShortFormatedDate(),
                yesterday.ToShortFormatedDate());

            string[] lines =
            {
              WinLossVirtualSportResoure.SummaryTitleFirst,
              WinLossVirtualSportResoure.SummaryTitleSecond,
              periodTimeTitle
            };

            string title = string.Join(Environment.NewLine, lines);
            for (int i = 0; i < 4; i++)
            {
                row = worksheet.CreateRow(i);
                for (int j = 0; j < 7; j++)
                {
                    cell = row.CreateCell(j);
                    this.RenderCell(ref cell, titleStyle, string.Empty);
                }
            }

            cell = worksheet.GetRow(0).GetCell(0);
            this.RenderCell(ref cell, titleStyle, title);
            worksheet.AddMergedRegion(new CellRangeAddress(0, 3, 0, 6));
        }

        /// <summary>
        /// Renders the header.
        /// </summary>
        /// <param name="worksheet">The work sheet.</param>
        internal void RenderHeader(ref ISheet worksheet)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.White.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var headerStyle = _workbook.CreateCellStyle();
            headerStyle.CloneStyleFrom(this._summaryStyleBase);
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Top;
            headerStyle.FillForegroundColor = HeaderColorIndex;
            headerStyle.SetFont(font);

            var headerRow = worksheet.CreateRow(_startRowNumber);
            headerRow.Sheet.SetColumnWidth(0, 25 * 256);
            headerRow.Sheet.SetColumnWidth(1, 20 * 256);
            headerRow.Sheet.SetColumnWidth(2, 25 * 256);
            headerRow.Sheet.SetColumnWidth(3, 15 * 256);
            headerRow.Sheet.SetColumnWidth(4, 25 * 256);
            headerRow.Sheet.SetColumnWidth(5, 15 * 256);
            headerRow.Sheet.SetColumnWidth(6, 15 * 256);
            headerRow.HeightInPoints = 32;
            var cell = headerRow.CreateCell(0);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.Sport);

            cell = headerRow.CreateCell(1);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.BetType);

            cell = headerRow.CreateCell(2);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.SumBetCount);

            cell = headerRow.CreateCell(3);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.TurnoverIBC);

            cell = headerRow.CreateCell(4);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.AvarageValuePerBet);

            cell = headerRow.CreateCell(5);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.WinLossIBC);

            cell = headerRow.CreateCell(6);
            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.Margin);
        }

        /// <summary>
        /// Renders the win loss by bet type.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        /// <param name="winlossByBettypeObj">The win loss by bet type object.</param>
        /// <param name="isOddRow">If set to <c>true</c> [is odd row].</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        internal void RenderWinLossByBettype(ref IRow newRow, WinLossSummaryBySportType winlossByBettypeObj, bool isOddRow)
        {
            var centerStyle = _workbook.CreateCellStyle();
            centerStyle.CloneStyleFrom(this._summaryStyleBase);
            centerStyle.Alignment = HorizontalAlignment.Center;
            centerStyle.VerticalAlignment = VerticalAlignment.Top;
            centerStyle.FillForegroundColor = isOddRow ? HSSFColor.White.Index : EvenRowColorIndex;

            var rightStyle = _workbook.CreateCellStyle();
            rightStyle.CloneStyleFrom(centerStyle);
            rightStyle.Alignment = HorizontalAlignment.Right;
            rightStyle.DataFormat = this._intergerFormat;

            var leftStyle = _workbook.CreateCellStyle();
            leftStyle.CloneStyleFrom(centerStyle);
            leftStyle.Alignment = HorizontalAlignment.Left;

            var percentStyle = _workbook.CreateCellStyle();
            percentStyle.CloneStyleFrom(rightStyle);
            percentStyle.DataFormat = this._pecentFormat;

            var decimalStyle = _workbook.CreateCellStyle();
            decimalStyle.CloneStyleFrom(rightStyle);
            decimalStyle.DataFormat = this._decimalFormat;

            var grayBackgroundLeftStyle = _workbook.CreateCellStyle();
            grayBackgroundLeftStyle.CloneStyleFrom(leftStyle);
            grayBackgroundLeftStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var grayBackgroundRightStyle = _workbook.CreateCellStyle();
            grayBackgroundRightStyle.CloneStyleFrom(rightStyle);
            grayBackgroundRightStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var grayBackgroundPercentStyle = _workbook.CreateCellStyle();
            grayBackgroundPercentStyle.CloneStyleFrom(percentStyle);
            grayBackgroundPercentStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var grayBackgroundDecimalStyle = _workbook.CreateCellStyle();
            grayBackgroundDecimalStyle.CloneStyleFrom(decimalStyle);
            grayBackgroundDecimalStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var cell = newRow.CreateCell(0);
            this.RenderCell(ref cell, centerStyle, winlossByBettypeObj.SportName);

            cell = newRow.CreateCell(1);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.BettypeName, grayBackgroundLeftStyle, leftStyle);

            cell = newRow.CreateCell(2);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.BetCount, grayBackgroundRightStyle, rightStyle);

            cell = newRow.CreateCell(3);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.TurnOverIBC, grayBackgroundRightStyle, rightStyle);

            cell = newRow.CreateCell(4);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.AverageValuePerBet, grayBackgroundDecimalStyle, decimalStyle);

            cell = newRow.CreateCell(5);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.WinLostIBC, grayBackgroundRightStyle, rightStyle);

            cell = newRow.CreateCell(6);
            this.RenderCellValue(cell, winlossByBettypeObj.BetCount, winlossByBettypeObj.Margin, grayBackgroundPercentStyle, percentStyle);
        }

        /// <summary>
        /// Renders the total win loss.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        /// <param name="winlossByBettypeObj">The win loss by bet type object.</param>
        internal void RenderTotalWinLoss(ref IRow newRow, WinLossSummaryBySportType winlossByBettypeObj)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);

            var leftStyle = _workbook.CreateCellStyle();
            leftStyle.CloneStyleFrom(this._summaryStyleBase);
            leftStyle.Alignment = HorizontalAlignment.Left;
            leftStyle.FillForegroundColor = SubTotalColorIndex;
            leftStyle.SetFont(font);

            var rightStyle = _workbook.CreateCellStyle();
            rightStyle.CloneStyleFrom(leftStyle);
            rightStyle.Alignment = HorizontalAlignment.Right;
            rightStyle.DataFormat = this._intergerFormat;

            var percentStyle = _workbook.CreateCellStyle();
            percentStyle.CloneStyleFrom(rightStyle);
            percentStyle.Alignment = HorizontalAlignment.Right;
            percentStyle.DataFormat = this._pecentFormat;

            var cell = newRow.CreateCell(0);
            this.RenderCell(ref cell, leftStyle, winlossByBettypeObj.SportName);

            cell = newRow.CreateCell(1);
            this.RenderCell(ref cell, leftStyle, winlossByBettypeObj.BettypeName);

            cell = newRow.CreateCell(2);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.BetCount);

            cell = newRow.CreateCell(3);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.TurnOverIBC);

            cell = newRow.CreateCell(4);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.AverageValuePerBet);

            cell = newRow.CreateCell(5);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.WinLostIBC);

            cell = newRow.CreateCell(6);
            this.RenderCell(ref cell, percentStyle, winlossByBettypeObj.Margin);
        }

        /// <summary>
        /// Renders the grand total win loss.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        /// <param name="winlossByBettypeObj">The win loss by bet type object.</param>
        internal void RenderGrandTotalWinLoss(ref IRow newRow, WinLossSummaryBySportType winlossByBettypeObj)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);

            var leftStyle = _workbook.CreateCellStyle();
            leftStyle.CloneStyleFrom(this._summaryStyleBase);
            leftStyle.Alignment = HorizontalAlignment.Left;
            leftStyle.FillForegroundColor = GrandTotalColorIndex;
            leftStyle.SetFont(font);

            var rightStyle = _workbook.CreateCellStyle();
            rightStyle.CloneStyleFrom(leftStyle);
            rightStyle.Alignment = HorizontalAlignment.Right;
            rightStyle.DataFormat = this._intergerFormat;

            var percentStyle = _workbook.CreateCellStyle();
            percentStyle.CloneStyleFrom(rightStyle);
            percentStyle.Alignment = HorizontalAlignment.Right;
            percentStyle.DataFormat = this._pecentFormat;

            var cell = newRow.CreateCell(0);
            this.RenderCell(ref cell, leftStyle, winlossByBettypeObj.SportName);

            cell = newRow.CreateCell(1);
            this.RenderCell(ref cell, leftStyle, winlossByBettypeObj.BettypeName);

            cell = newRow.CreateCell(2);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.BetCount);

            cell = newRow.CreateCell(3);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.TurnOverIBC);

            cell = newRow.CreateCell(4);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.AverageValuePerBet);

            cell = newRow.CreateCell(5);
            this.RenderCell(ref cell, rightStyle, winlossByBettypeObj.WinLostIBC);

            cell = newRow.CreateCell(6);
            this.RenderCell(ref cell, percentStyle, winlossByBettypeObj.Margin);
        }

        /// <summary>
        /// Renders the total win loss of sport type.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="totalWinlossBySportObj">The total win loss by sport object.</param>
        internal void RenderTotalWinLossOfSporttype(ref ISheet worksheet, WinLossSummaryBySportTypeTotal totalWinlossBySportObj)
        {
            var lengOfBettype = totalWinlossBySportObj.WinlostList.Count;
            int rowStartOfGroup = _startRowNumber + 1;
            int rowEndOfGroup = _startRowNumber + lengOfBettype;

            ICellStyle oddStyle = _workbook.CreateCellStyle();
            oddStyle.CloneStyleFrom(this._summaryStyleBase);
            oddStyle.FillForegroundColor = HSSFColor.White.Index;

            ICellStyle evenStyle = _workbook.CreateCellStyle();
            evenStyle.CloneStyleFrom(this._summaryStyleBase);
            evenStyle.FillForegroundColor = EvenRowColorIndex;

            ICellStyle totalStyle = _workbook.CreateCellStyle();
            totalStyle.CloneStyleFrom(this._summaryStyleBase);
            totalStyle.FillForegroundColor = SubTotalColorIndex;

            for (int j = 0; j < lengOfBettype; j++)
            {
                _startRowNumber++;
                var newRow = worksheet.CreateRow(_startRowNumber);
                RenderWinLossByBettype(ref newRow, totalWinlossBySportObj.WinlostList[j], j % 2 == 0 ? true : false);
            }

            _startRowNumber++;

            var totalRow = worksheet.CreateRow(_startRowNumber);
            RenderTotalWinLoss(ref totalRow, totalWinlossBySportObj.Total);
            worksheet.AddMergedRegion(new CellRangeAddress(rowStartOfGroup, rowEndOfGroup, 0, 0));
        }

        internal void RenderBody(ref ISheet worksheet, List<WinLossSummaryBySportTypeTotal> bodyDataList)
        {
            var lengOfTotalRow = bodyDataList.Count;

            for (int i = 0; i < lengOfTotalRow; i++)
            {
                this.RenderTotalWinLossOfSporttype(ref worksheet, bodyDataList[i]);
            }
        }

        internal void RenderFooter(ref ISheet worksheet, WinLossSummaryBySportType footerData)
        {
            ICellStyle headerStyle = _workbook.CreateCellStyle();
            headerStyle.CloneStyleFrom(this._summaryStyleBase);
            headerStyle.FillForegroundColor = GrandTotalColorIndex;
            _startRowNumber += 1;
            var grandTotalRow = worksheet.CreateRow(_startRowNumber);
            RenderGrandTotalWinLoss(ref grandTotalRow, footerData);
        }

        internal void RenderCellValue(ICell cell, decimal betCount, object value, ICellStyle grayBackgroundStyle, ICellStyle normalStyle)
        {
            if (betCount == 0)
            {
                this.RenderCell(ref cell, grayBackgroundStyle, value);
            }
            else
            {
                this.RenderCell(ref cell, normalStyle, value);
            }
        }
    }
}