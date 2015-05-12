// -----------------------------------------------------------------------
// <copyright file="CellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.BodyCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellDecimalNumberStyle : ICellDecimalNumberStyle
    {
        private HSSFWorkbook _workbook;
        private ICellStyle _decimalNumberStyle;
        private ICellStyle _positiveDecimalNumberStyle;
        private ICellStyle _negativeDecimalNumberStyle;
        private ICellStyle _parenthesesPositiveDecimalNumberStyle;
        private ICellStyle _parenthesesNegativeDecimalNumberStyle;
        private IDataFormat _dataFormat;
        private IFont _font;

        public CellDecimalNumberStyle(HSSFWorkbook workbook, IFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat();
            CreateDecimalNumberStyle();
            this.CreatePositiveDecimalNumberStyle();
            this.CreateNegativeDecimalNumberStyle();
            this.CreateParenthesesPositiveDecimalNumberStyle();
            this.CreateParenthesesNegativeDecimalNumberStyle();
        }

        public ICellStyle DecimalNumberStyle
        {
            get
            {
                return _decimalNumberStyle;
            }
        }

        public ICellStyle PositiveDecimalNumberStyle
        {
            get
            {
                return _positiveDecimalNumberStyle;
            }
        }

        public ICellStyle NegativeDecimalNumberStyle
        {
            get
            {
                return _negativeDecimalNumberStyle;
            }
        }

        public ICellStyle ParenthesesPositiveDecimalNumberStyle
        {
            get
            {
                return _parenthesesPositiveDecimalNumberStyle;
            }
        }

        public ICellStyle ParenthesesNegativeDecimalNumberStyle
        {
            get
            {
                return _parenthesesNegativeDecimalNumberStyle;
            }
        }

        private void CreateDecimalNumberStyle()
        {
            this._decimalNumberStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._decimalNumberStyle, HSSFColor.White.Index);
            this._decimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._decimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NormalDecimalNumberFormat);
            this._decimalNumberStyle.SetFont(_font);
        }

        private void CreatePositiveDecimalNumberStyle()
        {
            this._positiveDecimalNumberStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._positiveDecimalNumberStyle, HSSFColor.White.Index);
            this._positiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._positiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.PositiveDecimalNumberFormat);
            this._decimalNumberStyle.SetFont(_font);
        }

        private void CreateNegativeDecimalNumberStyle()
        {
            this._negativeDecimalNumberStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._negativeDecimalNumberStyle, HSSFColor.White.Index);
            this._negativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._negativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NegativeDecimalNumberFormat);
            this._negativeDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesPositiveDecimalNumberStyle()
        {
            this._parenthesesPositiveDecimalNumberStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._parenthesesPositiveDecimalNumberStyle, HSSFColor.White.Index);
            this._parenthesesPositiveDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesPositiveDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesPositiveDecimalNumberFormat);
            this._parenthesesPositiveDecimalNumberStyle.SetFont(_font);
        }

        private void CreateParenthesesNegativeDecimalNumberStyle()
        {
            this._parenthesesNegativeDecimalNumberStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._parenthesesNegativeDecimalNumberStyle, HSSFColor.White.Index);
            this._parenthesesNegativeDecimalNumberStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesNegativeDecimalNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.ParenthesesNegativeDecimalNumberFormat);
            this._parenthesesNegativeDecimalNumberStyle.SetFont(_font);
        }
    }
}