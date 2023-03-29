// -----------------------------------------------------------------------
// <copyright file="CreditMovementService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using System.IO;

namespace Fanex.CreditMovement.Business.Service
{
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.CreditMovement.Business.Repositories;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditMovementService : ICreditMovementService
    {
        private readonly ICreditMovementRepository _repository = null;
        private readonly XSSFWorkbook _workbook;
        private readonly Cell _cell;
        private readonly INpoiNewFormatFontService _fontService;
        private readonly INpoiNewFormatBodyCellStyleService _bodyCellStyleService;
        private readonly INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private readonly XSSFFont _regularFont;

        private XSSFSheet _workSheet;

        public CreditMovementService()
        {
            _repository = new CreditMovementRepository();
            _cell = new Cell();
            _workbook = new XSSFWorkbook();
            _fontService = new NpoiNewFormatFontService(_workbook);
            _regularFont = _fontService.RegularFont;
            _bodyCellStyleService = new NpoiNewFormatBodyCellStyleService(_workbook, _regularFont);
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(_workbook);
        }

        public string DirectoryOfFileName { get; set; }

        public string FileName { get; set; }

        public void ExportExcel(GetCreditMovementCriteria criteria)
        {
            var report = _repository.GetReportData(criteria);
            var json = JsonConvert.SerializeObject(report, Formatting.Indented);
            Directory.CreateDirectory(AppSettings.ExportDataDirectory);
            File.WriteAllText($"{AppSettings.ExportDataDirectory}/CreditMovement.json", json);
            if (report.Count > 0)
            {
                ExportReport(report, criteria);
            }
        }

        private void ExportReport(List<CreditMovement> report, GetCreditMovementCriteria criteria)
        {
            string sheetName = "Credit Movement";

            _workSheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            this.InitColumnWidth();
            ExportReportHeader(criteria);
            ExportReportContent(report);
            _workSheet.CreateFreezePane(0, 4, 0, 4);

            FileName = string.Format("Credit_Movement_Report_For_{0}.xlsx", criteria.ToDate.ToString("yyyy_MM_dd"));
            string parentDirectory = AppSettings.ExportDataDirectoryWithYearMonth;
            DirectoryOfFileName = parentDirectory + @"\" + FileName;

            SaveFile.SaveExcelFile(AppSettings.ExportDataDirectory, parentDirectory, FileName, _workbook, AppSettings.ExcelPassword);
        }

        private void InitColumnWidth()
        {
            int numberColumn = 6;
            for (int i = 0; i < numberColumn; i++)
            {
                _workSheet.SetColumnWidth(i, 130 * NpoiConstants.FontWidth);
            }

            _workSheet.SetColumnWidth(0, 60 * NpoiConstants.FontWidth);
            _workSheet.SetColumnWidth(1, 150 * NpoiConstants.FontWidth);
            _workSheet.SetColumnWidth(3, 150 * NpoiConstants.FontWidth);
            _workSheet.SetColumnWidth(4, 320 * NpoiConstants.FontWidth);
            _workSheet.SetColumnWidth(5, 200 * NpoiConstants.FontWidth);
            _workSheet.SetColumnWidth(6, 200 * NpoiConstants.FontWidth);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void ExportReportHeader(GetCreditMovementCriteria criteria)
        {
            Color headerBackgroundColor = Color.FromArgb(60, 94, 165);
            XSSFCellStyle headerStyle = _customCellStyleService.CreateCenterTextStyle(headerBackgroundColor, _fontService.WhiteBoldFont);

            _cell.CellValue = string.Format("Accounts which have been allocated with more than 300KRM credits for SMA + Member  Line on {0}", criteria.ToDate.ToShortFormatedDate());
            _cell.EndColumn = _cell.CurrentColumn + 6;
            _cell.EndRow = _cell.CurrentRow + 1;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseRowIndex();
            _cell.ResetColumnIndex();

            _cell.CellValue = "#";
            _cell.EndRow = _cell.CurrentRow + 1;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Date";
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Currency";
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Site";
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Super/Master/Agent/Member";
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Credit Movement";
            _cell.EndRow = _cell.CurrentRow;
            _cell.EndColumn = _cell.CurrentColumn + 1;
            NpoiNewFormatExcelExtension.RenderMergeCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseRowIndex();

            _cell.CurrentColumn = 5;
            _cell.EndColumn = 5;
            _cell.CellValue = "Original Currency";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseColumnIndex();

            _cell.CellValue = "Base Currency";
            NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, headerStyle);
            _cell.IncreaseRowIndex();
        }

        private void ExportReportContent(List<CreditMovement> report)
        {
            for (int i = 0; i < report.Count; i++)
            {
                var currentRow = report[i];
                _cell.ResetColumnIndex();

                _cell.CellValue = i + 1;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.CenterNumberStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.Date;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.CenterTextStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.Currency;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.CenterTextStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.Site;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.CenterTextStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.DownLinesInformation;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.CenterTextStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.OriginalCurrency;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.DecimalNumberStyle);
                _cell.IncreaseColumnIndex();

                _cell.CellValue = currentRow.BaseCurrency;
                NpoiNewFormatExcelExtension.RenderCell(_workSheet, _cell, _bodyCellStyleService.DecimalNumberStyle);
                _cell.IncreaseRowIndex();
            }
        }
    }
}