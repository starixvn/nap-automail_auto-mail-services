namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Bussiness
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Fanex.BalanceReport;
    using Fanex.Data;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Helper;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
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
    public class CreditTermManager : ICreditTermManager
    {
        private readonly IReportDAO _reportDao;
        private readonly List<Exchange> _exchangeList;
        private Color _headerColor;
        private Color _overLimitColor;
        private Color _normalColor;
        private Color _contentColor;
        private Color _borderColor;
        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;
        private XSSFFont _regularFont;

        #region Constructors
        public CreditTermManager() 
        {
            _reportDao = new ReportDAO();
            _exchangeList = CacheHelper.Exchange;
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();
        }

        public string ReportPath
        {
            get;
            set;
        }

        public void ExportExcel(Report report)
        {
            CreateSheet();
            InitColumnWidth();
            ExportExcelFile();
            string fileNameWithExtension = CreateReportNameWithExtension(report);
            ReportPath = Path.Combine(AppSettings.ExportDataDirectory, fileNameWithExtension);
            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectory, fileNameWithExtension, this._workbook);
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
            _workSheet.SetColumnWidth(7, 70 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(8, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(9, 150 * NPOIConstants.FontWidth);
        }
        #endregion

        private string GetExhange(int key)
        {
            return CacheHelper.GetExchangeName(key);
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        private void ExportReportHeader()
        {
            ExportReportTitle();
            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex();

            var headerStyle = CreateHeaderStyle();
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

        private XSSFCellStyle CreateHeaderStyle()
        {
            return _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.BlackBoldFont);
        }

        /// <summary>
        /// Exports the sub header.
        /// </summary>
        /// <param name="currency">The currency.</param>
        private void ExportSubHeader(int currency)
        {
            _cell.ResetColumnIndex();
            _cell.IncreaseColumnIndex();
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.BlackBoldFont);
            HeaderCellText(headerStyle, "Role");
            HeaderCellText(headerStyle, "UserName");
            HeaderCellText(headerStyle, "Curr");
            HeaderCellText(headerStyle, "Balance");
            HeaderCellText(headerStyle, GetExhange(currency));
            HeaderCellText(headerStyle, "RM");
            HeaderCellText(headerStyle, "Remark");
            HeaderCellText(headerStyle, string.Empty);
            HeaderCellText(headerStyle, "Remark Timestamp");
        }

        /// <summary>
        /// Creates the row data.
        /// </summary>
        /// <param name="item">The row item.</param>
        private void CreateRowData(CreditTerm item)
        {
            _cell.ResetColumnIndex();
            var color = item.OverLimit ? _overLimitColor : _normalColor;
            var textStyle = _customCellStyleService.CreateCenterTextStyle(color, _fontService.RegularFont);
            var usernameStyle = _customCellStyleService.CreateCenterTextStyle(color, _fontService.BlackBoldFont);
            var decimalNumberStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(color, _fontService.RegularFont);
            var textLeftStyle = _customCellStyleService.CreateLeftTextStyle(color, _fontService.RegularFont);
            BindData(item.Role, textStyle);
            BindData(item.Username, usernameStyle);
            BindData(GetExhange(item.Currency), textStyle);
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
            var usernameStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _fontService.BlackBoldFont);            
            _cell.IncreaseColumnIndex();
            BindData(item.Role, textStyle);
            BindData(item.Username, usernameStyle);
            BindData(GetExhange(item.Currency), textStyle);
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
            var creditTerms = new List<CreditTerm>();
            var creditTermDetails = new List<CreditTermDetail>();
            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple())
                {
                    creditTerms = multi.Read<CreditTerm>().ToList();
                    creditTermDetails = multi.Read<CreditTermDetail>().ToList();
                }
            }            

            ExportReportHeader();
            _cell.IncreaseRowIndex();

            foreach (var item in creditTerms)
            {
                CreateRowData(item);
                _cell.IncreaseRowIndex();

                ExportSubHeader(item.Currency);
                _cell.IncreaseRowIndex();

                var details = creditTermDetails.Where(s => s.GroupID == item.GroupID).ToList();
                foreach (var detail in details)
                {
                    CreateRowDetail(detail);
                    _cell.IncreaseRowIndex();
                }
            }
        }

        private string FormatDate(DateTime value)
        {
            return value.ToString("M/d/yyyy");
        }

        private string CreateReportNameWithExtension(Report report)
        {
            return string.Format("Credit Term ({0}).xlsx", report.Date.ToString("yyyy-M-d"));
        }
    }
}
