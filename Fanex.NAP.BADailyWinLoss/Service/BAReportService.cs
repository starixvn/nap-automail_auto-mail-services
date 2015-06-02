namespace Fanex.NAPReport.BADailyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Fanex.NAPReport.BADailyWinLoss.Business.Model;
    using Fanex.NAPReport.BADailyWinLoss.Business.Resource;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    public class BAReportService
    {
        private const string NotAvailableStr = "N/A";
        private const int ColumnWidth = 16;
        private XSSFWorkbook _workbook;
        private XSSFSheet _sheet;
        private int _currentRow = 0;
        private FontInformation _headerFont;
        private Color _borderColor = Color.FromArgb(124, 152, 164);
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiAlignmentService _alignmentService;

        public string FileName { get; set; }

        public string DirectoryOfFileName { get; set; }

        public void GenerateReport(DateTime winLossDate)
        {
            this.FileName = string.Format(Resource.FileName, winLossDate.ToString("yyyy-MM-dd"));
            this.DirectoryOfFileName = AppSettings.DataDirectoryWithYearMonth + @"\" + this.FileName;
            var service = new BetAggregatorService();
            this._workbook = new XSSFWorkbook();

            var dailyList = service.GetWinlossDaily(winLossDate);

            var monthToDateList = service.GetWinlossMonthToDate(winLossDate);

            var yearToDateList = service.GetWinlossYearToDate(winLossDate);

            InitWorkBook();

            BuildTitle();

            BuildDaily(winLossDate, dailyList);

            BuildMonthYearToDate(Resource.MonthToDate, monthToDateList);

            BuildMonthYearToDate(Resource.YearToDate, yearToDateList);

            SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYearMonth, this.FileName, this._workbook, AppSettings.ExcelPasswordProtected);
        }

        private void InitWorkBook()
        {
            this._workbook = new XSSFWorkbook();
            this._sheet = this._workbook.CreateSheet("Sheet1") as XSSFSheet;
            _headerFont = new FontInformation(NPOIConstants.FontName, 16, Color.Black);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            
            _cell = new Cell();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _sheet.DefaultColumnWidth = ColumnWidth;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _fontService.RegularFont);
            _alignmentService = new NpoiNewFormatAlignmentService();
        }

        private void BuildDaily(DateTime date, List<BALicWinLossDaily> dailyList)
        {
            _currentRow++;

            BuildHeaderSection(_currentRow, date.ToString("MM/dd/yyyy"));

            this._currentRow += 2;

            BuildTableHeader(this._currentRow++);

            var liveList = GetRows(dailyList, true);
            if (liveList.Count > 0)
            {
                var beginRowLive = this._currentRow;
                var licenseeList = GetLicenseeName(liveList);
                foreach (var licensee in licenseeList)
                {
                    this._currentRow = BuildTableBobyForLicensee(this._currentRow, licensee, liveList);
                }

                BuildIndicatorSection(Resource.Live, Color.FromArgb(221, 217, 196), beginRowLive, this._currentRow - 1);

                BuildTotal(this._currentRow++, liveList);
            }

            var nonLiveList = GetRows(dailyList, false);
            if (nonLiveList.Count > 0)
            {
                var beginRowLive = this._currentRow;
                var licenseeList = GetLicenseeName(nonLiveList);
                foreach (var licensee in licenseeList)
                {
                    this._currentRow = BuildTableBobyForLicensee(this._currentRow, licensee, nonLiveList);
                }

                BuildIndicatorSection(Resource.NonLive, Color.FromArgb(118, 147, 60), beginRowLive, this._currentRow - 1);

                BuildTotal(this._currentRow++, nonLiveList);
            }

            BuildGrandTotal(this._currentRow++, dailyList);
        }

        private void BuildIndicatorSection(string sectionName, Color backgroundColor, int rowBegin, int rowEnd)
        {
            var cellStyle = _customCellStyleService.CreateCenterTextStyle(backgroundColor, _fontService.RegularFont);
            this._cell = new Cell(0, rowBegin, 0, rowEnd, sectionName);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, _cell, cellStyle);
        }

        private void BuildMonthYearToDate(string sectionName, List<BALicWinLossDaily> list)
        {
            _currentRow += 3;

            BuildHeaderSection(_currentRow, sectionName);

            _currentRow += 2;

            BuildTableHeader(this._currentRow++);

            var licenseeList = GetLicenseeName(list);
            foreach (var licensee in licenseeList)
            {
                this._currentRow = BuildTableBobyForLicensee(this._currentRow, licensee, list);
            }

            BuildTotal(this._currentRow++, list);
        }

        private List<string> GetLicenseeName(List<BALicWinLossDaily> winLossList)
        {
            var licenseeNameList = new List<string>();
            foreach (var row in winLossList)
            {
                if (!licenseeNameList.Contains(row.LicenseeName))
                {
                    licenseeNameList.Add(row.LicenseeName);
                }
            }

            return licenseeNameList;
        }

        private List<BALicWinLossDaily> GetRows(List<BALicWinLossDaily> listBALicWL, bool liveIndicator)
        {
            var _listBALicWL = new List<BALicWinLossDaily>();
            foreach (var row in listBALicWL)
            {
                if (row.LiveIndicator == liveIndicator)
                {
                    _listBALicWL.Add(row);
                }
            }

            return _listBALicWL;
        }

        private void BuildTitle()
        {
            var borderInformation = new BorderInformation((short)BorderStyle.None, (short)BorderStyle.None, (short)BorderStyle.None, (short)BorderStyle.None, Color.White, Color.White, Color.White, Color.White);
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.White);            
            var cellStyle = this._customCellStyleService.CreateCellStyle(borderInformation, backgroundInformation, _alignmentService.CenterAlignment,  _fontService.CreateBoldFont(_headerFont));
            this._cell = new Cell(0, this._currentRow, 8, this._currentRow + 1, Resource.Title);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);
            this._currentRow++;
        }

        private void BuildHeaderSection(int rowIndex, string sectionName)
        {
            var cellStyle = this._customCellStyleService.CreateCenterTextStyle(Color.FromArgb(221, 217, 196), _fontService.CreateBoldFont(_headerFont));
            this._cell = new Cell(0, rowIndex, 3, rowIndex, sectionName);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);
        }

        private void BuildTableHeader(int rowIndex)
        {
            var columnNameList = new List<string> { Resource.Licensee, Resource.Currency, Resource.Turnover, Resource.TotalTrans, Resource.CustomerWL, Resource.LicenseeComm, Resource.AmountDueBA, Resource.Margin };
            var cellStyle = this._customCellStyleService.CreateCenterTextStyle(Color.FromArgb(218, 238, 243), _fontService.BlackBoldFont);

            for (var index = 1; index <= columnNameList.Count; index++)
            {
                this._cell = new Cell(index, rowIndex, index, rowIndex, columnNameList[index - 1]);
                NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);
            }
        }

        private int BuildTableBobyForLicensee(int rowIndex, string licenseeName, List<BALicWinLossDaily> list)
        {
            var cellStyleOfDateTime = this._customCellStyleService.CreateCenterTextStyle(Color.White, _fontService.RegularFont);

            var cellStyleOfNumber = this._customCellStyleService.CreateDecimalNumberStyle(Color.White, _fontService.RegularFont);

            var cellStyleOfCurrency = this._customCellStyleService.CreateLeftTextStyle(Color.White, _fontService.RegularFont);

            double turnOverRM_SubTotal = 0;
            double betCount_SubTotal = 0;
            double winlostRM_SubTotal = 0;
            double commRM_SubTotal = 0;
            double amtDueToBARM_SubTotal = 0;
            var percentStyle = _bodyCellStyleService.PercentageStyle;

            int beginRow = rowIndex;
            foreach (var item in list)
            {
                if (item.LicenseeName == licenseeName)
                {
                    var columnIndex = 1;
                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.LicenseeName);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfDateTime);

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.CurrencyName);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfCurrency);

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.TurnOverRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
                    turnOverRM_SubTotal += item.TurnOverRM;

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.BetCount);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
                    betCount_SubTotal += item.BetCount;

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.WinlostRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
                    winlostRM_SubTotal += item.WinlostRM;

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.CommRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
                    commRM_SubTotal += item.CommRM;

                    this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, item.AmtDueToBARM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
                    amtDueToBARM_SubTotal += item.AmtDueToBARM;

                    if (item.TurnOverRM == 0)
                    {
                        this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, NotAvailableStr);
                        NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfCurrency);
                    }
                    else
                    {
                        var marginSubTotal = CalculateMargin(item.WinlostRM, item.TurnOverRM);
                        this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, marginSubTotal);
                        NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, percentStyle);
                    }
                    rowIndex++;
                }
            }

            BuildSubTotal(rowIndex++, licenseeName, turnOverRM_SubTotal, betCount_SubTotal, winlostRM_SubTotal, commRM_SubTotal, amtDueToBARM_SubTotal);

            this._sheet.AddMergedRegion(new CellRangeAddress(beginRow, rowIndex - 1, 1, 1));

            return rowIndex;
        }

        private void BuildSubTotal(int rowIndex, string licenseeName, double turnOverRM_Total, double betCount_Total, double winlostRM_Total, double commRM_Total, double amtDueToBARM_Total)
        {
            var cellStyleOfLicenseeName = _customCellStyleService.CreateCenterTextStyle(Color.White, null);

            var cellStyleOfSubTotal = this._customCellStyleService.CreateCenterTextStyle(Color.FromArgb(196, 189, 151), _fontService.BlackBoldFont);

            var cellStyleOfSubTotalNumber = this._customCellStyleService.CreateDecimalNumberStyle(Color.FromArgb(196, 189, 151), _fontService.BlackBoldFont);

            var cellStyleOfPercent = this._customCellStyleService.CreatePercentageStyle(Color.FromArgb(196, 189, 151), _fontService.BlackBoldFont);

            var columnIndex = 1;

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, licenseeName);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfLicenseeName);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, Resource.SubTotal);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotal);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, turnOverRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotalNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, betCount_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotalNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, winlostRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotalNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, commRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotalNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, amtDueToBARM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotalNumber);

            if (turnOverRM_Total == 0)
            {
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, NotAvailableStr);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfSubTotal);
            }
            else
            {
                var marginSubTotal = CalculateMargin(winlostRM_Total, turnOverRM_Total);
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, marginSubTotal);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfPercent);
            }
        }

        private void BuildTotal(int rowIndex, List<BALicWinLossDaily> list)
        {
            var cellStyleOfTotal = this._customCellStyleService.CreateCenterTextStyle(Color.FromArgb(33, 89, 103), _fontService.WhiteBoldFont);

            var cellStyleOfNumber = this._customCellStyleService.CreateDecimalNumberStyle(Color.FromArgb(33, 89, 103), _fontService.WhiteBoldFont);

            var cellStyleOfPercent = this._customCellStyleService.CreatePercentageStyle(Color.FromArgb(33, 89, 103), _fontService.BlackBoldFont);

            var columnIndex = 1;
            this._cell = new Cell(columnIndex, rowIndex, ++columnIndex, rowIndex, Resource.Total);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, _cell, cellStyleOfTotal);

            columnIndex++;

            var turnOverRM = list.Sum(wl => wl.TurnOverRM);
            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, turnOverRM);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(wl => wl.BetCount));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            var winlossRM = list.Sum(wl => wl.WinlostRM);
            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, winlossRM);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(wl => wl.CommRM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(wl => wl.AmtDueToBARM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            if (turnOverRM == 0)
            {
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, NotAvailableStr);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfTotal);
            }
            else
            {
                var marginSubTotal = CalculateMargin(winlossRM, turnOverRM);
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, marginSubTotal);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfPercent);
            }
        }

        private void BuildGrandTotal(int rowIndex, List<BALicWinLossDaily> list)
        {
            var cellStyleOfTotal = this._customCellStyleService.CreateCenterTextStyle(Color.FromArgb(184, 204, 228), _fontService.BlackBoldFont);

            var cellStyleOfNumber = this._customCellStyleService.CreateDecimalNumberStyle(Color.FromArgb(184, 204, 228), _fontService.BlackBoldFont);

            var cellStyleOfPercent = this._customCellStyleService.CreatePercentageStyle(Color.FromArgb(184, 204, 228), _fontService.BlackBoldFont);

            var columnIndex = 0;
            this._cell = new Cell(columnIndex, rowIndex, columnIndex + 2, rowIndex, Resource.GrandTotal);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, _cell, cellStyleOfTotal);

            columnIndex += 2;

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, Resource.GrandTotal);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfTotal);

            var turnOverRM = list.Sum(item => item.TurnOverRM);
            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, turnOverRM);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(item => item.BetCount));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            var winlossRM = list.Sum(item => item.WinlostRM);
            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, winlossRM);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(item => item.CommRM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, list.Sum(item => item.AmtDueToBARM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfNumber);

            
            if (turnOverRM == 0)
            {
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, NotAvailableStr);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfTotal);
            }
            else
            {
                var margin = CalculateMargin(winlossRM, turnOverRM);
                this._cell = new Cell(columnIndex, rowIndex, columnIndex++, rowIndex, margin);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, _cell, cellStyleOfPercent);
            }
            
        }

        private double CalculateMargin(double winloss, double turnover)
        {
            double margin = 0;
            margin = Convert.ToDouble(-(winloss / turnover));

            return margin;
        }
    }
}
