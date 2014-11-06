// -----------------------------------------------------------------------
// <copyright file="NPOIExtension.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Excel extension.
    /// </summary>
    public static class NPOIExtension
    {
        /// <summary>
        /// Creates the cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="borderStyleInfo">The border style information.</param>
        /// <param name="backgroundInfo">The background information.</param>
        /// <param name="alignmentInfo">The alignment information.</param>
        /// <param name="font">The font information.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle CreateCellStyle(
            HSSFWorkbook workbook,
            BorderStyleInfo borderStyleInfo,
            BackgroundInfo backgroundInfo,
            AlignmentInfo alignmentInfo,
            IFont font)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.BorderLeft = (BorderStyle)borderStyleInfo.LeftBorderStyle;
            cellStyle.LeftBorderColor = borderStyleInfo.LeftBoderColor;
            cellStyle.BorderTop = (BorderStyle)borderStyleInfo.TopBorderStyle;
            cellStyle.TopBorderColor = borderStyleInfo.TopBoderColor;
            cellStyle.BorderRight = (BorderStyle)borderStyleInfo.RightBorderStyle;
            cellStyle.RightBorderColor = borderStyleInfo.RightBoderColor;
            cellStyle.BorderBottom = (BorderStyle)borderStyleInfo.BottomBorderStyle;
            cellStyle.BottomBorderColor = borderStyleInfo.BottomBoderColor;

            cellStyle.FillForegroundColor = backgroundInfo.FillForegroundColor;
            cellStyle.FillPattern = (FillPattern)backgroundInfo.FillPattern;

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
        public static IFont CreateFont(
            HSSFWorkbook workbook,
            FontInfo fontInfo,
            bool isBold)
        {
            IFont font = workbook.CreateFont();
            font.FontName = fontInfo.FontName;
            font.FontHeightInPoints = fontInfo.FontSize;
            font.Color = fontInfo.FontColor;

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
            ref ISheet worksheet,
            ref IRow row,
            ref CellInfo cellInfo,
            ICellStyle cellStyle)
        {
            for (var i = cellInfo.CurrentRow; i <= cellInfo.EndRow; i++)
            {
                row = worksheet.GetRow(i) ?? worksheet.CreateRow(i);
                row.Height = NPOIConstants.RowHeight;

                for (var j = cellInfo.CurrentColumn; j <= cellInfo.EndColumn; j++)
                {
                    ICell cell = row.GetCell(j) ?? row.CreateCell(j);
                    if (i == cellInfo.CurrentRow && j == cellInfo.CurrentColumn)
                    {
                        NPOIUtility.SetCellValue(cell, cellInfo.CellValue);
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
            ref ISheet worksheet,
            ref IRow row,
            ref CellInfo cellInfo,
            ICellStyle cellStyle)
        {
            RenderCell(ref worksheet, ref row, ref cellInfo, cellStyle);
            worksheet.AddMergedRegion(new CellRangeAddress(cellInfo.CurrentRow, cellInfo.EndRow, cellInfo.CurrentColumn, cellInfo.EndColumn));
        }
    }
}