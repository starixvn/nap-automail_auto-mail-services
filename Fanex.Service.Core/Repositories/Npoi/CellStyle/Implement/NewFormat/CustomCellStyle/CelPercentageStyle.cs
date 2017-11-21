// -----------------------------------------------------------------------
// <copyright file="CelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public class CelPercentageStyle : ICelPercentageStyle
    {
        private readonly XSSFWorkbook _workbook;
        private readonly XSSFDataFormat _dataFormat;

        public CelPercentageStyle(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
        }

        public XSSFCellStyle CreatePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle percentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(percentageStyle, backgroundColor);
            percentageStyle.Alignment = HorizontalAlignment.Right;
            percentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NormalPercentFormat);
            percentageStyle.SetFont(font);
            return percentageStyle;
        }

        public XSSFCellStyle CreatePositivePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle positivePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(positivePercentageStyle, backgroundColor);
            positivePercentageStyle.Alignment = HorizontalAlignment.Right;
            positivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.PositivePercentFormat);
            positivePercentageStyle.SetFont(font);
            return positivePercentageStyle;
        }

        public XSSFCellStyle CreateNegativePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle negativePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(negativePercentageStyle, backgroundColor);
            negativePercentageStyle.Alignment = HorizontalAlignment.Right;
            negativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NegativePercentFormat);
            negativePercentageStyle.SetFont(font);
            return negativePercentageStyle;
        }

        public XSSFCellStyle CreateParenthesesPositivePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle parenthesesPositivePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(parenthesesPositivePercentageStyle, backgroundColor);
            parenthesesPositivePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesPositivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesPositivePercentFormat);
            parenthesesPositivePercentageStyle.SetFont(font);
            return parenthesesPositivePercentageStyle;
        }

        public XSSFCellStyle CreateParenthesesNegativePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle parenthesesNegativePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(parenthesesNegativePercentageStyle, backgroundColor);
            parenthesesNegativePercentageStyle.Alignment = HorizontalAlignment.Right;
            parenthesesNegativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativePercentFormat);
            parenthesesNegativePercentageStyle.SetFont(font);
            return parenthesesNegativePercentageStyle;
        }
    }
}