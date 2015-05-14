// -----------------------------------------------------------------------
// <copyright file="NpoiOldFormatExcelExtension.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NpoiOldFormatExcelExtension
    {
        public static IFont CreateFont(HSSFWorkbook workbook, FontInformation fontInformation)
        {
            IFont font = workbook.CreateFont();
            font.FontName = fontInformation.FontName;
            font.FontHeightInPoints = fontInformation.FontSize;
            font.Color = fontInformation.FontColor;
            return font;
        }

        public static IFont CreateBoldFont(HSSFWorkbook workbook, FontInformation fontInformation)
        {
            IFont font = CreateFont(workbook, fontInformation);
            font.Boldweight = (short)FontBoldWeight.Bold;
            return font;
        }

        public static void RenderCell(
            ISheet worksheet,
            Cell cell,
            ICellStyle cellStyle)
        {
            IRow currentRow = worksheet.GetRow(cell.CurrentRow) ?? worksheet.CreateRow(cell.CurrentRow);
            currentRow.Height = NPOIConstants.RowHeight;
            ICell currentCell = currentRow.GetCell(cell.CurrentColumn) ?? currentRow.CreateCell(cell.CurrentColumn);
            NPOIUtility.SetCellValue(currentCell, cell.CellValue);
            currentCell.CellStyle = cellStyle;
        }

        public static void RenderMergeCell(
            ISheet worksheet,
            Cell cell,
            ICellStyle cellStyle)
        {
            for (var rowIndex = cell.CurrentRow; rowIndex <= cell.EndRow; rowIndex++)
            {
                IRow currentRow = worksheet.GetRow(rowIndex) ?? worksheet.CreateRow(rowIndex);
                currentRow.Height = NPOIConstants.RowHeight;

                for (var cellIndex = cell.CurrentColumn; cellIndex <= cell.EndColumn; cellIndex++)
                {
                    ICell currentCell = currentRow.GetCell(cellIndex) ?? currentRow.CreateCell(cellIndex);
                    if (rowIndex == cell.CurrentRow && rowIndex == cell.CurrentColumn)
                    {
                        NPOIUtility.SetCellValue(currentCell, cell.CellValue);
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