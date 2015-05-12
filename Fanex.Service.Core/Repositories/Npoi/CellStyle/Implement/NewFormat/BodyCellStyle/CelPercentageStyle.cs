// -----------------------------------------------------------------------
// <copyright file="CelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.BodyCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CelPercentageStyle : ICelPercentageStyle
    {
        private XSSFWorkbook _workbook;
        private XSSFCellStyle _percentageStyle;
        private XSSFCellStyle _positivePercentageStyle;
        private XSSFCellStyle _negativePercentageStyle;
        private XSSFCellStyle _parenthesesPositivePercentageStyle;
        private XSSFCellStyle _parenthesesNegativePercentageStyle;
        private XSSFDataFormat _dataFormat;
        private XSSFFont _font;

        public CelPercentageStyle(XSSFWorkbook workbook, XSSFFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
            this.CreatePercentageStyle();
            this.CreatePositivePercentageStyle();
            this.CreateNegativePercentageStyle();
            this.CreateParenthesesPositivePercentageStyle();
            this.CreateParenthesesNegativePercentageStyle();
        }

        public XSSFCellStyle PercentageStyle
        {
            get
            {
                return _percentageStyle;
            }
        }

        public XSSFCellStyle PositivePercentageStyle
        {
            get
            {
                return _positivePercentageStyle;
            }
        }

        public XSSFCellStyle NegativePercentageStyle
        {
            get
            {
                return _negativePercentageStyle;
            }
        }

        public XSSFCellStyle ParenthesesPositivePercentageStyle
        {
            get
            {
                return _parenthesesPositivePercentageStyle;
            }
        }

        public XSSFCellStyle ParenthesesNegativePercentageStyle
        {
            get
            {
                return _parenthesesNegativePercentageStyle;
            }
        }

        private void CreatePercentageStyle()
        {
            this._percentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._percentageStyle, Color.White);
            this._percentageStyle.Alignment = HorizontalAlignment.Right;
            this._percentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalPercentFormat);
            this._percentageStyle.SetFont(_font);
        }

        private void CreatePositivePercentageStyle()
        {
            this._positivePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._positivePercentageStyle, Color.White);
            this._positivePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._positivePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositivePercentFormat);
            this._positivePercentageStyle.SetFont(_font);
        }

        private void CreateNegativePercentageStyle()
        {
            this._negativePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._negativePercentageStyle, Color.White);
            this._negativePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._negativePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NegativePercentFormat);
            this._negativePercentageStyle.SetFont(_font);
        }

        private void CreateParenthesesPositivePercentageStyle()
        {
            this._parenthesesPositivePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesPositivePercentageStyle, Color.White);
            this._parenthesesPositivePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesPositivePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesPositivePercentFormat);
            this._parenthesesPositivePercentageStyle.SetFont(_font);
        }

        private void CreateParenthesesNegativePercentageStyle()
        {
            this._parenthesesNegativePercentageStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesNegativePercentageStyle, Color.White);
            this._parenthesesNegativePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesNegativePercentageStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesNegativePercentFormat);
            this._parenthesesNegativePercentageStyle.SetFont(_font);
        }
    }
}