// <copyright file="ExportExcelWinLossDetailService.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using Fanex.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Extensions;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Export excel for virtual sport's win loss.
    /// </summary>
    public class ExportExcelWinLossDetailService : ExportExcelBase
    {
        private HSSFWorkbook _workbook;
        private ICellStyle _detailStyleBase;

        private int _startRowNumber = 0;
        private short _doubleFormat;
        private short _percentFormat;

        public ExportExcelWinLossDetailService()
        {
        }

        /// <summary>
        /// Exports the excel win loss detail.
        /// </summary>
        /// <param name="workbook">The work book.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void ExportExcelWinlossDetail(ref HSSFWorkbook workbook)
        {
            List<VirtualSportExcelSheet> virtualSportDetailSheet = new List<VirtualSportExcelSheet>();
            var service = new WinLossDetailService();
            var model = service.GetWinLossDetail();

            var virtualSoccerSport = model.Where(item => item.SportID == 180).ToList();
            var virtualHorsesSport = model.Where(item => item.SportID == 181 || item.SportID == 182).ToList();
            var virtualMotorTennisSport = model.Where(item => item.SportID == 184 || item.SportID == 186).ToList();
            var virtualSpeedwayCyclingSport = model.Where(item => item.SportID == 183 || item.SportID == 185).ToList();

            virtualSportDetailSheet.Add(new VirtualSportExcelSheet
            {
                SheetName = "Soccer Daily",
                SportType = virtualSoccerSport
            });

            virtualSportDetailSheet.Add(new VirtualSportExcelSheet
            {
                SheetName = "Horses-Greyhounds Daily",
                SportType = virtualHorsesSport
            });

            virtualSportDetailSheet.Add(new VirtualSportExcelSheet
            {
                SheetName = "Motor-Tennis Daily",
                SportType = virtualMotorTennisSport
            });

            virtualSportDetailSheet.Add(new VirtualSportExcelSheet
            {
                SheetName = "Speedway-Cycling Daily",
                SportType = virtualSpeedwayCyclingSport
            });

            _workbook = workbook;
            this._detailStyleBase = this.CreateStyle(ref _workbook, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderColorIndex);

            this._doubleFormat = _workbook.CreateDataFormat().GetFormat("#,##0;[RED]-#,##0");
            this._percentFormat = _workbook.CreateDataFormat().GetFormat("#,##0.00%;[RED]-#,##0.00%;\"NA\"");

            if (virtualSportDetailSheet.Count > 0)
            {
                foreach (var sheet in virtualSportDetailSheet)
                {
                    CreateNewSheetBySport(sheet);
                }
            }
        }

        /// <summary>
        /// Creates the new sheet by sport.
        /// </summary>
        /// <param name="sheet">The sheet information.</param>
        internal void CreateNewSheetBySport(VirtualSportExcelSheet sheet)
        {
            _startRowNumber = 0;
            var worksheet = _workbook.CreateSheet(sheet.SheetName);
            if (sheet.SportType.Count > 0)
            {
                RenderHeaderBySport(ref worksheet, sheet);
                RenderBody(ref worksheet, sheet);
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
        /// Renders the header by sport.
        /// </summary>
        /// <param name="worksheet">The work sheet.</param>
        /// <param name="sheet">The virtual sport data information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        internal void RenderHeaderBySport(ref ISheet worksheet, VirtualSportExcelSheet sheet)
        {
            int startColumn = 0;
            int numberSport = sheet.SportType.Count;
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var headerFont = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.White.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var emptydataFont = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Red.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var titleStyle = _workbook.CreateCellStyle();
            titleStyle.CloneStyleFrom(this._detailStyleBase);
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.FillForegroundColor = TitleColorIndex;
            titleStyle.SetFont(font);

            var headerStyle = _workbook.CreateCellStyle();
            headerStyle.CloneStyleFrom(this._detailStyleBase);
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.FillForegroundColor = HeaderColorIndex;
            headerStyle.SetFont(headerFont);

            var emptyDataStyle = _workbook.CreateCellStyle();
            emptyDataStyle.CloneStyleFrom(this._detailStyleBase);
            emptyDataStyle.Alignment = HorizontalAlignment.Center;
            emptyDataStyle.FillForegroundColor = HSSFColor.White.Index;
            emptyDataStyle.SetFont(emptydataFont);

            if (numberSport > 0)
            {
                for (int sportIndex = 0; sportIndex < numberSport; sportIndex++)
                {
                    var currentSport = sheet.SportType[sportIndex];
                    var numberBettype = currentSport.WinLossByDateList.Count > 0 ?
                        currentSport.WinLossByDateList[0].BettypeList.Count :
                        0;
                    IRow startRow = worksheet.GetRow(_startRowNumber) ?? worksheet.CreateRow(_startRowNumber);

                    if (numberBettype > 0)
                    {
                        var cell = startRow.CreateCell(startColumn);
                        this.RenderCell(ref cell, titleStyle, currentSport.SportName);
                        worksheet.AddMergedRegion(new CellRangeAddress(0, 0, startColumn, startColumn + (numberBettype * 3)));

                        var headerRow1 = worksheet.GetRow(_startRowNumber + 1) ?? worksheet.CreateRow(_startRowNumber + 1);
                        var headerRow2 = worksheet.GetRow(_startRowNumber + 2) ?? worksheet.CreateRow(_startRowNumber + 2);

                        cell = headerRow1.CreateCell(startColumn);
                        this.RenderCell(ref cell, headerStyle, string.Empty);

                        cell = headerRow2.CreateCell(startColumn);
                        this.RenderCell(ref cell, headerStyle, string.Empty);

                        for (int bettypeIndex = 0; bettypeIndex < numberBettype; bettypeIndex++)
                        {
                            int startBetTypeGroupColumn = startColumn + (bettypeIndex * 3) + 1;
                            int endBetTypeGroupColumn = startColumn + (bettypeIndex * 3) + 3;

                            for (int cellIndex = startBetTypeGroupColumn; cellIndex <= endBetTypeGroupColumn; cellIndex++)
                            {
                                cell = headerRow1.GetCell(cellIndex) ?? headerRow1.CreateCell(cellIndex);
                                this.RenderCell(ref cell, headerStyle, string.Empty);
                            }

                            cell = headerRow1.CreateCell(startBetTypeGroupColumn);
                            this.RenderCell(ref cell, headerStyle, currentSport.WinLossByDateList[0].BettypeList[bettypeIndex].BettypeName);
                            worksheet.AddMergedRegion(new CellRangeAddress(1, 1, startBetTypeGroupColumn, endBetTypeGroupColumn));

                            cell = headerRow2.CreateCell(startBetTypeGroupColumn);
                            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.Turnover);

                            cell = headerRow2.CreateCell(++startBetTypeGroupColumn);
                            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.Winloss);

                            cell = headerRow2.CreateCell(++startBetTypeGroupColumn);
                            this.RenderCell(ref cell, headerStyle, WinLossVirtualSportResoure.Margin);
                        }

                        startColumn += (numberBettype * 3) + 2;
                    }
                    else
                    {
                        ICell cell = null;
                        var yesterday = DateTime.Now.AddDays(-1);
                        DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                        string nodataTitle = string.Format(
                            WinLossVirtualSportResoure.Sport_Nodata,
                            currentSport.SportName,
                            startMonth.ToShortFormatedDate(),
                            yesterday.ToShortFormatedDate());

                        for (int i = 0; i < 9; i++)
                        {
                            cell = startRow.CreateCell(startColumn + i);
                            this.RenderCell(ref cell, emptyDataStyle, string.Empty);
                        }

                        cell = startRow.GetCell(startColumn) ?? startRow.CreateCell(startColumn);
                        this.RenderCell(ref cell, emptyDataStyle, nodataTitle);
                        worksheet.AddMergedRegion(new CellRangeAddress(0, 0, startColumn, startColumn + 8));

                        startColumn = startColumn + 10;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the row by sport.
        /// </summary>
        /// <param name="worksheet">The work sheet.</param>
        /// <param name="winlossByDate">The win loss by date.</param>
        /// <param name="winlossByDateIndex">Index of the win loss by date.</param>
        /// <param name="startColumn">The start column.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        internal void RenderRowBySport(ref ISheet worksheet, WinLossDetailByDate winlossByDate, int winlossByDateIndex, int startColumn)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Normal);
            var boldFont = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var oddRowStyle = _workbook.CreateCellStyle();
            oddRowStyle.CloneStyleFrom(this._detailStyleBase);
            oddRowStyle.FillForegroundColor = HSSFColor.White.Index;
            oddRowStyle.Alignment = HorizontalAlignment.Right;
            oddRowStyle.DataFormat = _doubleFormat;
            oddRowStyle.SetFont(font);

            var evenRowStyle = _workbook.CreateCellStyle();
            evenRowStyle.CloneStyleFrom(this._detailStyleBase);
            evenRowStyle.FillForegroundColor = EvenRowColorIndex;
            evenRowStyle.Alignment = HorizontalAlignment.Right;
            evenRowStyle.DataFormat = _doubleFormat;
            evenRowStyle.SetFont(font);

            var zeroValueRowStyle = _workbook.CreateCellStyle();
            zeroValueRowStyle.CloneStyleFrom(this._detailStyleBase);
            zeroValueRowStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var marginOddRowStyle = _workbook.CreateCellStyle();
            marginOddRowStyle.CloneStyleFrom(oddRowStyle);
            marginOddRowStyle.DataFormat = _percentFormat;

            var marginEvenRowStyle = _workbook.CreateCellStyle();
            marginEvenRowStyle.CloneStyleFrom(evenRowStyle);
            marginEvenRowStyle.DataFormat = _percentFormat;

            var negativeMarginRowStyle = _workbook.CreateCellStyle();
            negativeMarginRowStyle.CloneStyleFrom(evenRowStyle);
            negativeMarginRowStyle.DataFormat = _percentFormat;
            negativeMarginRowStyle.FillForegroundColor = NegativeRowColorIndex;

            var totalOddRowStyle = _workbook.CreateCellStyle();
            totalOddRowStyle.CloneStyleFrom(oddRowStyle);
            totalOddRowStyle.SetFont(boldFont);

            var totalEvenRowStyle = _workbook.CreateCellStyle();
            totalEvenRowStyle.CloneStyleFrom(evenRowStyle);
            totalEvenRowStyle.SetFont(boldFont);

            var totalMarginOddRowStyle = _workbook.CreateCellStyle();
            totalMarginOddRowStyle.CloneStyleFrom(marginOddRowStyle);
            totalMarginOddRowStyle.SetFont(boldFont);

            var totalMarginEvenRowStyle = _workbook.CreateCellStyle();
            totalMarginEvenRowStyle.CloneStyleFrom(marginEvenRowStyle);
            totalMarginEvenRowStyle.SetFont(boldFont);

            var totalNegativeMarginRowStyle = _workbook.CreateCellStyle();
            totalNegativeMarginRowStyle.CloneStyleFrom(negativeMarginRowStyle);
            totalNegativeMarginRowStyle.SetFont(boldFont);

            var lengthOfBettypeList = winlossByDate.BettypeList.Count;
            worksheet.SetColumnWidth(startColumn, 15 * 256);
            var newRow = worksheet.GetRow(_startRowNumber) ?? worksheet.CreateRow(_startRowNumber);
            var cell = newRow.CreateCell(startColumn);
            this.RenderCell(ref cell, winlossByDateIndex % 2 == 0 ? evenRowStyle : oddRowStyle, winlossByDate.WinLossDate.ToString("d-MMM-yyyy"));

            int startBetTypeGroupColumn = 0;
            for (int i = 0; i < lengthOfBettypeList - 1; i++)
            {
                startBetTypeGroupColumn = startColumn + (i * 3) + 1;
                var winlossByBettype = winlossByDate.BettypeList[i];

                cell = newRow.CreateCell(startBetTypeGroupColumn);
                this.RenderTurnOverAndWinLossColumn(
                    cell,
                    zeroValueRowStyle,
                    oddRowStyle,
                    evenRowStyle,
                    winlossByBettype.BetCount,
                    winlossByBettype.TurnOver,
                    winlossByDateIndex);

                cell = newRow.CreateCell(++startBetTypeGroupColumn);
                this.RenderTurnOverAndWinLossColumn(
                    cell,
                    zeroValueRowStyle,
                    oddRowStyle,
                    evenRowStyle,
                    winlossByBettype.BetCount,
                    winlossByBettype.WinLoss,
                    winlossByDateIndex);

                cell = newRow.CreateCell(++startBetTypeGroupColumn);
                this.RenderMarginColumn(cell, negativeMarginRowStyle, marginOddRowStyle, marginEvenRowStyle, winlossByBettype.Margin, winlossByDateIndex);
            }

            var totalByBetTypeIndex = lengthOfBettypeList - 1;
            startBetTypeGroupColumn = startColumn + (totalByBetTypeIndex * 3) + 1;
            var totalByBetType = winlossByDate.BettypeList[totalByBetTypeIndex];

            cell = newRow.CreateCell(startBetTypeGroupColumn);
            this.RenderTurnOverAndWinLossColumn(
                cell,
                zeroValueRowStyle,
                totalOddRowStyle,
                totalEvenRowStyle,
                totalByBetType.BetCount,
                totalByBetType.TurnOver,
                winlossByDateIndex);

            cell = newRow.CreateCell(++startBetTypeGroupColumn);
            this.RenderTurnOverAndWinLossColumn(
                cell,
                zeroValueRowStyle,
                totalOddRowStyle,
                totalEvenRowStyle,
                totalByBetType.BetCount,
                totalByBetType.WinLoss,
                winlossByDateIndex);

            cell = newRow.CreateCell(++startBetTypeGroupColumn);
            this.RenderMarginColumn(cell, totalNegativeMarginRowStyle, totalMarginOddRowStyle, totalMarginEvenRowStyle, totalByBetType.Margin, winlossByDateIndex);
        }

        /// <summary>
        /// Renders the body.
        /// </summary>
        /// <param name="worksheet">The work sheet.</param>
        /// <param name="virtualSportSheet">The virtual sport sheet.</param>
        internal void RenderBody(ref ISheet worksheet, VirtualSportExcelSheet virtualSportSheet)
        {
            int startColumn = 0;
            int numberSport = virtualSportSheet.SportType.Count;

            for (int sportIndex = 0; sportIndex < numberSport; sportIndex++)
            {
                _startRowNumber = 3;
                var currentSport = virtualSportSheet.SportType[sportIndex];

                var currentSportByDateLength = currentSport.WinLossByDateList.Count;
                var numberBettypeOfSport = currentSport.WinLossByDateList.Count > 0 ?
                    currentSport.WinLossByDateList[0].BettypeList.Count
                    : 0;

                if (currentSportByDateLength > 0)
                {
                    for (int i = 0; i < currentSportByDateLength - 1; i++)
                    {
                        RenderRowBySport(ref worksheet, currentSport.WinLossByDateList[i], i, startColumn);
                        _startRowNumber++;
                    }

                    RenderTotalRow(ref worksheet, currentSport.WinLossByDateList[currentSportByDateLength - 1], startColumn);
                }

                startColumn += (numberBettypeOfSport * 3) + 2;
            }
        }

        private void RenderTurnOverAndWinLossColumn(
            ICell cell,
            ICellStyle zeroValueStyle,
            ICellStyle oddRowStyle,
            ICellStyle evenRowStyle,
            decimal betCount,
            object cellValue,
            int rowIndex)
        {
            if (betCount == 0)
            {
                this.RenderCell(ref cell, zeroValueStyle, string.Empty);
            }
            else
            {
                this.RenderCell(ref cell, rowIndex % 2 == 0 ? evenRowStyle : oddRowStyle, cellValue);
            }
        }

        private void RenderMarginColumn(
            ICell cell,
            ICellStyle negativeValueStyle,
            ICellStyle oddRowStyle,
            ICellStyle evenRowStyle,
            object cellValue,
            int rowIndex)
        {
            if (Converter.ToDecimal(cellValue) < 0)
            {
                this.RenderCell(ref cell, negativeValueStyle, cellValue);
            }
            else
            {
                this.RenderCell(ref cell, rowIndex % 2 == 0 ? evenRowStyle : oddRowStyle, cellValue);
            }
        }

        /// <summary>
        /// Renders the total row.
        /// </summary>
        /// <param name="worksheet">The work sheet.</param>
        /// <param name="total">The total information.</param>
        /// <param name="startColumn">The start column.</param>
        private void RenderTotalRow(
            ref ISheet worksheet,
            WinLossDetailByDate total,
            int startColumn)
        {
            var font = this.CreateFont(ref this._workbook, "Tahoma", 9, HSSFColor.Black.Index, (short)NPOI.SS.UserModel.FontBoldWeight.Bold);
            var totalRowStyle = _workbook.CreateCellStyle();
            totalRowStyle.CloneStyleFrom(this._detailStyleBase);
            totalRowStyle.FillForegroundColor = GrandTotalColorIndex;
            totalRowStyle.Alignment = HorizontalAlignment.Right;
            totalRowStyle.DataFormat = _doubleFormat;
            totalRowStyle.SetFont(font);

            var zeroValueRowStyle = _workbook.CreateCellStyle();
            zeroValueRowStyle.CloneStyleFrom(this._detailStyleBase);
            zeroValueRowStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            var marginOddRowStyle = _workbook.CreateCellStyle();
            marginOddRowStyle.CloneStyleFrom(totalRowStyle);
            marginOddRowStyle.DataFormat = _percentFormat;

            var marginRowStyle = _workbook.CreateCellStyle();
            marginRowStyle.CloneStyleFrom(totalRowStyle);
            marginRowStyle.DataFormat = _percentFormat;

            var negativeMarginRowStyle = _workbook.CreateCellStyle();
            negativeMarginRowStyle.CloneStyleFrom(totalRowStyle);
            negativeMarginRowStyle.DataFormat = _percentFormat;
            negativeMarginRowStyle.FillForegroundColor = NegativeRowColorIndex;

            var lengthOfBettypeList = total.BettypeList.Count;
            worksheet.SetColumnWidth(0, 15 * 256);
            var newRow = worksheet.GetRow(_startRowNumber) ?? worksheet.CreateRow(_startRowNumber);
            var cell = newRow.CreateCell(startColumn);
            this.RenderCell(ref cell, totalRowStyle, WinLossVirtualSportResoure.Total);

            int startBetTypeGroupColumn = 0;
            for (int i = 0; i < lengthOfBettypeList; i++)
            {
                startBetTypeGroupColumn = startColumn + (i * 3) + 1;
                var winlossByBettype = total.BettypeList[i];

                cell = newRow.CreateCell(startBetTypeGroupColumn);
                this.RenderTurnOverAndWinLossTotalColumn(cell, zeroValueRowStyle, totalRowStyle, winlossByBettype.BetCount, winlossByBettype.TurnOver);

                cell = newRow.CreateCell(++startBetTypeGroupColumn);
                this.RenderTurnOverAndWinLossTotalColumn(cell, zeroValueRowStyle, totalRowStyle, winlossByBettype.BetCount, winlossByBettype.WinLoss);

                cell = newRow.CreateCell(++startBetTypeGroupColumn);
                this.RenderMarginTotalColumn(cell, negativeMarginRowStyle, marginRowStyle, winlossByBettype.Margin);
            }
        }

        private void RenderTurnOverAndWinLossTotalColumn(
            ICell cell,
            ICellStyle zeroValueStyle,
            ICellStyle totalStyle,
            decimal betCount,
            object cellValue)
        {
            if (betCount == 0)
            {
                this.RenderCell(ref cell, zeroValueStyle, string.Empty);
            }
            else
            {
                this.RenderCell(ref cell, totalStyle, cellValue);
            }
        }

        private void RenderMarginTotalColumn(
            ICell cell,
            ICellStyle negativeValueStyle,
            ICellStyle totalStyle,
            object cellValue)
        {
            if (Converter.ToDecimal(cellValue) < 0)
            {
                this.RenderCell(ref cell, negativeValueStyle, cellValue);
            }
            else
            {
                this.RenderCell(ref cell, totalStyle, cellValue);
            }
        }
    }
}