// -----------------------------------------------------------------------
// <copyright file="CellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public class CellDecimalNumberStyle : ICellDecimalNumberStyle
    {
        private XSSFWorkbook _workbook;
        private XSSFCellStyle _decimalNumberStyle;
        private XSSFCellStyle _positiveDecimalNumberStyle;
        private XSSFCellStyle _negativeDecimalNumberStyle;
        private XSSFCellStyle _parenthesesPositiveDecimalNumberStyle;
        private XSSFCellStyle _parenthesesNegativeDecimalNumberStyle;
        private XSSFCellStyle _parenthesesBlackPositiveDecimalNumberStyle;
        private XSSFCellStyle _parenthesesBlackNegativeDecimalNumberStyle;
        private XSSFCellStyle _parenthesesUnsignedNegativeDecimalNumberStyle;
        private XSSFDataFormat _dataFormat;
        private XSSFFont _font;

        public CellDecimalNumberStyle(XSSFWorkbook workbook, XSSFFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
            this.CreateDecimalNumberStyle();
            this.CreatePositiveDecimalNumberStyle();
            this.CreateNegativeDecimalNumberStyle();
            this.CreateParenthesesPositiveDecimalNumberStyle();
            this.CreateParenthesesNegativeDecimalNumberStyle();
            this.CreateParenthesesBlackPositiveDecimalNumberStyle();
            this.CreateParenthesesBlackNegativeDecimalNumberStyle();
            this.CreateParenthesesUnsignedNegativeDecimalNumberStyle();
        }

        public XSSFCellStyle DecimalNumberStyle
        {
            get
            {
                return _decimalNumberStyle;
            }
        }

        public XSSFCellStyle PositiveDecimalNumberStyle
        {
            get
            {
                return _positiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle NegativeDecimalNumberStyle
        {
            get
            {
                return _negativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesPositiveDecimalNumberStyle
        {
            get
            {
                return _parenthesesPositiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesNegativeDecimalNumberStyle
        {
            get
            {
                return _parenthesesNegativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesBlackPositiveDecimalNumberStyle
        {
            get
            {
                return _parenthesesBlackPositiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesBlackNegativeDecimalNumberStyle
        {
            get
            {
                return _parenthesesBlackNegativeDecimalNumberStyle;
            }
        }
        public XSSFCellStyle ParenthesesUnsignedNegativeDecimalNumberStyle
        {
            get
            {
                return _parenthesesUnsignedNegativeDecimalNumberStyle;
            }
        }

        private void CreateDecimalNumberStyle()
        {
            this._decimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._decimalNumberStyle, Color.White);
            this._decimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);
            this._decimalNumberStyle.SetFont(_font);
        }

        private void CreatePositiveDecimalNumberStyle()
        {
            this._positiveDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._positiveDecimalNumberStyle, Color.White);
            this._positiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._positiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositiveDecimalNumberFormat);
            this._positiveDecimalNumberStyle.SetFont(_font);
        }

        private void CreateNegativeDecimalNumberStyle()
        {
            this._negativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._negativeDecimalNumberStyle, Color.White);
            this._negativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._negativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NegativeDecimalNumberFormat);
            this._negativeDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesPositiveDecimalNumberStyle()
        {
            this._parenthesesPositiveDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesPositiveDecimalNumberStyle, Color.White);
            this._parenthesesPositiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesPositiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesPositiveDecimalNumberFormat);
            this._parenthesesPositiveDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesNegativeDecimalNumberStyle()
        {
            this._parenthesesNegativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesNegativeDecimalNumberStyle, Color.White);
            this._parenthesesNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesNegativeDecimalNumberFormat);
            this._parenthesesNegativeDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesBlackPositiveDecimalNumberStyle()
        {
            this._parenthesesBlackPositiveDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesBlackPositiveDecimalNumberStyle, Color.White);
            this._parenthesesBlackPositiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesBlackPositiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesBlackPositiveDecimalNumberFormat);
            this._parenthesesBlackPositiveDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesBlackNegativeDecimalNumberStyle()
        {
            this._parenthesesBlackNegativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesBlackNegativeDecimalNumberStyle, Color.White);
            this._parenthesesBlackNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesBlackNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesBlackNegativeDecimalNumberFormat);
            this._parenthesesBlackNegativeDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesUnsignedNegativeDecimalNumberStyle()
        {
            this._parenthesesUnsignedNegativeDecimalNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(this._parenthesesUnsignedNegativeDecimalNumberStyle, Color.White);
            this._parenthesesUnsignedNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesUnsignedNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesUnsignedNegativeDecimalNumberFormat);
            this._parenthesesUnsignedNegativeDecimalNumberStyle.SetFont(_font);
        }
    }
}