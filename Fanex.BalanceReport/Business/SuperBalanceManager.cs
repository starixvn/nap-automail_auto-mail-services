using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Xml;
using Fanex.NAPReport.BalanceReport.DataAccess;
using Fanex.NAPReport.BalanceReport.Helper;
using Fanex.NAPReport.BalanceReport.Infrastructure.Extensions;
using System.Drawing;
using Fanex.NAPReport.BalanceReport.Model;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;

namespace Fanex.NAPReport.BalanceReport.Bussiness
{
    public class SuperBalanceManager : ISuperBalanceManager
    {
        private readonly IReportDAO _reportDao;
        private readonly List<Exchange> _exchangeList;
        private readonly string _saveDirectorySuperBalanceReport;

        private readonly string _superBalanceExcelFileName;

        #region config data variable
        private readonly string _toSuperBalance;
        private readonly string _ccSuperBalance;
        private readonly string _bccSuperBalance;

        private readonly string _subjectSuperBalance;
        private readonly string _contentSuperBalance;
        #endregion

        /// <summary>
        /// Check exist of config files
        /// </summary>
        private readonly bool _bCanDo = true;

        #region Constructors
        public SuperBalanceManager()
        {
            _reportDao = new ReportDAO();

            //Get data from cache
            _exchangeList = CacheHelper.Exchange;

            //-----------------------------------------------------------------------------------------------------------------
            //Init directory for save excel file
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data";
            var directoryInfo = new DirectoryInfo(rootDirectory);
            if (!directoryInfo.Exists) directoryInfo.Create();

            _saveDirectorySuperBalanceReport = rootDirectory + "\\Super Balance Report";
            directoryInfo = new DirectoryInfo(_saveDirectorySuperBalanceReport);
            if (!directoryInfo.Exists) directoryInfo.Create();

            this._superBalanceExcelFileName = "Super Balance (" +
                DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ").xls";
            //-----------------------------------------------------------------------------------------------------------------
            //Read mail config
            var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "MailSection.config");
            if (!fileInfo.Exists) _bCanDo = false;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileInfo.FullName);

            this._toSuperBalance =
                // ReSharper disable PossibleNullReferenceException
                GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "SuperBalance").Attributes["to"].Value;
            // ReSharper restore PossibleNullReferenceException
            this._ccSuperBalance =
                // ReSharper disable PossibleNullReferenceException
                GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "SuperBalance").Attributes["cc"].Value;
            // ReSharper restore PossibleNullReferenceException

            _bccSuperBalance =
                // ReSharper disable PossibleNullReferenceException
                GetNodeByName(xmlDocument.GetElementsByTagName("Send")[0], "SuperBalance").Attributes["bcc"].Value;
            // ReSharper restore PossibleNullReferenceException

            fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Mail.config");
            if (!fileInfo.Exists)
            {
                this._subjectSuperBalance = "Super balance report";
                this._contentSuperBalance = "Super balance report";
            }
            else
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(fileInfo.FullName);

                this._subjectSuperBalance =
                    String.Format(xmlDocument.GetElementsByTagName("subject_superbalance")[0].InnerText, FormatDateForMailMessage(DateTime.Now));
                this._contentSuperBalance =
                    String.Format(xmlDocument.GetElementsByTagName("content_superbalance")[0].InnerXml,
                    new object[]
                        {
                            FormatDateForMailMessage(DateTime.Now)
                            , this._superBalanceExcelFileName
                        });
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
            this.SendMailSuperBalance();
            Console.WriteLine("Super balance mail was sent!.");
        }

        void CreateRowData(IRow row, BalanceModel item)
        {
            row.CreateCell(0).SetCellValue(item.CustID);
            row.CreateCell(1).SetCellValue(item.Username);
            row.CreateCell(2).SetCellValue(item.Currency);
            row.CreateCell(3).SetCellValue(item.Balance);
            row.CreateCell(4).SetCellValue(item.BaseBalance);
        }

        /// <summary>
        /// Create super balance excel file
        /// </summary>
        private void SendMailSuperBalance()
        {
            var superBalance = _reportDao.GetSuperBalance(new SuperBalanceReportCriteria());

            var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            var sh = (HSSFSheet)wb.CreateSheet("Credit Term");
            var index = 1;
            
            foreach (var item in superBalance)
            {
                var r = sh.CreateRow(index);
                CreateRowData(r, item);
                
                index++;
            }

            using (var fs = new FileStream("Credit Term.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }

            /*
             * var ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Super Balance");
            ws.ViewOptions.ShowGridLines = false;
            const int fontSize = 8 * 20;
            const string fontName = "Tahoma";
            const string formatNumber = "#,##0.00";
            const int rowHeight = 315;
            const int standardWith = 7 * 256;

            //Header 1
            ws.Rows[0].Height = rowHeight;
            ws.Rows[0].Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
            ws.Rows[0].Cells[0].Value = "Super Balance";
            ws.Rows[0].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[0].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[0].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[0].Cells[0].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[0].Cells[0].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[0].Cells[0].Style.Font.Size = fontSize;
            ws.Rows[0].Cells[0].Style.Font.Name = fontName;
            //Header 2
            ws.Rows[1].Height = rowHeight;

            ws.Columns[0].Width = standardWith;
            ws.Rows[1].Cells[0].Value = "NO";
            ws.Rows[1].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[1].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[1].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[1].Cells[0].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[1].Cells[0].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[1].Cells[0].Style.Font.Size = fontSize;
            ws.Rows[1].Cells[0].Style.Font.Name = fontName;

            ws.Columns[1].Width = (standardWith * 2) + (int)(standardWith / 2);
            ws.Rows[1].Cells[1].Value = "Super";
            ws.Rows[1].Cells[1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[1].Cells[1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[1].Cells[1].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[1].Cells[1].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[1].Cells[1].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[1].Cells[1].Style.Font.Size = fontSize;
            ws.Rows[1].Cells[1].Style.Font.Name = fontName;

            ws.Columns[2].Width = standardWith;
            ws.Rows[1].Cells[2].Value = "CURR";
            ws.Rows[1].Cells[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[1].Cells[2].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[1].Cells[2].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[1].Cells[2].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[1].Cells[2].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[1].Cells[2].Style.Font.Size = fontSize;
            ws.Rows[1].Cells[2].Style.Font.Name = fontName;

            ws.Columns[3].Width = standardWith * 2;
            ws.Rows[1].Cells[3].Value = "BALANCE";
            ws.Rows[1].Cells[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[1].Cells[3].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[1].Cells[3].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[1].Cells[3].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[1].Cells[3].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[1].Cells[3].Style.Font.Size = fontSize;
            ws.Rows[1].Cells[3].Style.Font.Name = fontName;

            ws.Columns[4].Width = standardWith * 2;
            ws.Rows[1].Cells[4].Value = "BASE(RM)";
            ws.Rows[1].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[1].Cells[4].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[1].Cells[4].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[1].Cells[4].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[1].Cells[4].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(181, 196, 208));
            ws.Rows[1].Cells[4].Style.Font.Size = fontSize;
            ws.Rows[1].Cells[4].Style.Font.Name = fontName;
            //-------------------------------------------------------------------------------------------------------------------------------------
            //Get data
            var dbResult = _reportDao.GetSuperBalance();
            if (dbResult == null || dbResult.ResultSet.Tables.Count <= 0)
            {
                ws.Rows[2].Height = rowHeight;
                ws.Rows[2].Cells[0].Value = "There is no information available.";
                ws.Rows[2].Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
                ws.Rows[2].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Rows[2].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[2].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Rows[2].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Rows[2].Cells[0].Style.Font.Size = fontSize;
                ws.Rows[2].Cells[0].Style.Font.Name = fontName;
                goto End;
            }
            //Body
            var dtSupperBalance = dbResult.ResultSet.Tables[0];
            int index = 0;
            bool isOdd = false;
            int currentExcelRowIndex = 1;
            foreach (DataRow dataRow in dtSupperBalance.Rows)
            {
                if (
                    Math.Round(CastAide.AsDecimal(DNAide.Utils.NumberFormat.DecFormat(dataRow["balance"], 2)), 0) == 0
                    && Math.Round(CastAide.AsDecimal(DNAide.Utils.NumberFormat.DecFormat(dataRow["basebalance"], 2)), 0) == 0)
                    continue;
                index++;
                ++currentExcelRowIndex;

                if (isOdd)
                {
                    isOdd = false;
                    ws.Rows[currentExcelRowIndex].Cells[0].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
                    ws.Rows[currentExcelRowIndex].Cells[1].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
                    ws.Rows[currentExcelRowIndex].Cells[2].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(223, 234, 255));
                }
                else
                {
                    isOdd = true;
                    ws.Rows[currentExcelRowIndex].Cells[0].Style.FillPattern.SetSolid(System.Drawing.Color.White);
                    ws.Rows[currentExcelRowIndex].Cells[1].Style.FillPattern.SetSolid(System.Drawing.Color.White);
                    ws.Rows[currentExcelRowIndex].Cells[2].Style.FillPattern.SetSolid(System.Drawing.Color.White);
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.FillPattern.SetSolid(System.Drawing.Color.White);
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.FillPattern.SetSolid(System.Drawing.Color.White);
                }

                ws.Rows[currentExcelRowIndex].Height = rowHeight;

                ws.Rows[currentExcelRowIndex].Cells[0].Value = index;
                ws.Rows[currentExcelRowIndex].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[0].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[currentExcelRowIndex].Cells[0].Style.Font.Size = fontSize;
                ws.Rows[currentExcelRowIndex].Cells[0].Style.Font.Name = fontName;

                ws.Rows[currentExcelRowIndex].Cells[1].Value = Convert.ToString(dataRow["username"]);
                ws.Rows[currentExcelRowIndex].Cells[1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                ws.Rows[currentExcelRowIndex].Cells[1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[1].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[currentExcelRowIndex].Cells[1].Style.Font.Size = fontSize;
                ws.Rows[currentExcelRowIndex].Cells[1].Style.Font.Name = fontName;

                ws.Rows[currentExcelRowIndex].Cells[2].Value = this.GetExhange(DNAide.Core.CastAide.AsInt32(dataRow["currency"]));
                ws.Rows[currentExcelRowIndex].Cells[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[2].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[2].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[currentExcelRowIndex].Cells[2].Style.Font.Size = fontSize;
                ws.Rows[currentExcelRowIndex].Cells[2].Style.Font.Name = fontName;

                ws.Rows[currentExcelRowIndex].Cells[3].Value = Math.Round(IBCUtils.FormatHandler.GetDouble(dataRow["balance"]), 2);
                if ((double)ws.Rows[currentExcelRowIndex].Cells[3].Value != 0)
                    ws.Rows[currentExcelRowIndex].Cells[3].Style.NumberFormat = formatNumber;
                else ws.Rows[currentExcelRowIndex].Cells[3].Value = 0;
                ws.Rows[currentExcelRowIndex].Cells[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                ws.Rows[currentExcelRowIndex].Cells[3].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[3].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[currentExcelRowIndex].Cells[3].Style.Font.Size = fontSize;
                ws.Rows[currentExcelRowIndex].Cells[3].Style.Font.Name = fontName;

                ws.Rows[currentExcelRowIndex].Cells[4].Value = Math.Round(FormatHandler.GetDouble(dataRow["basebalance"]), 2);
                if ((double)ws.Rows[currentExcelRowIndex].Cells[4].Value != 0)
                    ws.Rows[currentExcelRowIndex].Cells[4].Style.NumberFormat = formatNumber;
                else ws.Rows[currentExcelRowIndex].Cells[4].Value = 0;
                ws.Rows[currentExcelRowIndex].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                ws.Rows[currentExcelRowIndex].Cells[4].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ws.Rows[currentExcelRowIndex].Cells[4].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
                ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Size = fontSize;
                ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Name = fontName;
            }
            ++currentExcelRowIndex;
            //Grant total
            ws.Rows[currentExcelRowIndex].Height = rowHeight;
            ws.Rows[currentExcelRowIndex].Cells.GetSubrangeAbsolute(currentExcelRowIndex, 0, currentExcelRowIndex, 3).Merged = true;
            ws.Rows[currentExcelRowIndex].Cells[0].Value = "Grand Total";
            ws.Rows[currentExcelRowIndex].Cells[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[currentExcelRowIndex].Cells[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[currentExcelRowIndex].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[currentExcelRowIndex].Cells[0].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[currentExcelRowIndex].Cells[0].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(192, 220, 192));
            ws.Rows[currentExcelRowIndex].Cells[0].Style.Font.Size = fontSize;
            ws.Rows[currentExcelRowIndex].Cells[0].Style.Font.Name = fontName;

            double grantTotal = dtSupperBalance.Rows.Cast<DataRow>().Sum(row => FormatHandler.GetDouble(row["basebalance"], 2));
            ws.Rows[currentExcelRowIndex].Cells[4].Value = Math.Round(grantTotal, 2);
            if (Math.Round(grantTotal, 0) != 0)
            {
                ws.Rows[currentExcelRowIndex].Cells[4].Style.NumberFormat = formatNumber;
            }
            else ws.Rows[currentExcelRowIndex].Cells[4].Value = 0;
            ws.Rows[currentExcelRowIndex].Cells[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            ws.Rows[currentExcelRowIndex].Cells[4].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[currentExcelRowIndex].Cells[4].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.FromArgb(124, 152, 164), LineStyle.Thin);
            ws.Rows[currentExcelRowIndex].Cells[4].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(192, 220, 192));
            ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Size = fontSize;
            ws.Rows[currentExcelRowIndex].Cells[4].Style.Font.Name = fontName;


        End:
            for (int i = 0; i < 5; i++)
            {
                ws.Columns[i].AutoFit();
            }
            //ef.SaveXls(this._saveDirectorySuperBalanceReport + "\\" + this._superBalanceExcelFileName);
            var memoryStream = new MemoryStream();
            ef.SaveXls(memoryStream);
            SendMailExtension.SendMail(
                this._toSuperBalance,
                this._ccSuperBalance,
                _bccSuperBalance,
                this._subjectSuperBalance,
                this._subjectSuperBalance,
                this._contentSuperBalance,
                memoryStream, this._superBalanceExcelFileName);
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
            return mm.ToString() + "/" + dd.ToString() + "/" + yyyy.ToString() + " " + hh.ToString() + ":" + mn.ToString();
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
