// -----------------------------------------------------------------------
// <copyright file="CellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.CustomCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellDecimalNumberStyle : ICellDecimalNumberStyle
    {
        private readonly HSSFWorkbook _workbook;
        private readonly IDataFormat _dataFormat;

        public CellDecimalNumberStyle(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat();
        }

        public ICellStyle CreateDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle decimalNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(decimalNumberStyle, backgroundColorIndex);
            decimalNumberStyle.Alignment = HorizontalAlignment.Right;
            decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NormalDecimalNumberFormat);
            decimalNumberStyle.SetFont(font);
            return decimalNumberStyle;
        }

        public ICellStyle CreatePositiveDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle positiveDecimalNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(positiveDecimalNumberStyle, backgroundColorIndex);
            positiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            positiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.PositiveDecimalNumberFormat);
            positiveDecimalNumberStyle.SetFont(font);
            return positiveDecimalNumberStyle;
        }

        public ICellStyle CreateNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle negativeDecimalNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(negativeDecimalNumberStyle, backgroundColorIndex);
            negativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            negativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NegativeDecimalNumberFormat);
            negativeDecimalNumberStyle.SetFont(font);
            return negativeDecimalNumberStyle;
        }

        public ICellStyle CreateParenthesesPositiveDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesPositiveDecimalNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(parenthesesPositiveDecimalNumberStyle, backgroundColorIndex);
            parenthesesPositiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            parenthesesPositiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesPositiveDecimalNumberFormat);
            parenthesesPositiveDecimalNumberStyle.SetFont(font);
            return parenthesesPositiveDecimalNumberStyle;
        }

        public ICellStyle CreateParenthesesNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesNegativeDecimalNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(parenthesesNegativeDecimalNumberStyle, backgroundColorIndex);
            parenthesesNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativeDecimalNumberFormat);
            parenthesesNegativeDecimalNumberStyle.SetFont(font);
            return parenthesesNegativeDecimalNumberStyle;
        }
    }
}