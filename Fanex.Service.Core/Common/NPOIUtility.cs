// -----------------------------------------------------------------------
// <copyright file="NPOIUtility.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    using System;
    using System.Drawing;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NPOIUtility
    {
        public static void IncreaseRowIndex(ref CellInfo cellInfo)
        {
            cellInfo.CurrentRow = cellInfo.EndRow + 1;
            cellInfo.EndRow = cellInfo.EndRow + 1;
        }

        public static void IncreaseColumnIndex(ref CellInfo cellInfo)
        {
            cellInfo.CurrentColumn = cellInfo.EndColumn + 1;
            cellInfo.EndColumn = cellInfo.EndColumn + 1;
        }

        public static void ResetColumnIndex(ref CellInfo cellInfo)
        {
            cellInfo.CurrentColumn = 0;
            cellInfo.EndColumn = 0;
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The cell of row.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue is string)
            {
                cell.SetCellValue((string)cellValue);
            }
            else if (cellValue is int)
            {
                cell.SetCellValue((int)cellValue);
            }
            else if (cellValue is long)
            {
                cell.SetCellValue((long)cellValue);
            }
            else if (cellValue is double)
            {
                cell.SetCellValue((double)cellValue);
            }
            else if (cellValue is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue is bool)
            {
                cell.SetCellValue((bool)cellValue);
            }
            else if (cellValue is System.DateTime)
            {
                cell.SetCellValue((System.DateTime)cellValue);
            }
        }

        public static ICellStyle AssignOldFormatCellStyleData(ICellStyle cellStyle, short backgroundColorIndex)
        {
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.LeftBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.TopBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.RightBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.BottomBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.FillForegroundColor = backgroundColorIndex;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            return cellStyle;
        }

        public static XSSFCellStyle AssignNewFormatCellStyleData(XSSFCellStyle cellStyle, Color backgroundColor)
        {
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Gray));
            cellStyle.SetBorderColor(BorderSide.LEFT, new XSSFColor(Color.Gray));
            cellStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Gray));
            cellStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Gray));
            cellStyle.SetFillForegroundColor(new XSSFColor(backgroundColor));
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            return cellStyle;
        }
    }
}