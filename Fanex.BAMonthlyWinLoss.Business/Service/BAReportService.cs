namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using Fanex.BAMonthlyWinLoss.Business.Resources;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Model;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiBase;
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
        private const int ColumnWidth = 18;
        private const int StartColumn = 1;

        private AlignmentInformation _alignmentCenter = new AlignmentInformation()
        {
            HorizontalAlignment = (int)HorizontalAlignment.Center,
            VerticalAlignment = (int)VerticalAlignment.Center
        };

        private AlignmentInformation _alignmentLeft = new AlignmentInformation()
        {
            HorizontalAlignment = (int)HorizontalAlignment.Left,
            VerticalAlignment = (int)VerticalAlignment.Center
        };

        private AlignmentInformation _alignmentRight = new AlignmentInformation()
        {
            HorizontalAlignment = (int)HorizontalAlignment.Right,
            VerticalAlignment = (int)VerticalAlignment.Center
        };

        private XSSFWorkbook _workbook;
        private XSSFSheet _sheet;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private int _currentRow = 1;
        private BorderInformation _borderInformation;
        private IDataFormat _dataFormat;
        private XSSFFont _font;

        public string FileName { get; set; }

        public string DirectoryOfFileName { get; set; }

        public void GenerateReport()
        {
            var service = new BetAggregatorService();
            var criteria = new GetByDateCriteria();
            var winLossList = service.GetBALicWinlossMonthly(criteria);
            var winLossDate = Convert.ToDateTime(criteria.FromDate);
            this.FileName = string.Format(Label.FileName, winLossDate.ToString("MMM_yyyy"));
            this.DirectoryOfFileName = AppSettings.DataDirectoryWithYearMonth + @"\" + this.FileName;

            this._workbook = new XSSFWorkbook();

            var licenseeList = GetLicenseeName(winLossList);
            foreach (var licenseeName in licenseeList)
            {
                var winLossByLicensee = GetWinLossByLicenseeName(winLossList, licenseeName);

                InitSheet(licenseeName);

                BuildTitle(winLossDate);

                BuildLicenseeName(licenseeName);

                BuildTableHeader();

                BuildTableBody(winLossByLicensee);

                BuildTableTotal(winLossByLicensee);
            }

            if (licenseeList.Count == 0)
            {
                string licenseeName = "There is no data";
                InitSheet(licenseeName);

                BuildTitle(winLossDate);

                BuildLicenseeName(licenseeName);
            }

            SaveFile.SaveExcelFile(AppSettings.DataDirectory, AppSettings.DataDirectoryWithYearMonth, this.FileName, this._workbook, AppSettings.ExcelPasswordProtected);
        }

        private void InitSheet(string licenseeName)
        {
            _sheet = this._workbook.CreateSheet(licenseeName) as XSSFSheet;
            _sheet.DefaultColumnWidth = 15;
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _fontService = new NpoiNewFormatFontService(_workbook);
            _font = _fontService.RegularFont;
            _cell = new Cell();
            _currentRow = 1;
            _dataFormat = this._workbook.CreateDataFormat();
            _borderInformation = new BorderInformation(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.FromArgb(124, 152, 164),
                Color.FromArgb(124, 152, 164),
                Color.FromArgb(124, 152, 164),
                Color.FromArgb(124, 152, 164));
        }

        private void BuildTitle(DateTime winLossDate)
        {
            var headerFont = new FontInformation(NPOIConstants.FontName, 14, Color.Black);
            var cellStyle = this._customCellStyleService.CreateCenterTextStyle(Color.White, _fontService.CreateBoldFont(headerFont));
            cellStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.White));
            cellStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.White));
            cellStyle.SetBorderColor(BorderSide.LEFT, new XSSFColor(Color.White));
            cellStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.White));
            this._cell = new Cell(StartColumn, _currentRow, 7, _currentRow++, Label.Title);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);

            this._cell = new Cell(StartColumn, _currentRow, 7, _currentRow++, winLossDate.ToString("MMM yyyy"));
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void BuildLicenseeName(string licenseeName)
        {
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.FromArgb(255, 102, 0));
            _cell = new Cell(StartColumn, this._currentRow, StartColumn + 1, this._currentRow, licenseeName);
            var licenseeFont = new FontInformation(NPOIConstants.FontName, 14, Color.White);

            var cellStyle = this._customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentCenter, _fontService.CreateBoldFont(licenseeFont));
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyle);

            this._currentRow += 2;
        }

        private void BuildTableHeader()
        {
            var columnNameList = new List<string> { Label.Date, Label.Currency, Label.Turnover, Label.TotalTrans, Label.CustomerWL, Label.LicenseeComm, Label.AmountDueBA };
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.FromArgb(218, 238, 243));
            var cellStyle = this._customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentCenter, _fontService.BlackBoldFont);

            var index = StartColumn;
            foreach (var name in columnNameList)
            {
                _cell = new Cell(index, this._currentRow, index++, this._currentRow, name);
                NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyle);
            }

            this._currentRow++;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void BuildTableBody(List<BALicWinlossDaily> winLossList)
        {
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.White);
            var cellStyleOfDateTime = this._customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentCenter, _font);

            var cellStyleOfNumber = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentRight, _font);
            cellStyleOfNumber.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            var cellStyleOfCurrency = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentLeft, _font);

            var listDate = GetDistinDate(winLossList);
            foreach (var date in listDate)
            {
                var listRowsByDate = GetRowsByDate(winLossList, date);
                int beginRow = this._currentRow;
                foreach (var item in listRowsByDate)
                {
                    var columnIndex = StartColumn;

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.WinlostDate.ToString("MM/dd"));
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfDateTime);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.CurrencyName);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfCurrency);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.TurnOverRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.BetCount);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.WinlostRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.CommRM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

                    _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, item.AmtDueToBARM);
                    NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

                    _currentRow++;
                }

                double turnOverRM_SubTotal = listRowsByDate.Sum(a => a.TurnOverRM);
                double betCount_SubTotal = listRowsByDate.Sum(a => a.BetCount);
                double winlostRM_SubTotal = listRowsByDate.Sum(a => a.WinlostRM);
                double commRM_SubTotal = listRowsByDate.Sum(a => a.CommRM);
                double amtDueToBARM_SubTotal = listRowsByDate.Sum(a => a.AmtDueToBARM);

                BuildSubTotal(date, turnOverRM_SubTotal, betCount_SubTotal, winlostRM_SubTotal, commRM_SubTotal, amtDueToBARM_SubTotal);
                this._sheet.AddMergedRegion(new CellRangeAddress(beginRow, this._currentRow - 1, StartColumn, StartColumn));
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void BuildSubTotal(DateTime date, double turnOverRM_Total, double betCount_Total, double winlostRM_Total, double commRM_Total, double amtDueToBARM_Total)
        {
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.FromArgb(255, 255, 153));
            var backgroundDateTime = new BackgroundInformation((int)FillPattern.SolidForeground, Color.White);
            var cellStyleOfSubTotal = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentCenter, _fontService.BlackBoldFont);
            var cellStyleOfDateTime = this._customCellStyleService.CreateCellStyle(_borderInformation, backgroundDateTime, _alignmentCenter, _font);

            var cellStyleOfSubTotalNumber = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentRight, _fontService.BlackBoldFont);
            cellStyleOfSubTotalNumber.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            var columnIndex = StartColumn;

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, date.ToString("MM/dd"));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfDateTime);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, Label.SubTotal);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotal);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, turnOverRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotalNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, betCount_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotalNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winlostRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotalNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, commRM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotalNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, amtDueToBARM_Total);
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfSubTotalNumber);

            _currentRow++;
        }

        private void BuildTableTotal(List<BALicWinlossDaily> winLossList)
        {
            var backgroundInformation = new BackgroundInformation((int)FillPattern.SolidForeground, Color.FromArgb(0, 128, 128));
            var cellStyleOfTotal = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentCenter, _fontService.WhiteBoldFont);
            var cellStyleOfNumber = _customCellStyleService.CreateCellStyle(_borderInformation, backgroundInformation, _alignmentRight, _fontService.WhiteBoldFont);
            cellStyleOfNumber.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            var columnIndex = StartColumn;
            _cell = new Cell(columnIndex, this._currentRow, columnIndex + 1, this._currentRow, Label.Total);
            NpoiNewFormatExcelExtension.RenderMergeCell(this._sheet, this._cell, cellStyleOfTotal);

            columnIndex += 2;

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winLossList.Sum(i => i.TurnOverRM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winLossList.Sum(i => i.BetCount));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winLossList.Sum(i => i.WinlostRM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winLossList.Sum(i => i.CommRM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);

            _cell = new Cell(columnIndex, this._currentRow, columnIndex++, this._currentRow, winLossList.Sum(i => i.AmtDueToBARM));
            NpoiNewFormatExcelExtension.RenderCell(this._sheet, this._cell, cellStyleOfNumber);
        }

        private List<DateTime> GetDistinDate(List<BALicWinlossDaily> winLossList)
        {
            var listDate = new List<DateTime>();
            foreach (var row in winLossList)
            {
                if (!listDate.Contains(row.WinlostDate))
                {
                    listDate.Add(row.WinlostDate);
                }
            }

            return listDate;
        }

        private List<BALicWinlossDaily> GetRowsByDate(List<BALicWinlossDaily> winLossList, DateTime date)
        {
            var listResult = new List<BALicWinlossDaily>();
            foreach (var row in winLossList)
            {
                if (row.WinlostDate == date)
                {
                    listResult.Add(row);
                }
            }

            return listResult;
        }

        private List<BALicWinlossDaily> GetWinLossByLicenseeName(List<BALicWinlossDaily> winLossList, string licenseeName)
        {
            var listResult = new List<BALicWinlossDaily>();
            foreach (var row in winLossList)
            {
                if (row.LicenseeName == licenseeName)
                {
                    listResult.Add(row);
                }
            }

            return listResult;
        }

        private List<string> GetLicenseeName(List<BALicWinlossDaily> winLossList)
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
    }
}