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
    using System.Drawing;
    using System.Linq;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.XSSF.UserModel;
    
    public class ExportExcelWinLossDetailService 
    {
        private Color _titleColor;
        private Color _headerColor;
        private Color _evenColor;
        private Color _greyColor;
        private Color _grandColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;

        private XSSFCellStyle _headerStyle;
        private XSSFCellStyle _textCenterNormalStyle;
        private XSSFCellStyle _textCenterTitleStyle;
        private XSSFCellStyle _textRightNormalStyle;
        private XSSFCellStyle _numberNormalStyle;
        private XSSFCellStyle _decimalNormalStyle;
        private XSSFCellStyle _percentNormalStyle;

        private XSSFCellStyle _textRightEvenStyle;
        private XSSFCellStyle _numberEvenStyle;
        private XSSFCellStyle _decimalEvenStyle;
        private XSSFCellStyle _percentEvenStyle;

        private XSSFCellStyle _numberGreyStyle;
        private XSSFCellStyle _decimalGreyStyle;
        private XSSFCellStyle _percentGreyStyle;

        private XSSFCellStyle _textGrandTotalRightStyle;
        private XSSFCellStyle _numberGrandTotalStyle;
        private XSSFCellStyle _decimalGrandTotalStyle;
        private XSSFCellStyle _percentGrandTotalStyle;

        private XSSFCellStyle _textStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _percentStyle;  

        public ExportExcelWinLossDetailService(XSSFWorkbook workbook)
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
            _grandColor = Color.FromArgb(220, 226, 188);
        }

        private void CreateStyle()
        {
            _headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            _textCenterTitleStyle = _customCellStyleService.CreateCenterTextStyle(_titleColor, _fontService.BlackBoldFont);
            _textCenterNormalStyle = _bodyCellStyleService.CenterTextStyle;
            _textRightNormalStyle = _bodyCellStyleService.RightTextStyle;
            _numberNormalStyle = _bodyCellStyleService.RightNumberStyle;
            _decimalNormalStyle = _bodyCellStyleService.DecimalNumberStyle;
            _percentNormalStyle = _bodyCellStyleService.NegativePercentageStyle;

            _textRightEvenStyle = _customCellStyleService.CreateRightTextStyle(_evenColor, _fontService.RegularFont);
            _numberEvenStyle = _customCellStyleService.CreateRightNumberStyle(_evenColor, _fontService.RegularFont);
            _decimalEvenStyle = _customCellStyleService.CreateDecimalNumberStyle(_evenColor, _fontService.RegularFont);
            _percentEvenStyle = _customCellStyleService.CreateNegativePercentageStyle(_evenColor, _fontService.RegularFont);

            _numberGreyStyle = _customCellStyleService.CreateRightNumberStyle(_greyColor, _fontService.RegularFont);
            _decimalGreyStyle = _customCellStyleService.CreateDecimalNumberStyle(_greyColor, _fontService.RegularFont);
            _percentGreyStyle = _customCellStyleService.CreateNegativePercentageStyle(_greyColor, _fontService.RegularFont);

            _textGrandTotalRightStyle = _customCellStyleService.CreateRightTextStyle(_grandColor, _fontService.BlackBoldFont);
            _numberGrandTotalStyle = _customCellStyleService.CreateRightNumberStyle(_grandColor, _fontService.BlackBoldFont);
            _decimalGrandTotalStyle = _customCellStyleService.CreateDecimalNumberStyle(_grandColor, _fontService.BlackBoldFont);
            _percentGrandTotalStyle = _customCellStyleService.CreateNegativePercentageStyle(_grandColor, _fontService.BlackBoldFont);
        }

        public void ExportExcelDetail()
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

            if (virtualSportDetailSheet.Count > 0)
            {
                foreach (var sheet in virtualSportDetailSheet)
                {
                    CreateNewSheetBySport(sheet);              
                }
            }
        }

        private void CreateNewSheetBySport(VirtualSportExcelSheet sheet)
        {
            _workSheet = _workbook.CreateSheet(sheet.SheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(1, 3);
            _cell = new Cell();
            if (sheet.SportType.Count > 0)
            {                
                RenderHeaderBySport(sheet);
                RenderBody(sheet);
            }
            else
            {
                var yesterday = DateTime.Now.AddDays(-1);
                DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                string nodataTitle = string.Format(WinLossVirtualSportResoure.NoData, startMonth.ToShortFormatedDate(), yesterday.ToShortFormatedDate());
                _cell.CellValue = nodataTitle;
                _cell.EndColumn = _cell.CurrentColumn + 7;
                NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textCenterTitleStyle);
            }
        }

        private void RenderHeaderBySport(VirtualSportExcelSheet sheet)
        {
            int startColumn = 0;            
            int numberSport = sheet.SportType.Count;                            
            for (int sportIndex = 0; sportIndex < numberSport; sportIndex++)
            {
                var currentSport = sheet.SportType[sportIndex];
                var numberBettype = currentSport.WinLossByDateList.Count > 0 ?
                    currentSport.WinLossByDateList[0].BettypeList.Count : 0;

                if (numberBettype > 0)
                {                        
                    _cell.CurrentColumn = startColumn;
                    _cell.EndColumn = startColumn + (numberBettype * 3);
                    _cell.CurrentRow = 0;
                    _cell.EndRow = 0;
                    _cell.CellValue = currentSport.SportName;
                    NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textCenterTitleStyle);

                    _cell.CurrentColumn = startColumn;
                    _cell.CurrentRow = 2;
                    _cell.CellValue = string.Empty;                    
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);
                 
                    for (int bettypeIndex = 0; bettypeIndex < numberBettype; bettypeIndex ++)
                    {
                        int startBetTypeGroupColumn = startColumn + (bettypeIndex * 3) + 1;
                        int endBetTypeGroupColumn = startColumn + (bettypeIndex * 3) + 3;

                        _cell.CurrentRow = 1;
                        _cell.EndRow = 1;

                        _cell.CurrentColumn = startBetTypeGroupColumn - 1;                        
                        _cell.CellValue = string.Empty;
                        NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

                        _cell.CurrentColumn = startBetTypeGroupColumn;
                        _cell.EndColumn = _cell.CurrentColumn;
                        _cell.EndColumn = endBetTypeGroupColumn;                            
                        _cell.CellValue = currentSport.WinLossByDateList[0].BettypeList[bettypeIndex].BettypeName;                            
                        NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _headerStyle);

                        _cell.CurrentRow = 2;
                        _cell.EndRow = 2;                        

                        _cell.CurrentColumn = startBetTypeGroupColumn;
                        _cell.CellValue = WinLossVirtualSportResoure.Turnover;
                        NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

                        _cell.CurrentColumn = startBetTypeGroupColumn + 1;
                        _cell.CellValue = WinLossVirtualSportResoure.Winloss;
                        NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

                        _cell.CurrentColumn = startBetTypeGroupColumn + 2;
                        _cell.CellValue = WinLossVirtualSportResoure.Margin;
                        NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);
                    }

                    startColumn += (numberBettype * 3) + 2;
                }
                else
                {
                    var yesterday = DateTime.Now.AddDays(-1);
                    DateTime startMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                    string nodataTitle = string.Format(
                        WinLossVirtualSportResoure.Sport_Nodata,
                        currentSport.SportName,
                        startMonth.ToShortFormatedDate(),
                        yesterday.ToShortFormatedDate());

                    _cell.CellValue = nodataTitle;
                    _cell.CurrentColumn = startColumn;
                    _cell.EndColumn = startColumn + 8;
                    NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _textCenterNormalStyle);

                    startColumn = startColumn + 10;
                }
            }            
        }

        private void RenderBody(VirtualSportExcelSheet virtualSportSheet)
        {
            int startColumn = 0;
            int currentRow = 0;
            int numberSport = virtualSportSheet.SportType.Count;

            for (int sportIndex = 0; sportIndex < numberSport; sportIndex++)
            {
                currentRow = 3;
                var currentSport = virtualSportSheet.SportType[sportIndex];

                var currentSportByDateLength = currentSport.WinLossByDateList.Count;
                var numberBettypeOfSport = currentSport.WinLossByDateList.Count > 0 ?
                    currentSport.WinLossByDateList[0].BettypeList.Count
                    : 0;

                if (currentSportByDateLength > 0)
                {
                    for (int i = 0; i < currentSportByDateLength - 1; i++)
                    {
                        if (sportIndex == 0)
                        {
                            _cell.IncreaseRowIndex();
                        }

                        RenderRowBySport(currentSport.WinLossByDateList[i], i, startColumn, currentRow);
                        currentRow++;                       
                    }

                    RenderTotalRow(currentSport.WinLossByDateList[currentSportByDateLength - 1], startColumn, currentRow);
                }

                startColumn += (numberBettypeOfSport * 3) + 2;
            }
        }

        private void RenderRowBySport(WinLossDetailByDate winlossByDate, int winlossByDateIndex, int startColumn, int currentRow)
        {
            var lengthOfBettypeList = winlossByDate.BettypeList.Count;            
            
            _cell.CurrentColumn = startColumn;
            _cell.CurrentRow = currentRow;            
            _cell.CellValue = winlossByDate.WinLossDate.ToString("d-MMM-yyyy");
            _textStyle = winlossByDateIndex % 2 == 0 ? _textRightNormalStyle : _textRightEvenStyle;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _textStyle);

            int startBetTypeGroupColumn = 0;
            for (int i = 0; i < lengthOfBettypeList - 1; i++)
            {
                startBetTypeGroupColumn = startColumn + (i * 3) + 1;
                var row = winlossByDate.BettypeList[i];
                RenderRow(startBetTypeGroupColumn, winlossByDateIndex, row);
            }

            var totalByBetTypeIndex = lengthOfBettypeList - 1;
            startBetTypeGroupColumn = startColumn + (totalByBetTypeIndex * 3) + 1;
            var total = winlossByDate.BettypeList[totalByBetTypeIndex];
            RenderRow(startBetTypeGroupColumn, winlossByDateIndex, total);
        }

        private void RenderRow(int startBetTypeGroupColumn, int rowIndex, WinLossDetailByBettype row)
        {            
            _numberStyle = row.BetCount == 0 ? _numberGreyStyle : (rowIndex % 2 == 0 ? _numberNormalStyle : _numberEvenStyle);
            _decimalStyle = row.BetCount == 0 ? _decimalGreyStyle : (rowIndex % 2 == 0 ? _decimalNormalStyle : _decimalEvenStyle);
            _percentStyle = row.BetCount == 0 ? _percentGreyStyle : (rowIndex % 2 == 0 ? _percentNormalStyle : _percentEvenStyle);

            if (row.BetCount == 0)
            {
                _cell.CurrentColumn = startBetTypeGroupColumn;
                _cell.CellValue = string.Empty;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberGreyStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = string.Empty;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberGreyStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = "N/A";
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberGreyStyle);
            }
            else
            {
                _cell.CurrentColumn = startBetTypeGroupColumn;
                _cell.CellValue = row.TurnOver;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = row.WinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = row.Margin;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _percentStyle);
            }
        }

        private void RenderTotalRow(WinLossDetailByDate total, int startColumn, int currentRow)
        {
            var lengthOfBettypeList = total.BettypeList.Count;
            _cell.ResetColumnIndex();
            _cell.CurrentColumn = startColumn;
            _cell.CurrentRow = currentRow;
           
            _cell.CellValue = WinLossVirtualSportResoure.Total;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _textGrandTotalRightStyle);

            int startBetTypeGroupColumn = 0;
            for (int i = 0; i < lengthOfBettypeList; i++)
            {
                startBetTypeGroupColumn = startColumn + (i * 3) + 1;
                var winlossByBettype = total.BettypeList[i];
                RenderRowTotal(startBetTypeGroupColumn, winlossByBettype);
            }
        }

        private void RenderRowTotal(int startBetTypeGroupColumn, WinLossDetailByBettype row)
        {
            _numberStyle = row.BetCount == 0 ? _textGrandTotalRightStyle : _numberGrandTotalStyle;
            _decimalStyle = row.BetCount == 0 ? _textGrandTotalRightStyle : _decimalGrandTotalStyle;
            _percentStyle = row.BetCount == 0 ? _textGrandTotalRightStyle : _percentGrandTotalStyle;

            if (row.BetCount == 0)
            {
                _cell.CurrentColumn = startBetTypeGroupColumn;
                _cell.CellValue = string.Empty;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = string.Empty;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = "N/A";
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);
            }
            else
            {
                _cell.CurrentColumn = startBetTypeGroupColumn;
                _cell.CellValue = row.TurnOver;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = row.WinLoss;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _numberStyle);

                _cell.CurrentColumn = ++startBetTypeGroupColumn;
                _cell.CellValue = row.Margin;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _percentStyle);
            }
        }
    }
}