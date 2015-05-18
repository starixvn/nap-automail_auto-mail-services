using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
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
    public class CreditTermManager : ICreditTermManager
    {
        private readonly IReportDAO _reportDao;
        private readonly List<Exchange> _exchangeList;
        public string CreditTermExcelFileName {
            get;
            set;
        }

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

        /// <summary>
        /// Check exist of config files
        /// </summary>
        private readonly bool _bCanDo = true;

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

        /// <summary>
        /// Send mail: Credit term report excel file, super balance report excel file, direct balace excel file
        /// </summary>
        public void ExportExcel(Report report)
        {
            CreateSheet();
            InitColumnWidth();
            Console.WriteLine("Please wait...");
            ExportExcelFile();

            string fileNameWithExtension = CreateReportNameWithExtension(report);
            CreditTermExcelFileName = Path.Combine(AppSettings.ExportDataDirectory, fileNameWithExtension);

            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectory, fileNameWithExtension, this._workbook);
            Console.WriteLine("Credit term mail was sent!.");
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

        /// <summary>
        /// Use for MailSection.Config file
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static XmlNode GetNodeByName(XmlNode parent, string name)
        {
            return (from XmlNode xmlNode in parent.ChildNodes
                    where xmlNode.Attributes != null
                    // ReSharper disable PossibleNullReferenceException
                    where xmlNode.Attributes["name"] != null
                    // ReSharper restore PossibleNullReferenceException
                    // ReSharper disable PossibleNullReferenceException
                    select xmlNode).FirstOrDefault(xmlNode => xmlNode.Attributes["name"].Value.ToLower().Contains(name.ToLower()));
            // ReSharper restore PossibleNullReferenceException
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

        void CreateRowData(CreditTerm item)
        {
            _cell.ResetColumnIndex();
            var color = item.OverLimit ? _overLimitColor : _normalColor;
            var textStyle = _customCellStyleService.CreateCenterTextStyle(color, _fontService.RegularFont);
            var usernameStyle = _customCellStyleService.CreateCenterTextStyle(color, _fontService.BlackBoldFont);
            var decimalNumberStyle = _customCellStyleService.CreatePositiveDecimalNumberStyle(color, _fontService.RegularFont);
            var textLeftStyle = _customCellStyleService.CreateLeftTextStyle(color, _fontService.RegularFont);
            bindData(item.Role, textStyle);
            bindData(item.Username, usernameStyle);
            bindData(GetExhange(item.Currency), textStyle);
            bindData(item.PreAgreeAmount, decimalNumberStyle);
            bindData(item.Balance, decimalNumberStyle);
            bindData(item.Balance_Base, decimalNumberStyle);
            bindData(item.AccountRemark, textLeftStyle);
            bindData(item.Phone, textLeftStyle);
            bindData(item.CheckerName, textLeftStyle);
            bindData(FormatDate(item.AccountRemarkDate), textLeftStyle);
        }

        void CreateRowDetail(CreditTermDetail item)
        {
            _cell.ResetColumnIndex();
            var textStyle = _bodyCellStyleService.LeftTextStyle;
            var decimalNumberStyle = _bodyCellStyleService.PositiveDecimalNumberStyle;
            var usernameStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _fontService.BlackBoldFont);            
            _cell.IncreaseColumnIndex();
            bindData(item.Role, textStyle);
            bindData(item.Username, usernameStyle);
            bindData(GetExhange(item.Currency), textStyle);
            bindData(item.Balance, decimalNumberStyle);
            bindData(item.Balance_Group, decimalNumberStyle);
            bindData(item.Balance_Base, decimalNumberStyle);
            bindData(item.AccountRemark, textStyle);
            bindData("Edit", textStyle);
            bindData(FormatDate(item.AccountRemarkDate), textStyle);
        }

        void bindData(object dataValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = dataValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
            _cell.IncreaseColumnIndex();
        }

        /// <summary>
        /// Create credit term report excel file
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
                    //var employees = multi.Read<CreditTerm, CreditTermDetail>();
                    creditTerms = multi.Read<CreditTerm>().Take(10).ToList();
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
            return value.ToString("m/d/yyyy");
        }

        private string FormatDateExcel(DateTime value)
        {
            return value.ToString("yyyy-M-d");
        }

        private string CreateReportNameWithExtension(Report report)
        {
            return string.Format("Credit Term ({0}).xlsx", report.Date.ToString("yyyy-M-d"));
        }
    }
}
