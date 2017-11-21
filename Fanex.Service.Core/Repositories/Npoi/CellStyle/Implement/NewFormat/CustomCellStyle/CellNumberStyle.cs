// -----------------------------------------------------------------------
// <copyright file="CellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.CustomCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.CustomCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellNumberStyle : ICellNumberStyle
    {
        private readonly XSSFWorkbook _workbook;
        private readonly XSSFDataFormat _dataFormat;

        public CellNumberStyle(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
        }

        public XSSFCellStyle CreateLeftNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle leftNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(leftNumberStyle, backgroundColor);
            leftNumberStyle.Alignment = HorizontalAlignment.Left;
            leftNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            leftNumberStyle.SetFont(font);
            return leftNumberStyle;
        }

        public XSSFCellStyle CreateRightNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle rightNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(rightNumberStyle, backgroundColor);
            rightNumberStyle.Alignment = HorizontalAlignment.Right;
            rightNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            rightNumberStyle.SetFont(font);
            return rightNumberStyle;
        }

        public XSSFCellStyle CreateCenterNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle centerNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(centerNumberStyle, backgroundColor);
            centerNumberStyle.Alignment = HorizontalAlignment.Center;
            centerNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            centerNumberStyle.SetFont(font);
            return centerNumberStyle;
        }

        public XSSFCellStyle CreateParenthesesNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle parenthesesNegativeIntegerNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(parenthesesNegativeIntegerNumberStyle, backgroundColor);
            parenthesesNegativeIntegerNumberStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativeIntegerNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativeIntegerNumberFormat);
            parenthesesNegativeIntegerNumberStyle.SetFont(font);
            return parenthesesNegativeIntegerNumberStyle;
        }
    }
}