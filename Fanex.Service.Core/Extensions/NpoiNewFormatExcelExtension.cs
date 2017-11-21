// -----------------------------------------------------------------------
// <copyright file="NpoiNewFormatExcelExtension.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NpoiNewFormatExcelExtension
    {
        public static XSSFFont CreateFont(XSSFWorkbook workbook, FontInformation fontInformation)
        {
            XSSFFont font = workbook.CreateFont() as XSSFFont;
            font.FontName = fontInformation.FontName;
            font.FontHeightInPoints = fontInformation.FontSize;
            font.SetColor(new XSSFColor(fontInformation.FontColor));
            return font;
        }

        public static XSSFFont CreateBoldFont(XSSFWorkbook workbook, FontInformation fontInformation)
        {
            XSSFFont font = CreateFont(workbook, fontInformation);
            font.Boldweight = (short)FontBoldWeight.Bold;
            return font;
        }

        public static void RenderCell(
            XSSFSheet worksheet,
            Cell cell,
            XSSFCellStyle cellStyle)
        {
            XSSFRow currentRow = worksheet.GetRow(cell.CurrentRow) as XSSFRow ?? worksheet.CreateRow(cell.CurrentRow) as XSSFRow;
            currentRow.Height = NpoiConstants.RowHeight;
            XSSFCell currentCell = currentRow.GetCell(cell.CurrentColumn) as XSSFCell ?? currentRow.CreateCell(cell.CurrentColumn) as XSSFCell;
            NpoiUtility.SetCellValue(currentCell, cell.CellValue);
            currentCell.CellStyle = cellStyle;
        }

        public static void RenderMergeCell(
          XSSFSheet worksheet,
          Cell cell,
          XSSFCellStyle cellStyle)
        {
            for (var rowIndex = cell.CurrentRow; rowIndex <= cell.EndRow; rowIndex++)
            {
                XSSFRow currentRow = worksheet.GetRow(rowIndex) as XSSFRow ?? worksheet.CreateRow(rowIndex) as XSSFRow;
                currentRow.Height = NpoiConstants.RowHeight;

                for (var cellIndex = cell.CurrentColumn; cellIndex <= cell.EndColumn; cellIndex++)
                {
                    XSSFCell currentCell = currentRow.GetCell(cellIndex) as XSSFCell ?? currentRow.CreateCell(cellIndex) as XSSFCell;
                    if (rowIndex == cell.CurrentRow && cellIndex == cell.CurrentColumn)
                    {
                        NpoiUtility.SetCellValue(currentCell, cell.CellValue);
                    }

                    if (rowIndex == cell.CurrentRow ||
                        rowIndex == cell.EndRow ||
                        cellIndex == cell.CurrentColumn ||
                        cellIndex == cell.EndColumn)
                    {
                        currentCell.CellStyle = cellStyle;
                    }
                }
            }

            worksheet.AddMergedRegion(new CellRangeAddress(cell.CurrentRow, cell.EndRow, cell.CurrentColumn, cell.EndColumn));
        }
    }
}