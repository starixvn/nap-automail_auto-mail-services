// -----------------------------------------------------------------------
// <copyright file="CellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public class CellDecimalNumberStyle : ICellDecimalNumberStyle
    {
        private readonly XSSFWorkbook _workbook;
        private readonly XSSFDataFormat _dataFormat;

        public CellDecimalNumberStyle(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
        }

        public XSSFCellStyle CreateDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle decimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(decimalNumberStyle, backgroundColor);
            decimalNumberStyle.Alignment = HorizontalAlignment.Right;
            decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NormalDecimalNumberFormat);
            decimalNumberStyle.SetFont(font);
            return decimalNumberStyle;
        }

        public XSSFCellStyle CreatePositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle positiveDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(positiveDecimalNumberStyle, backgroundColor);
            positiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            positiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.PositiveDecimalNumberFormat);
            positiveDecimalNumberStyle.SetFont(font);
            return positiveDecimalNumberStyle;
        }

        public XSSFCellStyle CreateNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle negativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(negativeDecimalNumberStyle, backgroundColor);
            negativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            negativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NegativeDecimalNumberFormat);
            negativeDecimalNumberStyle.SetFont(font);
            return negativeDecimalNumberStyle;
        }

        public XSSFCellStyle CreateParenthesesPositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle parenthesesPositiveDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(parenthesesPositiveDecimalNumberStyle, backgroundColor);
            parenthesesPositiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            parenthesesPositiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesPositiveDecimalNumberFormat);
            parenthesesPositiveDecimalNumberStyle.SetFont(font);
            return parenthesesPositiveDecimalNumberStyle;
        }

        public XSSFCellStyle CreateParenthesesNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle parenthesesNegativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(parenthesesNegativeDecimalNumberStyle, backgroundColor);
            parenthesesNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativeDecimalNumberFormat);
            parenthesesNegativeDecimalNumberStyle.SetFont(font);
            return parenthesesNegativeDecimalNumberStyle;
        }
    }
}