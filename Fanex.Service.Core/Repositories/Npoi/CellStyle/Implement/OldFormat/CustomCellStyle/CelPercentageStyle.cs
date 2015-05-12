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
        private HSSFWorkbook _workbook;
        private IDataFormat _dataFormat;

        public CelPercentageStyle(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat();
        }

        public ICellStyle CreatePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle percentageStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(percentageStyle, backgroundColorIndex);
            percentageStyle.Alignment = HorizontalAlignment.Right;
            percentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalPercentFormat);
            percentageStyle.SetFont(font);
            return percentageStyle;
        }

        public ICellStyle CreatePositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle positivePercentageStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(positivePercentageStyle, backgroundColorIndex);
            positivePercentageStyle.Alignment = HorizontalAlignment.Right;
            positivePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositivePercentFormat);
            positivePercentageStyle.SetFont(font);
            return positivePercentageStyle;
        }

        public ICellStyle CreateNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle negativePercentageStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(negativePercentageStyle, backgroundColorIndex);
            negativePercentageStyle.Alignment = HorizontalAlignment.Right;
            negativePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NegativePercentFormat);
            negativePercentageStyle.SetFont(font);
            return negativePercentageStyle;
        }

        public ICellStyle CreateParenthesesPositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesPositivePercentageStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(parenthesesPositivePercentageStyle, backgroundColorIndex);
            parenthesesPositivePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesPositivePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesPositivePercentFormat);
            parenthesesPositivePercentageStyle.SetFont(font);
            return parenthesesPositivePercentageStyle;
        }

        public ICellStyle CreateParenthesesNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle parenthesesNegativePercentageStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(parenthesesNegativePercentageStyle, backgroundColorIndex);
            parenthesesNegativePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesNegativePercentFormat);
            parenthesesNegativePercentageStyle.SetFont(font);
            return parenthesesNegativePercentageStyle;
        }
    }
}