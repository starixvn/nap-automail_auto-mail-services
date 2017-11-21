// -----------------------------------------------------------------------
// <copyright file="CelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public class CelPercentageStyle : ICelPercentageStyle
    {
        private readonly HSSFWorkbook _workbook;
        private readonly IDataFormat _dataFormat;

        public CelPercentageStyle(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat();
        }

        public ICellStyle CreatePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle percentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(percentageStyle, backgroundColorIndex);
            percentageStyle.Alignment = HorizontalAlignment.Right;
            percentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NormalPercentFormat);
            percentageStyle.SetFont(font);
            return percentageStyle;
        }

        public ICellStyle CreatePositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle positivePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(positivePercentageStyle, backgroundColorIndex);
            positivePercentageStyle.Alignment = HorizontalAlignment.Right;
            positivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.PositivePercentFormat);
            positivePercentageStyle.SetFont(font);
            return positivePercentageStyle;
        }

        public ICellStyle CreateNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle negativePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(negativePercentageStyle, backgroundColorIndex);
            negativePercentageStyle.Alignment = HorizontalAlignment.Right;
            negativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NegativePercentFormat);
            negativePercentageStyle.SetFont(font);
            return negativePercentageStyle;
        }

        public ICellStyle CreateParenthesesPositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesPositivePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(parenthesesPositivePercentageStyle, backgroundColorIndex);
            parenthesesPositivePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesPositivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesPositivePercentFormat);
            parenthesesPositivePercentageStyle.SetFont(font);
            return parenthesesPositivePercentageStyle;
        }

        public ICellStyle CreateParenthesesNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesNegativePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(parenthesesNegativePercentageStyle, backgroundColorIndex);
            parenthesesNegativePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativePercentFormat);
            parenthesesNegativePercentageStyle.SetFont(font);
            return parenthesesNegativePercentageStyle;
        }
    }
}