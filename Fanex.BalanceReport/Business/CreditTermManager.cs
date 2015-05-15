using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml;
using Fanex.NAPReport.BalanceReport.DataAccess;
using Fanex.NAPReport.BalanceReport.Helper;
using Fanex.NAPReport.BalanceReport.Infrastructure.Extensions;
using Fanex.Data;
using Fanex.NAPReport.BalanceReport;
using Fanex.NAPReport.BalanceReport.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Service.Interface.Npoi;
using Fanex.Service.Core.Service.Implement.Npoi;
using Fanex.Service.Core.Extensions;

namespace Fanex.NAPReport.BalanceReport.Bussiness
{
    public class CreditTermManager : ICreditTermManager
    {
        private readonly IReportDAO _reportDao;
        private readonly List<Exchange> _exchangeList;
        private readonly string _saveDirectoryCreditTerm;
        private readonly string _creditTermExcelFileName;

        private const short HeaderColorIndex = 45;
        private const short SubTotalColorIndex = 46;
        private const short GrandTotalColorIndex = 47;
        private const short GrandTotalBorderColorIndex = 48;
        private const short RacingDueToPartnersColorIndex = 49;

        private HSSFWorkbook _workbook;
        private ISheet _workSheet;
        private Cell _cell;
        private INpoiOldFormatFontService _fontService;
        private INpoiOldFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiOldFormatCustomCellStyleService _customCellStyleService;
        private IFont _regularFont;

        #region config data variable
        private readonly string _toCreditTerm;
        private readonly string _ccCreditTerm;
        private readonly string _bccCreditTerm;


        private readonly string _subjectCreditTerm;
        private readonly string _contentCreditTerm;

        #endregion

        /// <summary>
        /// Check exist of config files
        /// </summary>
        private readonly bool _bCanDo = true;

        #region Constructors
        public CreditTermManager()
        {
            _cell = new Cell();
            _workbook = new HSSFWorkbook();
            _workSheet = (HSSFSheet)_workbook.CreateSheet("Credit Term");
            this.SetColorIndex();
            _fontService = new NpoiOldFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiOldFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiOldFormatCustomCellStyleService(_workbook);

            _reportDao = new ReportDAO();

            //Get data from cache
            _exchangeList = CacheHelper.Exchange;

            //-----------------------------------------------------------------------------------------------------------------
            //Init directory for save excel file
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data";
            var directoryInfo = new DirectoryInfo(rootDirectory);
            if (!directoryInfo.Exists) directoryInfo.Create();

            _saveDirectoryCreditTerm = rootDirectory + "\\Credit Term";
            directoryInfo = new DirectoryInfo(_saveDirectoryCreditTerm);
            if (!directoryInfo.Exists) directoryInfo.Create();

            this._creditTermExcelFileName = "Credit Term (" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ").xls";
            //-----------------------------------------------------------------------------------------------------------------
            //Read mail config
            var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "MailSection.config");
            if (!fileInfo.Exists) _bCanDo = false;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileInfo.FullName);

            this._toCreditTerm =
                // ReSharper disable PossibleNullReferenceException
                GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "CreditTerm").Attributes["to"].Value;
            // ReSharper restore PossibleNullReferenceException
            this._ccCreditTerm =
                // ReSharper disable PossibleNullReferenceException
                GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "CreditTerm").Attributes["cc"].Value;
            // ReSharper restore PossibleNullReferenceException
            _bccCreditTerm =
                // ReSharper disable PossibleNullReferenceException
               GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "CreditTerm").Attributes["bcc"].Value;

            fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Mail.config");
            if (!fileInfo.Exists)
            {
                this._subjectCreditTerm = "Credit term report";
                this._contentCreditTerm = "Credit term report";
            }
            else
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(fileInfo.FullName);

                this._subjectCreditTerm =
                    String.Format(xmlDocument.GetElementsByTagName("subject_creditterm")[0].InnerXml, FormatDateForMailMessage(DateTime.Now));
                this._contentCreditTerm =
                    String.Format(xmlDocument.GetElementsByTagName("content_creditterm")[0].InnerXml, new object[]
                    {
                        FormatDateForMailMessage(DateTime.Now)
                        , this._creditTermExcelFileName
                    }
                );
            }

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

        /// <summary>
        /// Send mail: Credit term report excel file, super balance report excel file, direct balace excel file
        /// </summary>
        public void ExportExcel()
        {
            if (!this._bCanDo) return;
            Console.WriteLine("Please wait...");
            this.SendMailCreditTerm();
            Console.WriteLine("Credit term mail was sent!.");
        }

        private void SetColorIndex()
        {
            HSSFPalette palette = _workbook.GetCustomPalette();
            palette.SetColorAtIndex(HeaderColorIndex, (byte)60, (byte)94, (byte)165);
            palette.SetColorAtIndex(SubTotalColorIndex, (byte)244, (byte)244, (byte)190);
            palette.SetColorAtIndex(GrandTotalColorIndex, (byte)219, (byte)225, (byte)187);
            palette.SetColorAtIndex(GrandTotalBorderColorIndex, (byte)255, (byte)102, (byte)0);
            palette.SetColorAtIndex(RacingDueToPartnersColorIndex, (byte)192, (byte)192, (byte)192);
        }

        private void ExportReportHeader()
        {
            ExportReportTitle();

            // Reset all value of row.
            _cell.ResetColumnIndex();
            _cell.CurrentRow += 1;
            _cell.EndRow = _cell.CurrentRow;
            IRow currentRow = _workSheet.CreateRow(_cell.CurrentRow);

            HeaderCellText(currentRow, "Role");
            HeaderCellText(currentRow, "UserName");
            HeaderCellText(currentRow, "Curr");
            HeaderCellText(currentRow, "Pre-Agreed Amount");
            HeaderCellText(currentRow, "Balance");
            HeaderCellText(currentRow, "Base (RM)");
            HeaderCellText(currentRow, "Remark");
            HeaderCellText(currentRow, "Phone");
            HeaderCellText(currentRow, "Checker");
            HeaderCellText(currentRow, "Remark Timestamp");
        }

        private void ExportReportTitle()
        {
            IRow currentRow = _workSheet.CreateRow(_cell.CurrentRow);
            ICellStyle headerStyle = CreateHeaderStyle();
            _cell.EndColumn = _cell.CurrentColumn + 9;
            _cell.CellValue = "CREDIT TERM (PRE AGREE REPORT)";
            NpoiOldFormatExcelExtension.RenderMergeCell(_workSheet, currentRow, _cell, headerStyle);
        }

        private ICellStyle CreateHeaderStyle()
        {
            ICellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(HeaderColorIndex, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private void HeaderCellText(IRow currentRow, string textTitle)
        {
            ICellStyle headerStyle = CreateHeaderStyle();
            _cell.CellValue = textTitle;
            NpoiOldFormatExcelExtension.RenderCell(_workSheet, currentRow, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
        }

        void CreateRowData(IRow row, CreditTerm item)
        {
            row.CreateCell(0).SetCellValue(item.Role);
            row.CreateCell(1).SetCellValue(item.Username);
            row.CreateCell(2).SetCellValue(item.Currency);
            row.CreateCell(3).SetCellValue(item.PreAgreeAmount);
            row.CreateCell(4).SetCellValue(item.Balance);
            row.CreateCell(5).SetCellValue(item.Balance_Base);
            row.CreateCell(6).SetCellValue(item.AccountRemark);
            row.CreateCell(7).SetCellValue(item.Phone);
            row.CreateCell(8).SetCellValue(item.CheckerName);
            row.CreateCell(9).SetCellValue(item.AccountRemarkDate);
        }

        void CreateRowDetail(IRow row, CreditTermDetail item)
        {
            row.CreateCell(0).SetCellValue(string.Empty);
            row.CreateCell(1).SetCellValue(item.Role);
            row.CreateCell(2).SetCellValue(item.Username);
            row.CreateCell(3).SetCellValue(item.Currency);
            row.CreateCell(4).SetCellValue(item.Balance);
            row.CreateCell(5).SetCellValue(item.Balance_Group);
            row.CreateCell(6).SetCellValue(item.Balance_Base);
            row.CreateCell(7).SetCellValue(item.AccountRemark);
            row.CreateCell(8).SetCellValue(item.CheckerName);
            row.CreateCell(9).SetCellValue(item.AccountRemarkDate);
        }

        /// <summary>
        /// Create credit term report excel file
        /// </summary>
        private void SendMailCreditTerm()
        {

            var criteria = new CreditTermReportCriteria();
            var creditTerms = new List<CreditTerm>();
            var creditTermDetails = new List<CreditTermDetail>();
            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple())
                {
                    //var employees = multi.Read<CreditTerm, CreditTermDetail>();
                    creditTerms = multi.Read<CreditTerm>().ToList();
                    creditTermDetails = multi.Read<CreditTermDetail>().ToList();
                }
            }            

            ExportReportHeader();
            var index = 2;
            
            foreach (var item in creditTerms)
            {
                var row = _workSheet.CreateRow(index);
                CreateRowData(row, item);
                var details = creditTermDetails.Where(s => s.GroupID == item.GroupID).ToList();
                foreach (var detail in details)
                {
                    index++;
                    var rowDetail = _workSheet.CreateRow(index);
                    CreateRowDetail(rowDetail, detail);
                }
                
                index++;
            }

            using (var fs = new FileStream(_creditTermExcelFileName, FileMode.Create, FileAccess.Write))
            {
                _workbook.Write(fs);
            }


            /*var ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Credit Term");

            ws.ViewOptions.ShowGridLines = false;
            ws.Rows[0].Cells.GetSubrangeAbsolute(0, 0, 0, 9).Merged = true;
            ws.Rows[0].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[0].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[0].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
            const int fontSize = 8 * 20;
            const string fontName = "Tahoma";
            const string formatNumber = "#,##0.00";
            const int rowHeight = 315;
            const int standardWith = 7 * 256;

            ws.Rows[0].Cells[0].Style.Font.Size = fontSize;
            ws.Rows[0].Cells[0].Style.Font.Name = fontName;

            ws.Rows[0].Height = rowHeight;
            //-------------------------------------------------------------------------------------------------------------------------------------
            
            //var dbResult = _reportDao.GetCreditTerm();

            if (dbResult == null || dbResult.ResultSet.Tables.Count < 2)
            {
                ws.Rows[0].Cells[0].Value = "There is no information available.";
                goto End;
            }
            var dtCreditTerm = dbResult.ResultSet.Tables[0];
            var dtCreditTermDetail = dbResult.ResultSet.Tables[1];
            //-------------------------------------------------------------------------------------------------------------------------------------
            //Header
            ws.Columns[0].Width = standardWith;
            ws.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[0].Cells[0].Value = "CREDIT TERM (PRE AGREE REPORT)";

            ws.Columns[1].Width = standardWith * 2;
            ws.Columns[2].Width = standardWith * 2;
            ws.Columns[3].Width = (standardWith * 3) - (standardWith / 2);
            ws.Columns[4].Width = standardWith * 2;
            ws.Columns[5].Width = standardWith * 2;
            ws.Columns[6].Width = standardWith * 2;
            ws.Columns[7].Width = standardWith;
            ws.Columns[8].Width = standardWith + (standardWith / 2);
            ws.Columns[9].Width = (standardWith * 3) - (standardWith / 2);

            ws.Rows[1].Height = rowHeight;
            ws.Rows[1].Style.Font.Name = fontName;
            for (int i = 0; i < 10; i++)
            {
                ws.Rows[1].Cells[i].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[1].Cells[i].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Rows[1].Cells[i].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
                ws.Rows[1].Cells[i].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[1].Cells[i].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Rows[1].Cells[i].Style.Font.Size = fontSize;
            }
            ws.Rows[1].Cells[0].Value = "Role";
            ws.Rows[1].Cells[1].Value = "UserName";
            ws.Rows[1].Cells[2].Value = "Curr";
            ws.Rows[1].Cells[3].Value = "Pre-Agreed Amount";
            ws.Rows[1].Cells[4].Value = "Balance";
            ws.Rows[1].Cells[5].Value = "Base (RM)";
            ws.Rows[1].Cells[6].Value = "Remark";
            ws.Rows[1].Cells[7].Value = "Phone";
            ws.Rows[1].Cells[8].Value = "Checker";
            ws.Rows[1].Cells[9].Value = "Remark Timestamp";
            //-------------------------------------------------------------------------------------------------------------------------------------
            int currentExcelRowIndex = 1;
            foreach (DataRow rowCreditTerm in dtCreditTerm.Rows)
            {
                //Master
                currentExcelRowIndex++;
                ws.Rows[currentExcelRowIndex].Height = rowHeight;
                ws.Rows[currentExcelRowIndex].Style.Font.Name = fontName;
                bool overLimit = Convert.ToBoolean(rowCreditTerm["OverLimit"]);
                for (int i = 0; i < 10; i++)
                {
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    if (overLimit)
                        ws.Rows[currentExcelRowIndex].Cells[i].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(230, 233, 240));
                    else ws.Rows[currentExcelRowIndex].Cells[i].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(253, 233, 217));
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.Font.Size = fontSize;
                }
                ws.Rows[currentExcelRowIndex].Cells[0].Value = Convert.ToString(rowCreditTerm["Role"]);
                ws.Rows[currentExcelRowIndex].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                ws.Rows[currentExcelRowIndex].Cells[1].Value = Convert.ToString(rowCreditTerm["Username"]);
                ws.Rows[currentExcelRowIndex].Cells[1].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Rows[currentExcelRowIndex].Cells[1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;


                ws.Rows[currentExcelRowIndex].Cells[2].Value =
                    this.GetExhange(DNAide.Core.CastAide.AsInt32(rowCreditTerm["currency"]));
                ws.Rows[currentExcelRowIndex].Cells[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                ws.Rows[currentExcelRowIndex].Cells[3].Value = Math.Round(FormatHandler.GetDouble(rowCreditTerm["PreAgreeAmount"]), 2);
                if ((double)ws.Rows[currentExcelRowIndex].Cells[3].Value != 0)
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.NumberFormat = formatNumber;
                else ws.Rows[currentExcelRowIndex].Cells[3].Value = 0;
                ws.Rows[currentExcelRowIndex].Cells[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (FormatHandler.GetDouble(rowCreditTerm["PreAgreeAmount"]) > 0)
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.Font.Color = System.Drawing.Color.Red;

                ws.Rows[currentExcelRowIndex].Cells[4].Value = Math.Round(FormatHandler.GetDouble(rowCreditTerm["Balance"]), 2);
                if ((double)ws.Rows[currentExcelRowIndex].Cells[4].Value != 0)
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.NumberFormat = formatNumber;
                else ws.Rows[currentExcelRowIndex].Cells[4].Value = 0;
                ws.Rows[currentExcelRowIndex].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (FormatHandler.GetDouble(rowCreditTerm["Balance"]) > 0)
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Color = System.Drawing.Color.Red;

                ws.Rows[currentExcelRowIndex].Cells[5].Value = Math.Round(FormatHandler.GetDouble(rowCreditTerm["Balance_Base"]), 2);
                if ((double)ws.Rows[currentExcelRowIndex].Cells[5].Value != 0)
                    ws.Rows[currentExcelRowIndex].Cells[5].Style.NumberFormat = formatNumber;
                else ws.Rows[currentExcelRowIndex].Cells[5].Value = 0;
                ws.Rows[currentExcelRowIndex].Cells[5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (FormatHandler.GetDouble(rowCreditTerm["Balance"]) > 0)
                    ws.Rows[currentExcelRowIndex].Cells[5].Style.Font.Color = System.Drawing.Color.Red;

                ws.Rows[currentExcelRowIndex].Cells[6].Value = Convert.ToString(rowCreditTerm["AccountRemark"]);
                ws.Rows[currentExcelRowIndex].Cells[7].Value = Convert.ToString(rowCreditTerm["Phone"]);
                ws.Rows[currentExcelRowIndex].Cells[7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                ws.Rows[currentExcelRowIndex].Cells[8].Value = Convert.ToString(rowCreditTerm["checkername"]);
                ws.Rows[currentExcelRowIndex].Cells[9].Value = FormatDate(rowCreditTerm["AccountRemarkDate"]);
                ws.Rows[currentExcelRowIndex].Cells[9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                if (DNAide.Core.CastAide.AsInt32(rowCreditTerm["groupID"]) <= 0) continue;

                //Detail
                //Detail header
                currentExcelRowIndex++;
                ws.Rows[currentExcelRowIndex].Height = rowHeight;
                ws.Rows[currentExcelRowIndex].Style.Font.Name = fontName;
                for (int i = 1; i < 10; i++)
                {
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.Font.Weight = ExcelFont.BoldWeight;
                    ws.Rows[currentExcelRowIndex].Cells[i].Style.Font.Size = fontSize;
                }
                ws.Rows[currentExcelRowIndex].Cells[1].Value = "Role";
                ws.Rows[currentExcelRowIndex].Cells[2].Value = "UserName";
                ws.Rows[currentExcelRowIndex].Cells[3].Value = "Curr";
                ws.Rows[currentExcelRowIndex].Cells[4].Value = "Balance";
                ws.Rows[currentExcelRowIndex].Cells[5].Value = this.GetExhange(DNAide.Core.CastAide.AsInt32(rowCreditTerm["currency"]));
                ws.Rows[currentExcelRowIndex].Cells[6].Value = "RM";
                ws.Rows[currentExcelRowIndex].Cells[7].Value = "Remark";
                ws.Rows[currentExcelRowIndex].Cells[8].Value = "";
                ws.Rows[currentExcelRowIndex].Cells[9].Value = "Remark Timestamp";
                //Detail body
                DataRow[] dataRows =
                    dtCreditTermDetail.Select("groupID=" + DNAide.Core.CastAide.AsInt32(rowCreditTerm["groupID"]));
                foreach (var dataRow in dataRows)
                {
                    currentExcelRowIndex++;
                    ws.Rows[currentExcelRowIndex].Height = rowHeight;
                    ws.Rows[currentExcelRowIndex].Style.Font.Name = fontName;
                    for (int i = 1; i < 10; i++)
                    {
                        ws.Rows[currentExcelRowIndex].Cells[i].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                        ws.Rows[currentExcelRowIndex].Cells[i].Style.Font.Name = fontName;
                        ws.Rows[currentExcelRowIndex].Cells[i].Style.Font.Size = fontSize;
                        ws.Rows[currentExcelRowIndex].Cells[i].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    }

                    ws.Rows[currentExcelRowIndex].Cells[1].Value = Convert.ToString(dataRow["Role"]);
                    ws.Rows[currentExcelRowIndex].Cells[1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                    ws.Rows[currentExcelRowIndex].Cells[2].Value = Convert.ToString(dataRow["Username"]);
                    ws.Rows[currentExcelRowIndex].Cells[2].Style.Font.Weight = ExcelFont.BoldWeight;
                    ws.Rows[currentExcelRowIndex].Cells[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


                    ws.Rows[currentExcelRowIndex].Cells[3].Value =
                        this.GetExhange(DNAide.Core.CastAide.AsInt32(dataRow["currency"]));
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                    ws.Rows[currentExcelRowIndex].Cells[4].Value = Math.Round(FormatHandler.GetDouble(dataRow["Balance"]), 2);
                    if ((double)ws.Rows[currentExcelRowIndex].Cells[4].Value != 0)
                        ws.Rows[currentExcelRowIndex].Cells[4].Style.NumberFormat = formatNumber;
                    else ws.Rows[currentExcelRowIndex].Cells[4].Value = 0;
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    if (FormatHandler.GetDouble(dataRow["Balance"]) > 0)
                        ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Color = System.Drawing.Color.Red;

                    ws.Rows[currentExcelRowIndex].Cells[5].Value = Math.Round(FormatHandler.GetDouble(dataRow["balance_Group"]), 2);
                    if ((double)ws.Rows[currentExcelRowIndex].Cells[5].Value != 0)
                        ws.Rows[currentExcelRowIndex].Cells[5].Style.NumberFormat = formatNumber;
                    else ws.Rows[currentExcelRowIndex].Cells[5].Value = 0;
                    ws.Rows[currentExcelRowIndex].Cells[5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    if (FormatHandler.GetDouble(dataRow["balance_Group"]) > 0)
                        ws.Rows[currentExcelRowIndex].Cells[5].Style.Font.Color = System.Drawing.Color.Red;

                    ws.Rows[currentExcelRowIndex].Cells[6].Value = Math.Round(FormatHandler.GetDouble(dataRow["Balance_Base"]), 2);
                    if ((double)ws.Rows[currentExcelRowIndex].Cells[6].Value != 0)
                        ws.Rows[currentExcelRowIndex].Cells[6].Style.NumberFormat = formatNumber;
                    else ws.Rows[currentExcelRowIndex].Cells[6].Value = 0;
                    ws.Rows[currentExcelRowIndex].Cells[6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    if (FormatHandler.GetDouble(dataRow["Balance_Base"]) > 0)
                        ws.Rows[currentExcelRowIndex].Cells[6].Style.Font.Color = System.Drawing.Color.Red;

                    ws.Rows[currentExcelRowIndex].Cells[7].Value = Convert.ToString(dataRow["AccountRemark"]);
                    ws.Rows[currentExcelRowIndex].Cells[7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                    ws.Rows[currentExcelRowIndex].Cells[8].Value = Convert.ToString("Edit");
                    ws.Rows[currentExcelRowIndex].Cells[8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    ws.Rows[currentExcelRowIndex].Cells[9].Value = FormatDate(dataRow["AccountRemarkDate"]);
                    ws.Rows[currentExcelRowIndex].Cells[9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                }
            }
        //-------------------------------------------------------------------------------------------------------------------------------------
        End:
            //ef.SaveXls(this._saveDirectoryCreditTerm + "\\" + this._creditTermExcelFileName);
            var memoryStream = new MemoryStream();
            ef.SaveXls(memoryStream);
            SendMailExtension.SendMail(
                this._toCreditTerm,
                this._ccCreditTerm,
                _bccCreditTerm,
                this._subjectCreditTerm,
                this._subjectCreditTerm,
                this._contentCreditTerm,
                memoryStream, this._creditTermExcelFileName);
             * */
        }

        /// <summary>
        /// format date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string FormatDate(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            int mm = date.Month;
            int dd = date.Day;
            int hh = date.Hour;
            int mn = date.Minute;
            int yyyy = date.Year;
            return mm.ToString() + "/" + dd.ToString() + "/" + yyyy.ToString();
        }

        private static string FormatDateForMailMessage(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            int mm = date.Month;
            int dd = date.Day;
            int hh = date.Hour;
            int mn = date.Minute;
            int yyyy = date.Year;
            return yyyy.ToString() + "-" + mm.ToString() + "-" + dd.ToString();
        }
    }
}
