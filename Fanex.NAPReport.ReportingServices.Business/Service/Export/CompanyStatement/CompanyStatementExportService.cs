// -----------------------------------------------------------------------
// <copyright file="CompanyStatementExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyStatement
{
    using System;
    using System.Drawing;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyStatementExportService : ICompanyStatementExportService
    {
        private Color _headerColor;
        private XSSFWorkbook _workbook;
        private XSSFSheet _workSheet;
        private Cell _cell;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;

        public CompanyStatementExportService()
        {
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            _headerColor = Color.FromArgb(60, 94, 165);
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\ReportingServices\Settings\CompanyStatement\CompanyStatement.Settings.xml");
            configurationParser.Parse<AppSettings>();
        }

        public void ExportReport(CompanyStatement companyStatement, DateTime currentTime)
        {
            try
            {
                this.CreateSheet();
                this.InitColumnWidth();
                this.ExportReportTitle(currentTime);
                this.ExportReportHeader();
                this.ExportReportContent(companyStatement);
                string parentDirectory = AppSettings.ExportDataDirectoryWithYearMonth;
                string reportName = string.Format(
                    ReportingServicesResource.CompanyStatement_ReportName,
                    currentTime.AddDays(-1).ToShortFormatedDateWithSlash().Replace('/', '_'));
                string reportNameWithExtension = string.Format("{0}.xlsx", reportName);
                SaveFile.SaveExcelFile(parentDirectory, reportNameWithExtension, _workbook, AppSettings.ExcelPassword);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void CreateSheet()
        {
            string sheetName = ReportingServicesResource.CompanyStatement_SheetName;
            _workSheet = _workbook.CreateSheet(sheetName) as XSSFSheet;
        }

        private void InitColumnWidth()
        {
            for (int i = 0; i < 4; i++)
            {
                _workSheet.SetColumnWidth(i, 170 * NPOIConstants.FontWidth);
            }
        }

        private void ExportReportTitle(DateTime currentTime)
        {
            var headerStyle = this.CreateHeaderStyle();
            _cell.CellValue = string.Format(
                    ReportingServicesResource.CompanyStatement_ReportTitle,
                    currentTime.AddDays(-1).ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 3;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportHeader()
        {
            var headerStyle = this.CreateHeaderStyle();
            this.RenderCell(Labels.Date, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Type, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Total, headerStyle);
            _cell.IncreaseColumnIndex();
            this.RenderCell(Labels.Accummulated_Total, headerStyle);
            _cell.ResetColumnIndex();
            _cell.IncreaseRowIndex();
        }

        private void ExportReportContent(CompanyStatement companyStatement)
        {
            if (companyStatement.ReportRows.Count == 0)
            {
                ExportNoDataMessage();
            }
            else
            {
                foreach (var companyStatementItem in companyStatement.ReportRows)
                {
                    this.RenderCell(companyStatementItem.Date, _bodyCellStyleService.CenterTextStyle);
                    _cell.IncreaseColumnIndex();
                    this.RenderCell(companyStatementItem.Type, _bodyCellStyleService.CenterTextStyle);
                    _cell.IncreaseColumnIndex();
                    this.RenderCell(companyStatementItem.Total, _bodyCellStyleService.DecimalNumberStyle);
                    _cell.IncreaseColumnIndex();
                    this.RenderCell(companyStatementItem.AccumulatedTotal, _bodyCellStyleService.DecimalNumberStyle);
                    _cell.ResetColumnIndex();
                    _cell.IncreaseRowIndex();
                }
            }
        }

        private void ExportNoDataMessage()
        {
            _cell.CellValue = Labels.There_is_no_data;
            _cell.EndColumn = _cell.CurrentColumn + 3;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, _bodyCellStyleService.LeftTextStyle);
            _cell.ResetColumnIndex();
        }

        private XSSFCellStyle CreateHeaderStyle()
        {
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(_headerColor, _fontService.WhiteBoldFont);
            return headerStyle;
        }

        private void RenderCell(object cellValue, XSSFCellStyle cellStyle)
        {
            _cell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, cellStyle);
        }
    }
}