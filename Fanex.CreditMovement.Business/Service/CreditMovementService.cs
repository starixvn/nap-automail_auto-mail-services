// -----------------------------------------------------------------------
// <copyright file="CreditMovementService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.CreditMovement.Business.Repositories;
    using Fanex.CreditMovement.Business.Resources;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using Fanex.MailDelivery.Client.Models;
    using OfficeOpenXmlCrypto;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditMovementService : ICreditMovementService
    {
        private readonly ICreditMovementRepository _repository = null;

        private XSSFWorkbook _workbook;
        private BorderInfo _borderInfo;

        private FontAndColorInfo _fontInfo;
        private XSSFFont _font;
        private FontAndColorInfo _headerFontInfo;
        private XSSFFont _headerFont;

        private CellInfo _cellInfo;
        private XSSFDataFormat _dataFormat;

        private BackgroundStyleInfo _normalBackgroundInfo;

        private XSSFCellStyle _centerTextStyle;
        private XSSFCellStyle _numberStyle;
        private XSSFCellStyle _decimalNumberStyle;

        private AlignmentInfo _centerAlignmentInfo;
        private AlignmentInfo _rightAlignmentInfo;

        public CreditMovementService()
        {
            _repository = new CreditMovementRepository();
            this.InitAlignmentInfo();
            this.InitBorderInfo();
            this.InitFontInfo();
            this.InitBackGroundInfo();
        }

        /// <summary>
        /// Processes to send email.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public Mail ProcessToSendEmail(GetCreditMovementCriteria criteria)
        {
            var report = _repository.GetReportData(criteria);
            if (report.Count > 0)
            {
                string reportUrl = ExportReport(report, criteria);
                return Mailing.SendCreditMovementReportEmail(reportUrl, criteria);
            }
            return new Mail();
        }

        /// <summary>
        /// Exports the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The report URL.</returns>
        private string ExportReport(List<CreditMovement> report, GetCreditMovementCriteria criteria)
        {
            string sheetName = "Credit Movement";
            string reportTitle = string.Empty;
            _workbook = new XSSFWorkbook();
            XSSFSheet worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            _dataFormat = _workbook.CreateDataFormat() as XSSFDataFormat;
            this.InitColumnWidth(worksheet);
            this.InitFont();
            this.InitCellStyleInfo();
            _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);
            ExportReportHeader(worksheet, criteria);
            ExportReportContent(worksheet, report);
            worksheet.CreateFreezePane(0, 4, 0, 4);            

            string reportName = string.Format("Credit_Movement_Report_On_{0}.xlsx", criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "-"));            
            var fullReportPath = string.Empty;

            string parentDirectory = AppSettings.ExportDataDirectory;
            fullReportPath = parentDirectory + @"\" + reportName;
            SaveFile.SaveExcelFile(parentDirectory, reportName, _workbook, AppSettings.ExcelPassword);

            GC.Collect();

            return fullReportPath;
        }

        /// <summary>
        /// Initializes the width of the column.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void InitColumnWidth(XSSFSheet worksheet)
        {
            int numberColumn = 6;
            for (int i = 0; i < numberColumn; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }

            worksheet.SetColumnWidth(0, 60 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(1, 150 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(3, 150 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(4, 320 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(5, 200 * NPOIConstants.FontWidth);
            worksheet.SetColumnWidth(6, 200 * NPOIConstants.FontWidth);
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void ExportReportHeader(XSSFSheet worksheet, GetCreditMovementCriteria criteria)
        {
            Color headerBackgroundColor = Color.FromArgb(60, 94, 165);
            BackgroundStyleInfo headerBackgroundInfo = new BackgroundStyleInfo(headerBackgroundColor, (int)FillPattern.SolidForeground);
            XSSFCellStyle headerStyle = NPOIHighVersion.CreateCellStyle(_workbook, _borderInfo, headerBackgroundInfo, _centerAlignmentInfo, _headerFont);
            XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

            _cellInfo.CellValue = string.Format("Accounts which have been allocated with more than 300KRM credits for SMA + Member  Line on {0}", criteria.ToDate.ToShortFormatedDate());
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 6;
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            NPOIUtility.ResetColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "#";
            _cellInfo.EndRow = _cellInfo.CurrentRow + 1;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Date";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Currency";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Site";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Super/Master/Agent/Member";
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Credit Movement";
            _cellInfo.EndRow = _cellInfo.CurrentRow;
            _cellInfo.EndColumn = _cellInfo.CurrentColumn + 1;
            NPOIHighVersion.RenderMergeCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);

            _cellInfo.CurrentColumn = 5;
            _cellInfo.EndColumn = 5;
            _cellInfo.CellValue = "Original Currency";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

            _cellInfo.CellValue = "Base Currency";
            NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, headerStyle);
            NPOIUtility.IncreaseRowIndex(ref _cellInfo);
        }

        /// <summary>
        /// Exports the content of the report.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="report">The report.</param>
        private void ExportReportContent(XSSFSheet worksheet, List<CreditMovement> report)
        {
            for (int i = 0; i < report.Count; i++)
            {
                var currentRow = report[i];
                NPOIUtility.ResetColumnIndex(ref _cellInfo);
                XSSFRow row = worksheet.GetRow(_cellInfo.CurrentRow) as XSSFRow ?? worksheet.CreateRow(_cellInfo.CurrentRow) as XSSFRow;

                _cellInfo.CellValue = i + 1;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _numberStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.Date;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.Currency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.Site;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.DownLinesInformation;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _centerTextStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.OriginalCurrency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                NPOIUtility.IncreaseColumnIndex(ref _cellInfo);

                _cellInfo.CellValue = currentRow.BaseCurrency;
                NPOIHighVersion.RenderCell(ref worksheet, ref row, ref _cellInfo, _decimalNumberStyle);
                NPOIUtility.IncreaseRowIndex(ref _cellInfo);
            }
        }

        /// <summary>
        /// Initializes the border style information.
        /// </summary>
        private void InitBorderInfo()
        {
            _borderInfo = new BorderInfo(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                Color.Gray,
                Color.Gray,
                Color.Gray);
        }

        /// <summary>
        /// Initializes the font information.
        /// </summary>
        private void InitFontInfo()
        {
            _fontInfo = new FontAndColorInfo(
                    NPOIConstants.FontName,
                    NPOIConstants.FontSize,
                    Color.Black);

            _headerFontInfo = new FontAndColorInfo(
                NPOIConstants.FontName,
                NPOIConstants.FontSize,
                Color.White);
        }

        /// <summary>
        /// Initializes the font.
        /// </summary>
        private void InitFont()
        {
            _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, false);
            _headerFont = NPOIHighVersion.CreateFont(_workbook, _headerFontInfo, true);
        }

        /// <summary>
        /// Initializes the alignment information.
        /// </summary>
        private void InitAlignmentInfo()
        {
            _centerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);
            _rightAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Right,
                (int)VerticalAlignment.Center);
        }

        /// <summary>
        /// Initializes the background information.
        /// </summary>
        private void InitBackGroundInfo()
        {
            _normalBackgroundInfo = new BackgroundStyleInfo(Color.White, (int)FillPattern.SolidForeground);
        }

        /// <summary>
        /// Initializes the cell style information.
        /// </summary>
        private void InitCellStyleInfo()
        {
            _numberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalBackgroundInfo,
                _centerAlignmentInfo,
                _font);
            _numberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);

            _decimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalBackgroundInfo,
                _rightAlignmentInfo,
                _font);
            _decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);

            _centerTextStyle = NPOIHighVersion.CreateCellStyle(
               _workbook,
               _borderInfo,
               _normalBackgroundInfo,
               _centerAlignmentInfo,
               _font);
        }

        private string CreateCreditMovementReportDirectory(GetCreditMovementCriteria criteria)
        {
            string parentDirectory = AppSettings.ExportDataDirectory;
            DirectoryExtensions.CheckAndCreateDirectory(parentDirectory);
            string reportFrom = criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            string reportTo = criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "-");
            string reportFolder = string.Format("CreditMovement_Report_On_{0}", reportTo);
            string reportDirectory = Path.Combine(parentDirectory, reportFolder);
            DirectoryExtensions.CheckAndCreateDirectory(reportDirectory);
            return reportDirectory;
        }
    }
}