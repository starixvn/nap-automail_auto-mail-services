// -----------------------------------------------------------------------
// <copyright file="OverallWinLossProductsExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Resources;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OverallWinLossSportbook3rdPartyExcel
    {
        private IOverallWinLossProducts _productsWinLoss;
        private List<Product> _products;

        private Color _colorHeader;
        private Color _colorYellowBG;
        private Color _colorBlueBG;
        private Color _colorOrangeBG;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;

        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;

        private XSSFFont _regularFont;
        private XSSFFont _regularBoldFont;
        private XSSFFont _redBoldFont;      

        private XSSFCellStyle _boldCenterRedColorYellowBGStyle;
        private XSSFCellStyle _boldCenterRedColorBlueBGStyle;
        private XSSFCellStyle _boldCenterRedColorOrangeBGStyle;
        private XSSFCellStyle _boldCenterStyle;

        private XSSFCellStyle _leftBoldBlueBGStyle;
        private XSSFCellStyle _leftWhiteBGStyle;        
        private XSSFCellStyle _leftStyle;

        private XSSFCellStyle _decimalTodayPositiveBoldColorBlueBGStyle; 
        private XSSFCellStyle _decimalTodayPositiveColorWhiteBGStyle;
        private XSSFCellStyle _decimalTodayBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalTodayColorWhiteBGStyle;        
        private XSSFCellStyle _decimalTodayStyle;

        private XSSFCellStyle _decimalMonthToDatePositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalMonthToDatePositiveColorWhiteBGStyle;
        private XSSFCellStyle _decimalMonthToDateBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalMonthToDateColorWhiteBGStyle;        
        private XSSFCellStyle _decimalMonthToDateStyle;

        private XSSFCellStyle _decimalLastMonthPositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalLastMonthPositiveColorWhiteBGStyle;
        private XSSFCellStyle _decimalLastMonthBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalLastMonthColorWhiteBGStyle;        
        private XSSFCellStyle _decimalLastMonthStyle;

        private XSSFCellStyle _decimalYearToDatePositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalYearToDatePositiveColorWhiteBGStyle;
        private XSSFCellStyle _decimalYearToDateBoldColorBlueBGStyle;
        private XSSFCellStyle _decimalYearToDateColorWhiteBGStyle;        
        private XSSFCellStyle _decimalYearToDateStyle;

        private XSSFCellStyle _percentTodayPositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _percentTodayPositiveColorWhiteBGStyle;
        private XSSFCellStyle _percentTodayBoldColorBlueBGStyle;
        private XSSFCellStyle _percentTodayColorWhiteBGStyle;
        private XSSFCellStyle _percentTodayStyle;

        private XSSFCellStyle _percentMonthToDatePositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _percentMonthToDatePositiveColorWhiteBGStyle;
        private XSSFCellStyle _percentMonthToDateBoldColorBlueBGStyle;
        private XSSFCellStyle _percentMonthToDateColorWhiteBGStyle;
        private XSSFCellStyle _percentMonthToDateStyle;

        private XSSFCellStyle _percentLastMonthPositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _percentLastMonthPositiveColorWhiteBGStyle;
        private XSSFCellStyle _percentLastMonthBoldColorBlueBGStyle;
        private XSSFCellStyle _percentLastMonthColorWhiteBGStyle;
        private XSSFCellStyle _percentLastMonthStyle;

        private XSSFCellStyle _percentYearToDatePositiveBoldColorBlueBGStyle;
        private XSSFCellStyle _percentYearToDatePositiveColorWhiteBGStyle;
        private XSSFCellStyle _percentYearToDateBoldColorBlueBGStyle;
        private XSSFCellStyle _percentYearToDateColorWhiteBGStyle;
        private XSSFCellStyle _percentYearToDateStyle;

        private XSSFCellStyle _numberBoldColorBlueBGStyle;
        private XSSFCellStyle _numberNormalBGStyle;

        private XSSFCellStyle _todayStyle;
        private XSSFCellStyle _monthToDateStyle;
        private XSSFCellStyle _lastMonthStyle;
        private XSSFCellStyle _yearToDateStyle;

        private XSSFCellStyle _notAvailableTextRightStyle;
        private XSSFCellStyle _notAvailableTextRightBoldColorBlueBGStyle;

        public OverallWinLossSportbook3rdPartyExcel(XSSFWorkbook workbook)
        {
            _workbook = workbook;

            CreateColor();
            InitialSetting();
            InitialCellStyle();
        }

        private void InitialSetting()
        {
            _productsWinLoss = new OverallWinLossProducts();
            _products = new List<Product>();

            _cell = new Cell();

            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _regularBoldFont = _fontService.RegularFont;
            _regularBoldFont.IsBold = true;

            _redBoldFont = _fontService.RegularFont;
            _redBoldFont.IsBold = true;
            _redBoldFont.SetColor(new XSSFColor(Color.Red));

            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();            
        }

        private void InitialCellStyle()
        {
            _boldCenterRedColorYellowBGStyle = _customCellStyleService.CreateCenterTextStyle(_colorYellowBG, _redBoldFont);
            _boldCenterRedColorBlueBGStyle = _customCellStyleService.CreateCenterTextStyle(_colorBlueBG, _redBoldFont);
            _boldCenterRedColorOrangeBGStyle = _customCellStyleService.CreateCenterTextStyle(_colorOrangeBG, _redBoldFont);

            _leftBoldBlueBGStyle = _customCellStyleService.CreateLeftTextStyle(_colorBlueBG, _regularBoldFont);
            _leftWhiteBGStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _regularFont);

            _decimalTodayPositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalTodayPositiveColorWhiteBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(Color.White, _regularFont);
            _decimalTodayBoldColorBlueBGStyle = _customCellStyleService.CreateDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalTodayColorWhiteBGStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);

            _decimalMonthToDatePositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalMonthToDatePositiveColorWhiteBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(Color.White, _regularFont);
            _decimalMonthToDateBoldColorBlueBGStyle = _customCellStyleService.CreateDecimalNumberStyle(_colorBlueBG, _regularBoldFont); 
            _decimalMonthToDateColorWhiteBGStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);

            _decimalLastMonthPositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalLastMonthPositiveColorWhiteBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(Color.White, _regularFont);
            _decimalLastMonthBoldColorBlueBGStyle = _customCellStyleService.CreateDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalLastMonthColorWhiteBGStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);

            _decimalYearToDatePositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalYearToDatePositiveColorWhiteBGStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(Color.White, _regularFont);
            _decimalYearToDateBoldColorBlueBGStyle = _customCellStyleService.CreateDecimalNumberStyle(_colorBlueBG, _regularBoldFont);
            _decimalYearToDateColorWhiteBGStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);

            _percentTodayPositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositivePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentTodayPositiveColorWhiteBGStyle = _customCellStyleService.CreatePositivePercentageStyle(Color.White, _regularFont);
            _percentTodayBoldColorBlueBGStyle = _customCellStyleService.CreatePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentTodayColorWhiteBGStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _regularFont);

            _percentMonthToDatePositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositivePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentMonthToDatePositiveColorWhiteBGStyle = _customCellStyleService.CreatePositivePercentageStyle(Color.White, _regularFont);
            _percentMonthToDateBoldColorBlueBGStyle = _customCellStyleService.CreatePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentMonthToDateColorWhiteBGStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _regularFont);

            _percentLastMonthPositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositivePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentLastMonthPositiveColorWhiteBGStyle = _customCellStyleService.CreatePositivePercentageStyle(Color.White, _regularFont);
            _percentLastMonthBoldColorBlueBGStyle = _customCellStyleService.CreatePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentLastMonthColorWhiteBGStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _regularFont);

            _percentYearToDatePositiveBoldColorBlueBGStyle = _customCellStyleService.CreatePositivePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentYearToDatePositiveColorWhiteBGStyle = _customCellStyleService.CreatePositivePercentageStyle(Color.White, _regularFont);
            _percentYearToDateBoldColorBlueBGStyle = _customCellStyleService.CreatePercentageStyle(_colorBlueBG, _regularBoldFont);
            _percentYearToDateColorWhiteBGStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _regularFont);

            _numberBoldColorBlueBGStyle = _customCellStyleService.CreateRightNumberStyle(_colorBlueBG, _regularBoldFont);
            _numberNormalBGStyle = _customCellStyleService.CreateRightNumberStyle(Color.White, _regularFont);

            _notAvailableTextRightStyle = _customCellStyleService.CreateRightTextStyle(Color.White, _regularFont);
            _notAvailableTextRightBoldColorBlueBGStyle = _customCellStyleService.CreateRightTextStyle(_colorBlueBG, _regularBoldFont);
        }

        private void CreateColor()
        {
            _colorHeader = Color.FromArgb(60, 94, 165);
            _colorYellowBG = Color.FromArgb(255, 255, 0);
            _colorBlueBG = Color.FromArgb(220, 230, 241);
            _colorOrangeBG = Color.FromArgb(253, 233, 217);
        }

        public void ExportExcel(List<Product> sportbook3rdParty, OverallWinLossProductsCriteria criteria)
        {
            _products = sportbook3rdParty;

            CreateSheet();
            InitialColumnWidth();

            ExportReportHeader(criteria);
            ExportContent(sportbook3rdParty);            
        }

        private void CreateSheet()
        {
            string sheetName = Resources.WinLossByProducts_SheetName3rd;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(0, 4);
        }

        private void InitialColumnWidth()
        {
            for (int i = 0; i < 6; i++)
            {
                _workSheet.SetColumnWidth(i, 200 * NPOIConstants.FontWidth);
            }
        }

        private void ExportReportHeader(OverallWinLossProductsCriteria criteria)
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_colorHeader, _fontService.WhiteBoldFont);
            string reportTile = string.Format(Resources.WinLossByProducts_ReportTitle3rd, criteria.rptdate.ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 5;
            _cell.EndRow = _cell.CurrentRow + 1;
            _cell.CellValue = reportTile;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();

            _cell.CurrentRow += 3;
            _cell.EndRow = _cell.CurrentRow;
            _cell.CellValue = Labels.Product;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Betting_Statistics;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Today;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Month_To_Date;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Last_Month;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = Labels.Year_To_Date;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportContent(List<Product> sportbook3rdParty)
        {
            bool isIncreaseRowIndex = false;
            foreach (var product in sportbook3rdParty)
            {
                _cell.EndRow = _cell.CurrentRow + product.Rows.Count - 1;
                _cell.CellValue = product.ProductName;

                SetProductAndTotalStyle(product);

                NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _boldCenterStyle);

                foreach (var winLoss in product.Rows)
                {
                    SetOthersStyle(product.ProductName, winLoss);

                    _cell.EndRow = _cell.CurrentRow;
                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = winLoss.Name;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _leftStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = winLoss.IsMarginTodayNA ? Labels.N_A : (object)winLoss.Today;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _todayStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = winLoss.IsMarginMonthToDateNA ? Labels.N_A : (object)winLoss.MonthToDate;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _monthToDateStyle); 

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = winLoss.IsMarginLastMonthNA ? Labels.N_A : (object)winLoss.LastMonth;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _lastMonthStyle); 

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = winLoss.IsMarginYearToDateNA ? Labels.N_A : (object)winLoss.YearToDate;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _yearToDateStyle); 

                    _cell.IncreaseRowIndex();
                    _cell.ResetColumnIndex();
                }

                if (!isIncreaseRowIndex)
                {
                    isIncreaseRowIndex = true;
                    _cell.IncreaseRowIndex();
                }
            }

            _cell.IncreaseRowIndex();
        }

        private void SetProductAndTotalStyle(Product product)
        {
            if (IsProductAndTotal(product.ProductName))
            {
                _boldCenterStyle = _boldCenterRedColorOrangeBGStyle;
                _leftStyle = _leftBoldBlueBGStyle;
            }            
            else
            {
                _boldCenterStyle = _boldCenterRedColorBlueBGStyle;
                _leftStyle = _leftWhiteBGStyle;
            }
        }

        private void SetOthersStyle(string productName, WinLoss winLoss)
        {
            if (IsProductAndTotal(productName))
            {
                _boldCenterStyle = _boldCenterRedColorYellowBGStyle;
                _leftStyle = _leftBoldBlueBGStyle;

                _decimalTodayStyle = IsRedColorFont(winLoss.Name) && winLoss.Today > 0 ? _decimalTodayPositiveBoldColorBlueBGStyle : _decimalTodayBoldColorBlueBGStyle;
                _decimalMonthToDateStyle = IsRedColorFont(winLoss.Name) && winLoss.MonthToDate > 0 ? _decimalMonthToDatePositiveBoldColorBlueBGStyle : _decimalMonthToDateBoldColorBlueBGStyle;
                _decimalLastMonthStyle = IsRedColorFont(winLoss.Name) && winLoss.LastMonth > 0 ? _decimalLastMonthPositiveBoldColorBlueBGStyle : _decimalLastMonthBoldColorBlueBGStyle;
                _decimalYearToDateStyle = IsRedColorFont(winLoss.Name) && winLoss.YearToDate > 0 ? _decimalYearToDatePositiveBoldColorBlueBGStyle : _decimalYearToDateBoldColorBlueBGStyle;

                _percentTodayStyle = IsMargin(winLoss.Name) && winLoss.Today > 0 ? _percentTodayPositiveBoldColorBlueBGStyle : _percentTodayBoldColorBlueBGStyle;
                _percentMonthToDateStyle = IsMargin(winLoss.Name) && winLoss.MonthToDate > 0 ? _percentMonthToDatePositiveBoldColorBlueBGStyle : _percentMonthToDateBoldColorBlueBGStyle;
                _percentLastMonthStyle = IsMargin(winLoss.Name) && winLoss.LastMonth > 0 ? _percentLastMonthPositiveBoldColorBlueBGStyle : _percentLastMonthBoldColorBlueBGStyle;
                _percentYearToDateStyle = IsMargin(winLoss.Name) && winLoss.YearToDate > 0 ? _percentYearToDatePositiveBoldColorBlueBGStyle : _percentYearToDateBoldColorBlueBGStyle;
            }            
            else
            {
                _boldCenterStyle = _boldCenterRedColorBlueBGStyle;
                _leftStyle = _leftWhiteBGStyle;

                _decimalTodayStyle = IsRedColorFont(winLoss.Name) && winLoss.Today > 0 ? _decimalTodayPositiveColorWhiteBGStyle : _decimalTodayColorWhiteBGStyle;
                _decimalMonthToDateStyle = IsRedColorFont(winLoss.Name) && winLoss.MonthToDate > 0 ? _decimalMonthToDatePositiveColorWhiteBGStyle : _decimalMonthToDateColorWhiteBGStyle;
                _decimalLastMonthStyle = IsRedColorFont(winLoss.Name) && winLoss.LastMonth > 0 ? _decimalLastMonthPositiveColorWhiteBGStyle : _decimalLastMonthColorWhiteBGStyle;
                _decimalYearToDateStyle = IsRedColorFont(winLoss.Name) && winLoss.YearToDate > 0 ? _decimalYearToDatePositiveColorWhiteBGStyle : _decimalYearToDateColorWhiteBGStyle;

                _percentTodayStyle = IsMargin(winLoss.Name) && winLoss.Today > 0 ? _percentTodayPositiveColorWhiteBGStyle : _percentTodayColorWhiteBGStyle;
                _percentMonthToDateStyle = IsMargin(winLoss.Name) && winLoss.MonthToDate > 0 ? _percentMonthToDatePositiveColorWhiteBGStyle : _percentMonthToDateColorWhiteBGStyle;
                _percentLastMonthStyle = IsMargin(winLoss.Name) && winLoss.LastMonth > 0 ? _percentLastMonthPositiveColorWhiteBGStyle : _percentLastMonthColorWhiteBGStyle;
                _percentYearToDateStyle = IsMargin(winLoss.Name) && winLoss.YearToDate > 0 ? _percentYearToDatePositiveColorWhiteBGStyle : _percentYearToDateColorWhiteBGStyle;
            }

            _todayStyle = IsMargin(winLoss.Name) ? _percentTodayStyle : _decimalTodayStyle;
            _monthToDateStyle = IsMargin(winLoss.Name) ? _percentMonthToDateStyle : _decimalMonthToDateStyle;
            _lastMonthStyle = IsMargin(winLoss.Name) ? _percentLastMonthStyle : _decimalLastMonthStyle;
            _yearToDateStyle = IsMargin(winLoss.Name) ? _percentYearToDateStyle : _decimalYearToDateStyle;

            _todayStyle = winLoss.IsMarginTodayNA ? (IsProductAndTotal(productName) ? _notAvailableTextRightBoldColorBlueBGStyle : _notAvailableTextRightStyle) : GetStyleNumberOfBets(productName, winLoss.Name, _todayStyle);
            _monthToDateStyle = winLoss.IsMarginMonthToDateNA ? (IsProductAndTotal(productName) ? _notAvailableTextRightBoldColorBlueBGStyle : _notAvailableTextRightStyle) : GetStyleNumberOfBets(productName, winLoss.Name, _monthToDateStyle);
            _lastMonthStyle = winLoss.IsMarginLastMonthNA ? (IsProductAndTotal(productName) ? _notAvailableTextRightBoldColorBlueBGStyle : _notAvailableTextRightStyle) : GetStyleNumberOfBets(productName, winLoss.Name, _lastMonthStyle);
            _yearToDateStyle = winLoss.IsMarginYearToDateNA ? (IsProductAndTotal(productName) ? _notAvailableTextRightBoldColorBlueBGStyle : _notAvailableTextRightStyle) : GetStyleNumberOfBets(productName, winLoss.Name, _yearToDateStyle);
        }

        private bool IsProductAndTotal(string productName)
        {
            bool result = productName.ToLower().Equals(Labels.Sportsbook_Live.ToLower())
                || productName.ToLower().Equals(Labels.Sportsbook_Non_Live.ToLower());

            return result;
        }

        private bool IsRedColorFont(string name)
        {
            bool isRed = false;
            if (name.ToLower().Equals(Labels.Win_Loss.ToLower())
                || name.ToLower().Equals(Labels.Margin.ToLower()))
            {
                isRed = true;
            }

            return isRed;
        }

        private bool IsMargin(string name)
        {
            bool result = name.ToLower().Equals(Labels.Margin.ToLower());

            return result;
        }

        private XSSFCellStyle GetStyleNumberOfBets(string productName, string rowWinLossName, XSSFCellStyle style)
        {
            if (rowWinLossName.ToLower().Equals(Labels.Number_Of_bets_WL.ToLower()))
            {
                if (IsProductAndTotal(productName))
                {
                    style = _numberBoldColorBlueBGStyle;
                }
                else
                {
                    style = _numberNormalBGStyle;
                }
            }

            return style;
        }
    }
}
