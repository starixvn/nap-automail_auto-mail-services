// -----------------------------------------------------------------------
// <copyright file="CelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public class CelPercentageStyle : ICelPercentageStyle
    {
        private readonly HSSFWorkbook _workbook;
        private readonly IDataFormat _dataFormat;
        private readonly IFont _font;

        private ICellStyle _percentageStyle;
        private ICellStyle _positivePercentageStyle;
        private ICellStyle _negativePercentageStyle;
        private ICellStyle _parenthesesPositivePercentageStyle;
        private ICellStyle _parenthesesNegativePercentageStyle;        

        public CelPercentageStyle(HSSFWorkbook workbook, IFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat();
            this.CreatePercentageStyle();
            this.CreatePositivePercentageStyle();
            this.CreateNegativePercentageStyle();
            this.CreateParenthesesPositivePercentageStyle();
            this.CreateParenthesesNegativePercentageStyle();
        }

        public ICellStyle PercentageStyle
        {
            get
            {
                return _percentageStyle;
            }
        }

        public ICellStyle PositivePercentageStyle
        {
            get
            {
                return _positivePercentageStyle;
            }
        }

        public ICellStyle NegativePercentageStyle
        {
            get
            {
                return _negativePercentageStyle;
            }
        }

        public ICellStyle ParenthesesPositivePercentageStyle
        {
            get
            {
                return _parenthesesPositivePercentageStyle;
            }
        }

        public ICellStyle ParenthesesNegativePercentageStyle
        {
            get
            {
                return _parenthesesNegativePercentageStyle;
            }
        }

        private void CreatePercentageStyle()
        {
            this._percentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._percentageStyle, HSSFColor.White.Index);
            this._percentageStyle.Alignment = HorizontalAlignment.Right;
            this._percentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NormalPercentFormat);
            this._percentageStyle.SetFont(_font);
        }

        private void CreatePositivePercentageStyle()
        {
            this._positivePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._positivePercentageStyle, HSSFColor.White.Index);
            this._positivePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._positivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.PositivePercentFormat);
            this._positivePercentageStyle.SetFont(_font);
        }

        private void CreateNegativePercentageStyle()
        {
            this._negativePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._negativePercentageStyle, HSSFColor.White.Index);
            this._negativePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._negativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NegativePercentFormat);
            this._negativePercentageStyle.SetFont(_font);
        }

        private void CreateParenthesesPositivePercentageStyle()
        {
            this._parenthesesPositivePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._parenthesesPositivePercentageStyle, HSSFColor.White.Index);
            this._parenthesesPositivePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesPositivePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesPositivePercentFormat);
            this._parenthesesPositivePercentageStyle.SetFont(_font);
        }

        private void CreateParenthesesNegativePercentageStyle()
        {
            this._parenthesesNegativePercentageStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._parenthesesNegativePercentageStyle, HSSFColor.White.Index);
            this._parenthesesNegativePercentageStyle.Alignment = HorizontalAlignment.Right;
            this._parenthesesNegativePercentageStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.ParenthesesNegativePercentFormat);
            this._parenthesesNegativePercentageStyle.SetFont(_font);
        }
    }
}