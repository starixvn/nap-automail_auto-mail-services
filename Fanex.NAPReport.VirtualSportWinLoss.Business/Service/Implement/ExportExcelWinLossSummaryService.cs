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
    using System.Drawing;
    using System.IO;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Export excel for virtual sport's win loss.
    /// </summary>
    public class ExportExcelWinLossSummaryService 
    {
        private Color _titleColor;
        private Color _headerColor;
        private Color _evenColor;
        private Color _greyColor;
        private Color _subColor;
        private Color _grandColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;

        private XSSFCellStyle _textCenterNormalStyle;
        private XSSFCellStyle _textLeftTitleStyle;
        private XSSFCellStyle _textLeftNormalStyle;
        private XSSFCellStyle _numberNormalStyle;
        private XSSFCellStyle _decimalNormalStyle;
        private XSSFCellStyle _percentNormalStyle;

        private XSSFCellStyle _textCenterEvenStyle;
        private XSSFCellStyle _textLeftEvenStyle;
        private XSSFCellStyle _numberEvenStyle;
        private XSSFCellStyle _decimalEvenStyle;
        private XSSFCellStyle _percentEvenStyle;

        private XSSFCellStyle _textCenterGreyStyle;
        private XSSFCellStyle _textLeftGreyStyle;
        private XSSFCellStyle _numberGreyStyle;
        private XSSFCellStyle _decimalGreyStyle;
        private XSSFCellStyle _percentGreyStyle;

        private XSSFCellStyle _textTotalCenterStyle;
        private XSSFCellStyle _textTotalLeftStyle;
        private XSSFCellStyle _numberTotalStyle;
        private XSSFCellStyle _decimalTotalStyle;
        private XSSFCellStyle _percentTotalStyle;

        private XSSFCellStyle _textGrandTotalCenterStyle;
        private XSSFCellStyle _textGrandTotalLeftStyle;
        private XSSFCellStyle _numberGrandTotalStyle;
        private XSSFCellStyle _decimalGrandTotalStyle;
        private XSSFCellStyle _percentGrandTotalStyle;

        private XSSFCellStyle _textLeftStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _percentStyle;

        public ExportExcelWinLossSummaryService(XSSFWorkbook workbook)
        {
            _cell = new Cell();
            _workbook = workbook;
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);

            CreateStyle();
        }

        private void CreateColor()
        {
            _titleColor = Color.FromArgb(255, 204, 51);
            _headerColor = Color.FromArgb(60, 94, 165);
            _evenColor = Color.FromArgb(228, 242, 253);
            _greyColor = Color.FromArgb(191, 191, 191);
            _subColor = Color.FromArgb(244, 244, 190);
            _grandColor = Color.FromArgb(220, 226, 188);
        }

        private void CreateSheet()
        {
            string sheetName = WinLossVirtualSportResoure.Summary;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(0, 5);
        }

        private void CreateStyle()
        {
            _textLeftTitleStyle = _customCellStyleService.CreateLeftTextStyle(_titleColor, _fontService.BlackBoldFont);
            _textCenterNormalStyle = _bodyCellStyleService.CenterTextStyle;
            _textLeftNormalStyle = _bodyCellStyleService.LeftTextStyle;
            _numberNormalStyle = _bodyCellStyleService.RightNumberStyle;
            _decimalNormalStyle = _bodyCellStyleService.DecimalNumberStyle;
            _percentNormalStyle = _bodyCellStyleService.NegativePercentageStyle;

            _textCenterEvenStyle = _customCellStyleService.CreateCenterTextStyle(_evenColor, _fontService.RegularFont);
            _textLeftEvenStyle = _customCellStyleService.CreateLeftTextStyle(_evenColor, _fontService.RegularFont);
            _numberEvenStyle = _customCellStyleService.CreateRightNumberStyle(_evenColor, _fontService.RegularFont);
            _decimalEvenStyle = _customCellStyleService.CreateDecimalNumberStyle(_evenColor, _fontService.RegularFont);
            _percentEvenStyle = _customCellStyleService.CreateNegativePercentageStyle(_evenColor, _fontService.RegularFont);

            _textCenterGreyStyle = _customCellStyleService.CreateCenterTextStyle(_greyColor, _fontService.RegularFont);
            _textLeftGreyStyle = _customCellStyleService.CreateLeftTextStyle(_greyColor, _fontService.RegularFont);
            _numberGreyStyle = _customCellStyleService.CreateRightNumberStyle(_greyColor, _fontService.RegularFont);
            _decimalGreyStyle = _customCellStyleService.CreateDecimalNumberStyle(_greyColor, _fontService.RegularFont);
            _percentGreyStyle = _customCellStyleService.CreateNegativePercentageStyle(_greyColor, _fontService.RegularFont);

            _textTotalCenterStyle = _customCellStyleService.CreateCenterTextStyle(_subColor, _fontService.BlackBoldFont);
            _textTotalLeftStyle = _customCellStyleService.CreateLeftTextStyle(_subColor, _fontService.BlackBoldFont);
            _numberTotalStyle = _customCellStyleService.CreateRightNumberStyle(_subColor, _fontService.BlackBoldFont);
            _decimalTotalStyle = _customCellStyleService.CreateDecimalNumberStyle(_subColor, _fontService.BlackBoldFont);
            _percentTotalStyle = _customCellStyleService.CreateNegativePercentageStyle(_subColor, _fontService.BlackBoldFont);

            _textGrandTotalCenterStyle = _customCellStyleService.CreateCenterTextStyle(_grandColor, _fontService.BlackBoldFont);
            _textGrandTotalLeftStyle = _customCellStyleService.CreateLeftTextStyle(_grandColor, _fontService.BlackBoldFont);
            _numberGrandTotalStyle = _customCellStyleService.CreateRightNumberStyle(_grandColor, _fontService.BlackBoldFont);
            _decimalGrandTotalStyle = _customCellStyleService.CreateDecimalNumberStyle(_grandColor, _fontService.BlackBoldFont);
            _percentGrandTotalStyle = _customCellStyleService.CreateNegativePercentageStyle(_grandColor, _fontService.BlackBoldFont);
        }
        
        public void ExportExcelMain()
        {
            CreateSheet();
            InitColumnWidth();
            RenderTitle();
            RenderHeader();
            ExportMain();
        }

        private void ExportMain()
        {
            var service = new WinLossSummaryService();
            var model = service.GetWinLossSummary();

            if (model.WinlostTotalList.Count > 0)
            {
                RenderBody(model.WinlostTotalList);
                RenderFooter(model.GrandTotal);
            }
            else
            {
                var yesterday = DateTime.Now.AddDays(-1);
                DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);

                _cell.CellValue = string.Format(WinLossVirtualSportResoure.NoData, startMonth.ToShortFormatedDate(), yesterday.ToShortFormatedDate());
                _cell.EndColumn = _cell.CurrentColumn + 6;
                NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textCenterNormalStyle);
                _cell.IncreaseColumnIndex();
            }
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateLeftTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private void RenderTitle()
        {
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

            for (int i = 0; i < lines.Length; i++)
            {
                _cell.EndColumn = _cell.CurrentColumn + 6;
                _cell.CellValue = lines[i];
                NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textLeftTitleStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private void InitColumnWidth()
        {
            for (int i = 0; i < 7; i++)
            {
                _workSheet.SetColumnWidth(i, 170 * NPOIConstants.FontWidth);
            }

            _workSheet.SetColumnWidth(0, 220 * NPOIConstants.FontWidth);
        }

        private void RenderHeader()
        {
            var headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.Sport;            
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.BetType;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.SumBetCount;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.TurnoverIBC;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.AvarageValuePerBet;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.WinLossIBC;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = WinLossVirtualSportResoure.Margin;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void RenderTotalWinLossOfSporttype(WinLossSummaryBySportTypeTotal totalWinlossBySportObj)
        {
            var numberOfBettypes = totalWinlossBySportObj.WinlostList.Count;
            for (int j = 0; j < numberOfBettypes; j++)
            {
                var row = totalWinlossBySportObj.WinlostList[j];
                if (j == 0)
                {
                    _cell.EndRow = _cell.CurrentRow + totalWinlossBySportObj.WinlostList.Count - 1;
                    _cell.CellValue = row.SportName;
                    NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textCenterNormalStyle);
                }

                _textLeftStyle = row.BetCount == 0 ? _textLeftGreyStyle : (j % 2 == 0 ? _textLeftNormalStyle : _textLeftEvenStyle);
                _numberStyle = row.BetCount == 0 ? _numberGreyStyle : (j % 2 == 0 ? _numberNormalStyle : _numberEvenStyle);
                _decimalStyle = row.BetCount == 0 ? _decimalGreyStyle : (j % 2 == 0 ? _decimalNormalStyle : _decimalEvenStyle);
                _percentStyle = row.BetCount == 0 ? _percentGreyStyle : (j % 2 == 0 ? _percentNormalStyle : _percentEvenStyle);

                _cell.CurrentColumn = 1;
                _cell.EndColumn = 1;
                _cell.EndRow = _cell.CurrentRow;

                RenderRow(row);
            }
            
            _cell.ResetColumnIndex();
            
            var total = totalWinlossBySportObj.Total;
            _textLeftStyle = _textTotalLeftStyle; 
            _numberStyle = _numberTotalStyle;
            _decimalStyle = _decimalTotalStyle;
            _percentStyle = _percentTotalStyle;

            _cell.CellValue = total.SportName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _textLeftStyle);
            _cell.IncreaseColumnIndex();

            RenderRow(total);

            _cell.ResetColumnIndex();            
        }

        private void RenderRow(WinLossSummaryBySportType row)
        {
            _cell.CellValue = row.BettypeName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _textLeftStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = row.BetCount;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = row.TurnOverIBC;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = row.AverageValuePerBet;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _decimalStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = row.WinLostIBC;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = row.Margin;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _percentStyle);
            _cell.IncreaseRowIndex();
        }

        private void RenderBody(List<WinLossSummaryBySportTypeTotal> bodyDataList)
        {
            var lengOfTotalRow = bodyDataList.Count;
            for (int i = 0; i < lengOfTotalRow; i++)
            {
                this.RenderTotalWinLossOfSporttype(bodyDataList[i]);
            }
        }

        private void RenderFooter(WinLossSummaryBySportType footerData)
        {
            _textLeftStyle = _textGrandTotalLeftStyle;
            _numberStyle = _numberGrandTotalStyle;
            _decimalStyle = _decimalGrandTotalStyle;
            _percentStyle = _percentGrandTotalStyle;

            _cell.CellValue = footerData.SportName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _textLeftStyle);
            _cell.IncreaseColumnIndex();

            RenderRow(footerData);

            _cell.ResetColumnIndex(); 
        }
    }
}