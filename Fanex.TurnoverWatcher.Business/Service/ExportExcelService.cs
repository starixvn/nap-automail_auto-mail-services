namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.NAPReport.TurnoverWatcher.Business.Resources;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    public class ExportExcelService : IExportExcelService
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

        private XSSFCellStyle leagueStyle;
        private XSSFCellStyle eventStyle;
        private XSSFCellStyle percentStyle;
        private XSSFCellStyle percentRedColorStyle;
        private XSSFCellStyle decimalNumberStyle;
        private XSSFCellStyle textStyle;

        public ExportExcelService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            this.CreateColor();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            _alignmentService = new NpoiNewFormatAlignmentService();

            leagueStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, CreatFontLeague());
            eventStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _fontService.BlackBoldFont);
            percentStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _fontService.RegularFont);
            percentRedColorStyle = _customCellStyleService.CreatePercentageStyle(Color.Red, _fontService.WhiteBoldFont);
            decimalNumberStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _fontService.RegularFont);
            textStyle = _bodyCellStyleService.CenterTextStyle;            
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

        public void ExportExcel(List<TurnoverInfo> report)
        {
            CreateSheet();
            InitColumnWidth();
            ExportReport(report);
            FileName = CreateReportNameWithExtension();
            CreateReportPath(AppSettings.ExportDataDirectoryWithYearMonthDay, FileName);
            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectoryWithYearMonthDay, FileName, this._workbook, AppSettings.ExcelPasswordProtected);
        }

        private void ExportReport(List<TurnoverInfo> report)
        {
            if (report.Count > 0)
            {
                this.ExportReportHeader();
                this.ExportSportBookContent(report);
            }
        }

        private void ExportReportHeader()
        {
            var headerStyle = CreateHeaderStyle();
            var reportTile = string.Format(Resources.TurnoverWatcher_ReportTitle, DateTime.Now.ToShortDateString());
            _cell.EndColumn = _cell.CurrentColumn + 2;
            _cell.CellValue = reportTile;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.CurrentRow += 2;
            _cell.EndRow = _cell.CurrentRow;
            _cell.CellValue = Resources.Sport;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.League;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.MatchId;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.Event;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.EventDate;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.SmaName;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.Currency;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.MemberTurnover;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.CompanyTurnover;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.MaxTurnover;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();
            _cell.CellValue = Resources.PercentageUsed;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);

            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportSportBookContent(List<TurnoverInfo> report)
        {
            foreach (var sportbook in report)
            {
                _cell.CellValue = sportbook.MatchInfo.SportName;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.MatchInfo.LeagueName;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, leagueStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.MatchInfo.MatchId;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = string.Format(Resources.HomeVsAway, sportbook.MatchInfo.HomeName, sportbook.MatchInfo.AwayName);
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, eventStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.MatchInfo.EventDate.ToShortDateString();
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.CustomerInfo.CustomerName;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.CustomerInfo.Currency;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, textStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.CustomerInfo.MemberTurnover;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.CustomerInfo.CompanyTurnover;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);
                _cell.IncreaseColumnIndex();
                _cell.CellValue = sportbook.CustomerInfo.MaxTurnover;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, decimalNumberStyle);
                
                _cell.IncreaseColumnIndex();
                var _percentStyle = percentStyle;
                if (sportbook.CustomerInfo.PercentageUsed >= 100)
                {
                    _cell.CellValue = 1;
                    _percentStyle = percentRedColorStyle;
                }
                else if (sportbook.CustomerInfo.PercentageUsed >= 50)
                {
                    _cell.CellValue = sportbook.CustomerInfo.PercentageUsed / 100;
                }
                else
                {
                    _cell.CellValue = string.Empty;
                }

                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _percentStyle);

                _cell.ResetColumnIndex();
                _cell.IncreaseRowIndex();
            }
        }

        private XSSFFont CreatFontLeague()
        {
            var font = _fontService.CreateFont(new FontInformation());
            var color = new XSSFColor(Color.FromArgb(167, 76, 0));

            font.IsBold = true;
            font.SetColor(color);

            return font;
        }

        private void CreateColor()
        {
            _headerColor = Color.FromArgb(60, 94, 165);
        }

        private void CreateSheet()
        {
            string sheetName = Resources.TurnoverWatcher_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
            _workSheet.CreateFreezePane(4, 3);
        }

        private void InitColumnWidth()
        {
            _workSheet.SetColumnWidth(0, 90 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 260 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(2, 90 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 260 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 90 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(5, 90 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(6, 90 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(7, 140 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(8, 140 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(9, 120 * NPOIConstants.FontWidth);
            _workSheet.SetColumnWidth(10, 120 * NPOIConstants.FontWidth);
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private string CreateReportNameWithExtension()
        {
            string reportName = string.Format(Resources.TurnoverWatcher_ReportName, DateTime.Now.ToString("yyyy_MM_dd"));

            return reportName;
        }

        private void CreateReportPath(string exportDataDirectory, string reportNameWithExtension)
        {
            DirectoryOfFileName = Path.Combine(exportDataDirectory, reportNameWithExtension);
        }        
    }
}