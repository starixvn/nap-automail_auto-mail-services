// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.ReportingService.Business.Service.Implement.CompanyBalance
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.ReportingService.Business.Common;
    using Fanex.ReportingService.Business.Criteria.CompanyBalance;
    using Fanex.ReportingService.Business.Resources;
    using Fanex.ReportingService.Business.Service.Interface;
    using Fanex.ReportingService.Core.Extensions;
    using Fanex.ReportingService.Core.Services.Impl;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using WebConfigurationManager = System.Web.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalanceService : ServiceBase, ICompanyBalanceService
    {
        private const short EvenRowColorIndex = 29;

        /// <summary>
        /// Exports the company balance report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportCompanyBalanceReport(GetCompanyBalanceCriteria criteria)
        {
            FileStream fileStream = null;
            try
            {
                HSSFWorkbook excelFile = new HSSFWorkbook();
                this.SetColorAtIndex(excelFile);
                int currentRow = 0;
                string sheetName = ReportingServiceResources.CompanyBalance_SheetName;
                ISheet worksheet = excelFile.CreateSheet(sheetName);
                this.ExportReportHeader(excelFile, ref worksheet, ref currentRow, criteria);

                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IDataReader reader = objectDb.ExecuteReader();
                    ExportReportContent(reader, excelFile, ref worksheet, ref currentRow);
                }

                var reportDirectory = this.CreateCompanyBalanceReportDirectory(criteria);
                var reportName = string.Format(
                ReportingServiceResources.CompanyBalance_ReportName,
                criteria.ReportDate.ToShortFormatedDateWithSlash().Replace('/', '_'));
                var reportPath = Path.Combine(reportDirectory, reportName);
                var fullReportPath = string.Format("{0}.{1}", reportPath, "xls");

                fileStream = new FileStream(
                fullReportPath,
                FileMode.Create,
                FileAccess.Write);
                excelFile.Write(fileStream);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                fileStream.Close();
                GC.Collect();
            }
        }

        /// <summary>
        /// Exports the report header.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="currentRow">The current row.</param>
        /// <param name="criteria">The criteria.</param>
        private void ExportReportHeader(HSSFWorkbook excelFile, ref ISheet worksheet, ref int currentRow, GetCompanyBalanceCriteria criteria)
        {
            int currentColumn = 0;
            IRow row = worksheet.CreateRow(currentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(excelFile);

            worksheet.SetColumnWidth(0, 80 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(1, 170 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(2, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(3, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(4, 250 * NpoiExcelExtensions.FontWidth);

            string reportTitle = string.Format(
                    ReportingServiceResources.CompanyBalance_ReportTitle,
                    criteria.ReportDate.ToShortFormatedDate());
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn + 4, reportTitle, headerStyle);
            currentRow += 2;

            row = worksheet.CreateRow(currentRow);
            currentColumn = 0;
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn, "No", headerStyle);
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn, "Balance-Up Day", headerStyle);
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn, "Company Balance Before BU", headerStyle);
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn, "Company Balance After BU", headerStyle);
            NpoiExcelExtensions.RenderCell(ref worksheet, ref row, currentRow, ref currentColumn, currentRow, currentColumn, "Difference", headerStyle);
            currentRow++;
        }

        /// <summary>
        /// Exports the content of the report.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="currentRow">The current row.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private void ExportReportContent(IDataReader reader, HSSFWorkbook excelFile, ref ISheet worksheet, ref int currentRow)
        {
            ICellStyle textStyle = NpoiExcelExtensions.TextStyle(excelFile, HSSFColor.White.Index, false, NpoiExcelExtensions.Alignment.Center);
            ICellStyle evenRowTextStyle = NpoiExcelExtensions.TextStyle(excelFile, EvenRowColorIndex, false, NpoiExcelExtensions.Alignment.Center);
            ICellStyle intNumberStyle = NpoiExcelExtensions.IntStyle(excelFile, HSSFColor.White.Index, false, NpoiExcelExtensions.Alignment.Center);
            ICellStyle evenRowIntNumberStyle = NpoiExcelExtensions.IntStyle(excelFile, EvenRowColorIndex, false, NpoiExcelExtensions.Alignment.Center);
            ICellStyle decimalNumberStyle = NpoiExcelExtensions.DecimalStyle(excelFile, HSSFColor.White.Index, false);
            ICellStyle evenRowDecimalNumberStyle = NpoiExcelExtensions.DecimalStyle(excelFile, EvenRowColorIndex, false);

            int currentColumn = 0;
            int columnIndex = 0;
            while (reader.Read())
            {
                IRow row = worksheet.CreateRow(currentRow);
                currentColumn = 0;
                NpoiExcelExtensions.RenderCell(
                    ref worksheet,
                    ref row,
                    currentRow,
                    ref currentColumn,
                    currentRow,
                    currentColumn,
                    columnIndex + 1,
                    columnIndex % 2 == 0 ? evenRowIntNumberStyle : intNumberStyle);
                NpoiExcelExtensions.RenderCell(
                    ref worksheet,
                    ref row,
                    currentRow,
                    ref currentColumn,
                    currentRow,
                    currentColumn,
                    Converter.ToDateTime(reader["BalanceUpDay"]).ToShortFormatedDate(),
                    columnIndex % 2 == 0 ? evenRowTextStyle : textStyle);
                NpoiExcelExtensions.RenderCell(
                    ref worksheet,
                    ref row,
                    currentRow,
                    ref currentColumn,
                    currentRow,
                    currentColumn,
                    Converter.ToDecimal(reader["CompBalBefore"]),
                    columnIndex % 2 == 0 ? evenRowDecimalNumberStyle : decimalNumberStyle);
                NpoiExcelExtensions.RenderCell(
                    ref worksheet,
                    ref row,
                    currentRow,
                    ref currentColumn,
                    currentRow,
                    currentColumn,
                     Converter.ToDecimal(reader["CompBalAfter"]),
                    columnIndex % 2 == 0 ? evenRowDecimalNumberStyle : decimalNumberStyle);
                NpoiExcelExtensions.RenderCell(
                    ref worksheet,
                    ref row,
                    currentRow,
                    ref currentColumn,
                    currentRow,
                    currentColumn,
                    Converter.ToDecimal(reader["DIFFERENCE"]),
                    columnIndex % 2 == 0 ? evenRowDecimalNumberStyle : decimalNumberStyle);
                columnIndex++;
                currentRow++;
            }

            reader.Close();
        }

        private void SetColorAtIndex(HSSFWorkbook excelFile)
        {
            HSSFPalette palette = excelFile.GetCustomPalette();
            palette.SetColorAtIndex(EvenRowColorIndex, (byte)228, (byte)242, (byte)253);
        }

        /// <summary>
        /// Creates the company statement report directory.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Company statement report directory.</returns>
        private string CreateCompanyBalanceReportDirectory(GetCompanyBalanceCriteria criteria)
        {
            string reportDataDirectory = WebConfigurationManager.WebConfigurationManager.AppSettings["ExportDataDirectory"];
            DirectoryExtensions.CheckAndCreateDirectory(reportDataDirectory);

            string directoryByYearAndMonth = string.Format(
                "{0}{1}",
                criteria.ReportDate.ToString("yyyy"),
                criteria.ReportDate.ToString("MM"));
            string directoryByYearAndMonthPath = Path.Combine(reportDataDirectory, directoryByYearAndMonth);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByYearAndMonthPath);

            string directoryByReport = "CompanyBalanceDifference";
            string directoryByReportPath = Path.Combine(directoryByYearAndMonthPath, directoryByReport);
            DirectoryExtensions.CheckAndCreateDirectory(directoryByReportPath);

            return directoryByReportPath;
        }
    }
}
