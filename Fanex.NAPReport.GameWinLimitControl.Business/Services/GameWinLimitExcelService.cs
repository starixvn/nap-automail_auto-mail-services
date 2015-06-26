// -----------------------------------------------------------------------
// <copyright file="GameWinLimitExcelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimitExcelService : IGameWinLimitExcelService
    {
        private Color _headerColor;

        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiAlignmentService _alignmentService;
        private XSSFFont _regularFont;
        private XSSFCellStyle _headerStyle;
        private string _reportPath;

        public GameWinLimitExcelService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            _headerColor = Color.FromArgb(60, 94, 165); 
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();
            _headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
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

        public void ExportReportToExcel(List<GameWinLimit> games, CriteriaBase criteria)
        {
            if (games.Count == 0)
            {
                return;
            }

            CreateSheet();
            InitColumnWidth();
            ExportReport(games);
            FileName = "Blocked_Account_List_" + DateTime.Now.ToString(DateTimeFormatConstants.YEAR_MONTH_DATE_MINUTE) + ".xlsx";
            DirectoryOfFileName = _reportPath = AppSettings.ExportDataDirectoryWithYearMonthDay + @"\" + FileName;
            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectoryWithYearMonthDay, FileName, this._workbook, AppSettings.ExcelPasswordProtected);
        }

        private void CreateSheet()
        {
            string sheetName = "Blocked_Account_List";
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(0, 1);
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 50 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 100 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 200 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(5, 100 * NPOIConstants.FontWidth);
        }

        private void ExportReport(List<GameWinLimit> games)
        {
            if (games.Count > 0)
            {                
                this.ExportSportBookContent(games);                
            }           
        }

        private void ExportReportHeader()
        {            
            _cell.CellValue = "#";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.IncreaseColumnIndex();                        
            _cell.CellValue = "Username";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = "Currency";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = "Win Limit";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = "Winning Amount";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.IncreaseColumnIndex();
            _cell.CellValue = "Percentage";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _headerStyle);

            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportSportBookContent(List<GameWinLimit> games)
        {
            XSSFCellStyle textBoldStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _fontService.BlackBoldFont);
            XSSFCellStyle textCenterStyle = _bodyCellStyleService.CenterTextStyle;
            XSSFCellStyle decimalNumberStyle = _bodyCellStyleService.DecimalNumberStyle;
            XSSFCellStyle percentageNumberStyle = _bodyCellStyleService.PositivePercentageStyle;
            int indexMember = 0;
            int indexGame = 0;
            foreach (var game in games)
            {
                int numberOfMembersBlockedSuccessful = game.Members.Count(s => s.BlockStatus == true);
                if (numberOfMembersBlockedSuccessful == 0)
                {
                    continue;
                }

                this.ExportReportHeader();

                indexMember = 0;
                indexGame++;

                _cell.CellValue = indexGame + ". " + game.ProductName;
                _cell.EndColumn = _cell.CurrentColumn + 5;
                NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, textBoldStyle);
                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();

                foreach (var member in game.Members)
                {
                    if (!member.BlockStatus)
                    {
                        continue;
                    }

                    indexMember++;

                    _cell.CellValue = indexMember; 
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textCenterStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = member.Username;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textCenterStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = member.Currency;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textCenterStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = member.MaxWinLimit;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = member.TotalWinAmt;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);

                    _cell.IncreaseColumnIndex();
                    _cell.CellValue = member.Percentage;
                    NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, percentageNumberStyle);

                    _cell.ResetColumnIndex();
                    _cell.IncreaseRowIndex();
                }

                _cell.IncreaseRowIndex();
            }
        }
    }
}
