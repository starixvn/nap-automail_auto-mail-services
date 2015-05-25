// <copyright file="ExportExcelBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Service interface.
    /// </summary>
    public class ExportExcelBase
    {
        public const short EvenRowColorIndex = 50;
        public const short SubTotalColorIndex = 51;
        public const short GrandTotalColorIndex = 52;
        public const short TitleColorIndex = 53;
        public const short NegativeRowColorIndex = 54;
        public const short HeaderColorIndex = 55;
        public const short DisableRowColorIndex = 56;
        public const short BorderColorIndex = 57;

        public ICellStyle CreateStyle(
              ref HSSFWorkbook workbook,
              BorderStyle leftBorderStyle,
               BorderStyle topBorderStyle,
               BorderStyle rightBorderStyle,
               BorderStyle bottomBorderStyle,
              short borderColor)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderLeft = leftBorderStyle;
            style.BorderTop = topBorderStyle;
            style.BorderRight = rightBorderStyle;
            style.BorderBottom = bottomBorderStyle;
            style.LeftBorderColor = style.TopBorderColor = style.RightBorderColor = style.BottomBorderColor = borderColor;

            return style;
        }

        public IFont CreateFont(
            ref HSSFWorkbook workbook,
            string fontName,
            short fontHeight,
            short color,
            short fontWeight)
        {
            IFont font = workbook.CreateFont();
            font.FontName = fontName;
            font.FontHeightInPoints = fontHeight;
            font.Color = color;
            font.Boldweight = (short)fontWeight;

            return font;
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="cell">The cell of row.</param>
        /// <param name="style">The cell style.</param>
        /// <param name="cellValue">The cell value.</param>
        public void RenderCell(ref ICell cell, ICellStyle style, object cellValue)
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

            style.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            cell.CellStyle = style;
        }

        /// <summary>
        /// Sets the index of the color at.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        public void SetColorAtIndex(HSSFWorkbook workbook)
        {
            HSSFPalette palette = workbook.GetCustomPalette();
            palette.SetColorAtIndex(EvenRowColorIndex, (byte)228, (byte)242, (byte)253);
            palette.SetColorAtIndex(SubTotalColorIndex, (byte)244, (byte)244, (byte)190);
            palette.SetColorAtIndex(GrandTotalColorIndex, (byte)220, (byte)226, (byte)188);
            palette.SetColorAtIndex(TitleColorIndex, (byte)255, (byte)204, (byte)51);
            palette.SetColorAtIndex(NegativeRowColorIndex, (byte)224, (byte)177, (byte)191);
            palette.SetColorAtIndex(HeaderColorIndex, (byte)60, (byte)94, (byte)165);
            palette.SetColorAtIndex(DisableRowColorIndex, (byte)191, (byte)191, (byte)191);
            palette.SetColorAtIndex(BorderColorIndex, (byte)0, (byte)0, (byte)0);
        }
    }
}