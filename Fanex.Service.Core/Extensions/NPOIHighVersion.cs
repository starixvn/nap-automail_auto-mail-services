// -----------------------------------------------------------------------
// <copyright file="NPOIHighVersion.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    /// <summary>
    /// Excel extension.
    /// </summary>
    public static class NpoiHighVersion
    {
        /// <summary>
        /// Creates the cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="borderInfo">The border information.</param>
        /// <param name="backgroundStyleInfo">The background style information.</param>
        /// <param name="alignmentInfo">The alignment information.</param>
        /// <param name="font">The font of text.</param>
        /// <returns>The cell style.</returns>
        public static XSSFCellStyle CreateCellStyle(
            XSSFWorkbook workbook,
            BorderInfo borderInfo,
            BackgroundStyleInfo backgroundStyleInfo,
            AlignmentInfo alignmentInfo,
            XSSFFont font)
        {
            XSSFCellStyle cellStyle = workbook.CreateCellStyle() as XSSFCellStyle;

            cellStyle.BorderLeft = (BorderStyle)borderInfo.LeftBorderStyle;
            cellStyle.BorderTop = (BorderStyle)borderInfo.TopBorderStyle;
            cellStyle.BorderRight = (BorderStyle)borderInfo.RightBorderStyle;
            cellStyle.BorderBottom = (BorderStyle)borderInfo.BottomBorderStyle;
            cellStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(borderInfo.BottomBoderColor));
            cellStyle.SetBorderColor(BorderSide.LEFT, new XSSFColor(borderInfo.LeftBoderColor));
            cellStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(borderInfo.RightBoderColor));
            cellStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(borderInfo.TopBoderColor));

            cellStyle.SetFillForegroundColor(new XSSFColor(backgroundStyleInfo.FillForegroundColor));
            cellStyle.FillPattern = (FillPattern)backgroundStyleInfo.FillPattern;

            cellStyle.VerticalAlignment = (VerticalAlignment)alignmentInfo.VerticalAlign;
            cellStyle.Alignment = (HorizontalAlignment)alignmentInfo.HorizontalAlign;

            cellStyle.SetFont(font);

            return cellStyle;
        }

        /// <summary>
        /// Creates the font.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="fontInfo">The font information.</param>
        /// <param name="isBold">If set to <c>true</c> [is bold].</param>
        /// <returns>The information of font.</returns>
        public static XSSFFont CreateFont(
            XSSFWorkbook workbook,
            FontAndColorInfo fontInfo,
            bool isBold)
        {
            XSSFFont font = workbook.CreateFont() as XSSFFont;
            font.FontName = fontInfo.FontName;
            font.FontHeightInPoints = fontInfo.FontSize;
            font.SetColor(new XSSFColor(fontInfo.FontColor));

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            return font;
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="row">The row of sheet.</param>
        /// <param name="cellInfo">The cell information.</param>
        /// <param name="cellStyle">The cell style.</param>
        public static void RenderCell(
            ref XSSFSheet worksheet,
            ref XSSFRow row,
            ref CellInfo cellInfo,
            XSSFCellStyle cellStyle)
        {
            for (var i = cellInfo.CurrentRow; i <= cellInfo.EndRow; i++)
            {
                row = worksheet.GetRow(i) as XSSFRow ?? worksheet.CreateRow(i) as XSSFRow;
                row.Height = NpoiConstants.RowHeight;

                for (var j = cellInfo.CurrentColumn; j <= cellInfo.EndColumn; j++)
                {
                    XSSFCell cell = row.GetCell(j) as XSSFCell ?? row.CreateCell(j) as XSSFCell;
                    if (i == cellInfo.CurrentRow && j == cellInfo.CurrentColumn)
                    {
                        NpoiUtility.SetCellValue(cell, cellInfo.CellValue);
                    }

                    if (i == cellInfo.CurrentRow ||
                        i == cellInfo.EndRow ||
                        j == cellInfo.CurrentColumn ||
                        j == cellInfo.EndColumn)
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }
        }

        public static void RenderMergeCell(
          ref XSSFSheet worksheet,
          ref XSSFRow row,
          ref CellInfo cellInfo,
          XSSFCellStyle cellStyle)
        {
            RenderCell(ref worksheet, ref row, ref cellInfo, cellStyle);
            worksheet.AddMergedRegion(new CellRangeAddress(cellInfo.CurrentRow, cellInfo.EndRow, cellInfo.CurrentColumn, cellInfo.EndColumn));
        }
    }
}