using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Fanex.BalanceReport;
using Fanex.Data;
using Fanex.NAPReport.BalanceReport.DataAccess;
using Fanex.NAPReport.BalanceReport.Helper;
using Fanex.NAPReport.BalanceReport.Model;
using Fanex.Service.Core.Constants;
using Fanex.Service.Core.Extensions;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Service.Implement.Npoi;
using Fanex.Service.Core.Service.Interface.Npoi;
using Fanex.Service.Core.Utilities;
using NPOI.XSSF.UserModel;

namespace Fanex.NAPReport.BalanceReport.Bussiness
{
    public class BalanceManager : IBalanceManager
    {
        private readonly IReportDAO _reportDao;
        private readonly List<Exchange> _exchangeList;
        private readonly string _superBalanceExcelFileName;

        public string ReportPath
        {
            get;
            set;
        }

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

        #region Constructors
        public BalanceManager(BalanceReportBaseCriteria criteria)
        {
            _criteria = criteria;
            _reportDao = new ReportDAO();
            _exchangeList = CacheHelper.Exchange; 
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
        }
        #endregion

        /// <summary>
        /// Send mail: Credit term report excel file, super balance report excel file, direct balace excel file
        /// </summary>
        public void ExportExcel()
        {
            Console.WriteLine("Please wait...");
            
            ExportReportTitle();
            ExportReportContent();
            string fileNameWithExtension = CreateReportNameWithExtension();
            ReportPath = Path.Combine(AppSettings.ExportDataDirectory, fileNameWithExtension);
            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectory, fileNameWithExtension, this._workbook);

            Console.WriteLine("Super balance mail was sent!.");
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
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 250 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 150 * NPOIConstants.FontWidth);
        }

        /// <summary>
        /// Get exchange from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetExhange(int key)
        {
            return CacheHelper.GetExchangeName(key);
        }

        void CreateRowData(BalanceModel item, int rowIndex)
        {
            _cell.ResetColumnIndex();
            var color = rowIndex % 2 == 0 ? _evenColor : _oddColor;
            var textCenterStyle = _customCellStyleService.CreateCenterTextStyle(color, _fontService.RegularFont);
            var decimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(color, _fontService.RegularFont);
            var textLeftStyle = _customCellStyleService.CreateLeftTextStyle(color, _fontService.RegularFont);
            
            bindData(rowIndex, textCenterStyle);
            bindData(item.Username, textLeftStyle);
            bindData(GetExhange(item.Currency), textCenterStyle);
            bindData(item.Balance, decimalNumberStyle);
            bindData(item.BaseBalance, decimalNumberStyle);
        }

        void bindData(object dataValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = dataValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
            _cell.IncreaseColumnIndex();
        }

        /// <summary>
        /// Create super balance excel file
        /// </summary>
        private void ExportReportContent()
        {
            //var superBalance = _reportDao.GetSuperBalance(new SuperBalanceReportCriteria()).Take(10).ToList();
            var index = 1;
            double total = 0;
            using (ObjectDb db = new ObjectDb(_criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (index == 10)
                        {
                            break;
                        }
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
                    }
                }
            }

            ExportGrandTotal(total);
        }

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
            return value.ToString("m/d/yyyy");
        }

        private string FormatDateExcel(DateTime value)
        {
            return value.ToString("yyyy-M-d");
        }

        private string CreateReportNameWithExtension()
        {
            return string.Format("{0} ({1}).xlsx", _criteria.Name, _criteria.Date.ToString("yyyy-M-d"));
        }
    }
}
