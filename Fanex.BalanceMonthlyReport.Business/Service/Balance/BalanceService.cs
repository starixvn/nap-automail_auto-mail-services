namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Repository;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Balance Manager class.
    /// </summary>
    public class BalanceService : IBalanceService
    {
        private IBalanceRepository _repository;
        private Color _headerColor;
        private Color _evenColor;
        private Color _oddColor;
        private Color _totalColor;
        private Color _borderColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;
        private XSSFFont _regularFont;
        private BalanceReportBaseCriteria _criteria;

        private List<Exchange> _currencyList;

        private XSSFCellStyle evenCenterTextStyle;
        private XSSFCellStyle evenDecimalNumberStyle;
        private XSSFCellStyle evenLeftTextStyle;

        private XSSFCellStyle oddCenterTextStyle;
        private XSSFCellStyle oddDecimalNumberStyle;
        private XSSFCellStyle oddLeftTextStyle;

        #region Constructors
        public BalanceService(BalanceReportBaseCriteria criteria)
        {
            _repository = new BalanceRepository();
            _criteria = criteria;
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();
            CreateColor();
            CreateSheet();
            InitColumnWidth();
            InitCellStyle();
            _currencyList = _repository.GetCurrencyList();
        }

        public string DirectoryOfFileName
        {
            get;
            set;
        }

        public string FileName { get; set; }        
        #endregion

        /// <summary>
        /// Send mail: Credit term report excel file, super balance report excel file, direct balance excel file.
        /// </summary>
        public void ExportExcelBalance()
        {
            ExportReportTitle();
            ExportReportContent();
            FileName = CreateReportNameWithExtension();
            DirectoryOfFileName = Path.Combine(_criteria.DataDirectoryWithYear, FileName);
            SaveFile.SaveExcelFile(_criteria.DataDirectoryWithYear, FileName, this._workbook, _criteria.ExcelPasswordProtected);
        }

        private void CreateColor()
        {
            _borderColor = Color.FromArgb(124, 152, 164);
            _headerColor = Color.FromArgb(181, 196, 208);
            _oddColor = Color.FromArgb(223, 234, 255);
            _evenColor = Color.White;
            _totalColor = Color.FromArgb(192, 220, 192);
        }

        private void CreateSheet()
        {
            string sheetName = _criteria.Name;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(0, 2);
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 250 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 150 * NPOIConstants.FontWidth);
        }

        private void InitCellStyle()
        {
            evenCenterTextStyle = _customCellStyleService.CreateCenterTextStyle(_evenColor, _fontService.RegularFont);
            evenDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(_evenColor, _fontService.RegularFont);
            evenLeftTextStyle = _customCellStyleService.CreateLeftTextStyle(_evenColor, _fontService.RegularFont);

            oddCenterTextStyle = _customCellStyleService.CreateCenterTextStyle(_oddColor, _fontService.RegularFont);
            oddDecimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(_oddColor, _fontService.RegularFont);
            oddLeftTextStyle = _customCellStyleService.CreateLeftTextStyle(_oddColor, _fontService.RegularFont);
        }

        private string GetCurrencyName(int key)
        {
            string currencyName = string.Empty;
            currencyName = _currencyList.Where(s => s.ExchangeID == key).Select(s => s.Currency).FirstOrDefault();

            return currencyName;
        }

        /// <summary>
        /// Creates the row data.
        /// </summary>
        /// <param name="item">The item balance.</param>
        /// <param name="rowIndex">Index of the row.</param>
        private void CreateRowData(BalanceModel item, int rowIndex)
        {
            _cell.ResetColumnIndex();
            var textCenterStyle = rowIndex % 2 == 0 ? evenCenterTextStyle : oddCenterTextStyle;
            var decimalNumberStyle = rowIndex % 2 == 0 ? evenDecimalNumberStyle : oddDecimalNumberStyle;
            var textLeftStyle = rowIndex % 2 == 0 ? evenLeftTextStyle : oddLeftTextStyle;
            BindData(rowIndex, textCenterStyle);
            BindData(item.Username, textLeftStyle);
            BindData(GetCurrencyName(item.Currency), textCenterStyle);
            BindData(item.Balance, decimalNumberStyle);
            BindData(item.BaseBalance, decimalNumberStyle);
        }

        private void BindData(object dataValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = dataValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
            _cell.IncreaseColumnIndex();
        }

        /// <summary>
        /// Create super balance excel file.
        /// </summary>
        private void ExportReportContent()
        {
            var index = 1;
            double total = 0;

            using (IDataReader reader = _repository.GetBalance(_criteria))
            {
                while (reader.Read())
                {
                    var baseBalanceAmount = reader.GetDouble(4);
                    var balanceAmount = reader.GetDecimal(3);
                    if (baseBalanceAmount != 0 || balanceAmount != 0)
                    {
                        total += baseBalanceAmount;
                        var balance = new BalanceModel
                        {
                            CustID = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Currency = reader.GetInt32(2),
                            Balance = balanceAmount,
                            BaseBalance = baseBalanceAmount
                        };
                        _cell.IncreaseRowIndex();
                        CreateRowData(balance, index);
                        index++;
                    }

                    if (AppSettings.IsTest && index >= 100)
                    {
                        break;
                    }
                }

                reader.Close();
            }

            ExportGrandTotal(total);
        }

        /// <summary>
        /// Exports the grand total.
        /// </summary>
        /// <param name="total">The total.</param>
        private void ExportGrandTotal(double total)
        {
            _cell.ResetColumnIndex();
            var footerStyle = _customCellStyleService.CreateCenterTextStyle(_totalColor, _fontService.BlackBoldFont);
            _cell.EndColumn = _cell.CurrentColumn + 3;
            _cell.CellValue = "Grand Total";
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, footerStyle);

            var footerTotalStyle = _customCellStyleService.CreateDecimalNumberStyle(_totalColor, _fontService.BlackBoldFont);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = total;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, footerTotalStyle);
        }

        /// <summary>
        /// Exports the report title.
        /// </summary>
        private void ExportReportTitle()
        {
            var headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.BlackBoldFont);
            _cell.EndColumn = _cell.CurrentColumn + 4;
            _cell.CellValue = _criteria.Name;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);

            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex(); 
            HeaderCellText(headerStyle, "NO");
            HeaderCellText(headerStyle, _criteria.Level);
            HeaderCellText(headerStyle, "CURR");
            HeaderCellText(headerStyle, "BALANCE");
            HeaderCellText(headerStyle, "BASE(RM)");
        }

        private void HeaderCellText(XSSFCellStyle headerStyle, string textTitle)
        {
            _cell.CellValue = textTitle;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
        }

        private string FormatDate(DateTime value)
        {
            return value.ToString("M/d/yyyy");
        }

        private string FormatDateExcel(DateTime value)
        {
            return value.ToString("yyyy-M-d");
        }

        private string CreateReportNameWithExtension()
        {
            return string.Format("{0}_For_{1}.xlsx", _criteria.Name, _criteria.Date.ToString("MMM_yyyy"));
        }
    }
}
