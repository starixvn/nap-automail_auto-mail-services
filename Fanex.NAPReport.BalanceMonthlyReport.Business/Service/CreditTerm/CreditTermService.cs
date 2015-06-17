namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.CreditTerm
{
    using System;
    using System.Collections.Generic;
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
    /// Credit Term Manager class.
    /// </summary>
    public class CreditTermService : ICreditTermService
    {
        private IBalanceRepository _repository;

        private Color _headerColor;
        private Color _overLimitColor;
        private Color _normalColor;
        private Color _contentColor;
        private Color _borderColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private XSSFFont _regularFont;

        XSSFCellStyle overLimitTextStyle;
        XSSFCellStyle overLimitCenterBoldTextStyle;
        XSSFCellStyle overLimitDecimalNumberStyle;
        XSSFCellStyle overLimitTextLeftStyle;

        XSSFCellStyle normalTextStyle;
        XSSFCellStyle normalCenterBoldTextStyle;
        XSSFCellStyle normalDecimalNumberStyle;
        XSSFCellStyle normalTextLeftStyle;

        XSSFCellStyle normalLeftBoldTextStyle;
        XSSFCellStyle headerStyle;        

        private Cell _cell;

        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;

        private List<Exchange> _currencyList;

        #region Constructors
        public CreditTermService() 
        {
            _repository = new BalanceRepository();

            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();

            _currencyList = _repository.GetCurrencyList();
        }

        public string DirectoryOfFileName
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public void ExportExcelCreditTerm()
        {
            CreateSheet();
            InitColumnWidth();
            InitCellStyle();
            ExportExcelFile();
            FileName = CreateReportNameWithExtension();
            DirectoryOfFileName = Path.Combine(AppSettings.DataDirectoryWithYearCreditTerm, FileName);
            SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYearCreditTerm, FileName, this._workbook, AppSettings.ExcelPasswordProtectedCreditTerm); 
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(181, 196, 208);
            _overLimitColor = Color.FromArgb(230, 233, 240);
            _normalColor = Color.FromArgb(253, 233, 217);
            _borderColor = Color.FromArgb(124, 152, 164);
            _contentColor = Color.FromArgb(255, 255, 255);
        }

        private void CreateSheet()
        {
            string sheetName = "Credit Term";
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(4, 1, 4, 1);
        }

        /// <summary>
        /// Initializes the width of the column.
        /// </summary>
        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(5, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(6, 150 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(7, 400 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(8, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(9, 150 * NPOIConstants.FontWidth);
        }

        private void InitCellStyle()
        {
             overLimitTextStyle = _customCellStyleService.CreateCenterTextStyle(_overLimitColor, _fontService.RegularFont); ;
             overLimitCenterBoldTextStyle = _customCellStyleService.CreateCenterTextStyle(_overLimitColor, _fontService.BlackBoldFont);
             overLimitDecimalNumberStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_overLimitColor, _fontService.RegularFont);
             overLimitTextLeftStyle = _customCellStyleService.CreateLeftTextStyle(_overLimitColor, _fontService.RegularFont);

             normalTextStyle = _customCellStyleService.CreateCenterTextStyle(_normalColor, _fontService.RegularFont); ;
             normalCenterBoldTextStyle = _customCellStyleService.CreateCenterTextStyle(_normalColor, _fontService.BlackBoldFont);
             normalDecimalNumberStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(_normalColor, _fontService.RegularFont);
             normalTextLeftStyle = _customCellStyleService.CreateLeftTextStyle(_normalColor, _fontService.RegularFont);

             normalLeftBoldTextStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _fontService.BlackBoldFont);
             headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.BlackBoldFont);
        }
        #endregion

        private string GetCurrencyName(int key)
        {
            string currencyName = string.Empty;
            currencyName = _currencyList.Where(s => s.ExchangeID == key).Select(s => s.Currency).FirstOrDefault();

            return currencyName;
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        private void ExportReportHeader()
        {
            ExportReportTitle();
            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex();

            HeaderCellText(headerStyle, "Role");
            HeaderCellText(headerStyle, "UserName");
            HeaderCellText(headerStyle, "Curr");
            HeaderCellText(headerStyle, "Pre-Agreed Amount");
            HeaderCellText(headerStyle, "Balance");
            HeaderCellText(headerStyle, "Base (RM)");
            HeaderCellText(headerStyle, "Remark");
            HeaderCellText(headerStyle, "Phone");
            HeaderCellText(headerStyle, "Checker");
            HeaderCellText(headerStyle, "Remark Timestamp");
        }

        private void ExportReportTitle()
        {
            var headerStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _fontService.BlackBoldFont);
            _cell.EndColumn = _cell.CurrentColumn + 9;
            _cell.CellValue = "CREDIT TERM (PRE AGREE REPORT)"; 
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
        }

        private void HeaderCellText(XSSFCellStyle headerStyle, string textTitle)
        {
            _cell.CellValue = textTitle;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
        }

        /// <summary>
        /// Exports the sub header.
        /// </summary>
        /// <param name="currency">The currency.</param>
        private void ExportSubHeader(int currency)
        {
            _cell.ResetColumnIndex();
            _cell.IncreaseColumnIndex();
            HeaderCellText(headerStyle, "Role");
            HeaderCellText(headerStyle, "UserName");
            HeaderCellText(headerStyle, "Curr");
            HeaderCellText(headerStyle, "Balance");
            HeaderCellText(headerStyle, GetCurrencyName(currency));
            HeaderCellText(headerStyle, "RM");
            HeaderCellText(headerStyle, "Remark");
            HeaderCellText(headerStyle, string.Empty);
            HeaderCellText(headerStyle, "Remark Timestamp");
        }

        /// <summary>
        /// Creates the row data.
        /// </summary>
        /// <param name="item">The row item.</param>
        private void CreateRowData(CreditTermMain item)
        {
            _cell.ResetColumnIndex();
            var color = item.OverLimit ? _overLimitColor : _normalColor;
            var textStyle = item.OverLimit ? overLimitTextStyle : normalTextStyle;
            var centerBoldTextStyle = item.OverLimit ? overLimitCenterBoldTextStyle : normalCenterBoldTextStyle;
            var decimalNumberStyle = item.OverLimit ? overLimitDecimalNumberStyle : normalDecimalNumberStyle;
            var textLeftStyle = item.OverLimit ? overLimitTextLeftStyle : normalTextLeftStyle;
            BindData(item.Role, textStyle);
            BindData(item.Username, centerBoldTextStyle);
            BindData(GetCurrencyName(item.Currency), textStyle);
            BindData(item.PreAgreeAmount, decimalNumberStyle);
            BindData(item.Balance, decimalNumberStyle);
            BindData(item.Balance_Base, decimalNumberStyle);
            BindData(item.AccountRemark, textLeftStyle);
            BindData(item.Phone, textLeftStyle);
            BindData(item.CheckerName, textLeftStyle);
            BindData(FormatDate(item.AccountRemarkDate), textLeftStyle);
        }

        /// <summary>
        /// Creates the row detail.
        /// </summary>
        /// <param name="item">The row item.</param>
        private void CreateRowDetail(CreditTermDetail item)
        {
            _cell.ResetColumnIndex();
            var textStyle = _bodyCellStyleService.LeftTextStyle;
            var decimalNumberStyle = _bodyCellStyleService.PositiveDecimalNumberStyle;     
            _cell.IncreaseColumnIndex();
            BindData(item.Role, textStyle);
            BindData(item.Username, normalLeftBoldTextStyle);
            BindData(GetCurrencyName(item.Currency), textStyle);
            BindData(item.Balance, decimalNumberStyle);
            BindData(item.Balance_Group, decimalNumberStyle);
            BindData(item.Balance_Base, decimalNumberStyle);
            BindData(item.AccountRemark, textStyle);
            BindData("Edit", textStyle);
            BindData(FormatDate(item.AccountRemarkDate), textStyle);
        }

        private void BindData(object dataValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = dataValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
            _cell.IncreaseColumnIndex();
        }

        /// <summary>
        /// Create credit term report excel file.
        /// </summary>
        private void ExportExcelFile()
        {
            var criteria = new CreditTermReportCriteria();
            CreditTerm creditTerm = _repository.GetCreditTerm(criteria);

            ExportReportHeader();
            _cell.IncreaseRowIndex();

            foreach (var item in creditTerm.CreditTermMains)
            {
                CreateRowData(item);
                _cell.IncreaseRowIndex();

                ExportSubHeader(item.Currency);
                _cell.IncreaseRowIndex();

                var details = creditTerm.CreditTermDetails.Where(s => s.GroupID == item.GroupID).ToList();
                foreach (var detail in details)
                {
                    CreateRowDetail(detail);
                    _cell.IncreaseRowIndex();
                }
                if (AppSettings.IsTest)
                {
                    return;
                }
            }
        }

        private string FormatDate(DateTime value)
        {
            return value.ToString("M/d/yyyy");
        }

        private string CreateReportNameWithExtension()
        {
            return string.Format("Credit Term - {0}.xlsx", DateTime.Today.ToString("yyyy-MM-dd"));
        }
    }
}
