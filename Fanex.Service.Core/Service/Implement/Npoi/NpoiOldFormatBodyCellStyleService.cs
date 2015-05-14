// -----------------------------------------------------------------------
// <copyright file="NpoiOldFormatBodyCellStyleService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using Fanex.Service.Core.Model.NpoiBase;
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.BodyCellStyle;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiOldFormatBodyCellStyleService : INpoiOldFormatBodyCellStyleService
    {
        private ICellTextStyle _textStyle;
        private ICellNumberStyle _numberStyle;
        private ICellDecimalNumberStyle _decimalNumberStyle;
        private ICelPercentageStyle _percentageStyle;
        private IFont _font;

        public NpoiOldFormatBodyCellStyleService(HSSFWorkbook workbook, IFont font)
        {
            this._font = font;
            this._textStyle = new CellTextStyle(workbook, _font);
            this._numberStyle = new CellNumberStyle(workbook, _font);
            this._decimalNumberStyle = new CellDecimalNumberStyle(workbook, _font);
            this._percentageStyle = new CelPercentageStyle(workbook, _font);
        }

        public ICellStyle LeftTextStyle
        {
            get
            {
                return this._textStyle.LeftTextStyle;
            }
        }

        public ICellStyle RightTextStyle
        {
            get
            {
                return this._textStyle.RightTextStyle;
            }
        }

        public ICellStyle CenterTextStyle
        {
            get
            {
                return this._textStyle.CenterTextStyle;
            }
        }

        public ICellStyle LeftNumberStyle
        {
            get
            {
                return this._numberStyle.LeftNumberStyle;
            }
        }

        public ICellStyle RightNumberStyle
        {
            get
            {
                return this._numberStyle.RightNumberStyle;
            }
        }

        public ICellStyle CenterNumberStyle
        {
            get
            {
                return this._numberStyle.CenterNumberStyle;
            }
        }

        public ICellStyle DecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.DecimalNumberStyle;
            }
        }

        public ICellStyle PositiveDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.PositiveDecimalNumberStyle;
            }
        }

        public ICellStyle NegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.NegativeDecimalNumberStyle;
            }
        }

        public ICellStyle ParenthesesPositiveDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesPositiveDecimalNumberStyle;
            }
        }

        public ICellStyle ParenthesesNegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesNegativeDecimalNumberStyle;
            }
        }

        public ICellStyle PercentageStyle
        {
            get
            {
                return this._percentageStyle.PercentageStyle;
            }
        }

        public ICellStyle PositivePercentageStyle
        {
            get
            {
                return this._percentageStyle.PositivePercentageStyle;
            }
        }

        public ICellStyle NegativePercentageStyle
        {
            get
            {
                return this._percentageStyle.NegativePercentageStyle;
            }
        }

        public ICellStyle ParenthesesPositivePercentageStyle
        {
            get
            {
                return this._percentageStyle.ParenthesesPositivePercentageStyle;
            }
        }

        public ICellStyle ParenthesesNegativePercentageStyle
        {
            get
            {
                return this._percentageStyle.ParenthesesNegativePercentageStyle;
            }
        }

        public ICellStyle CreateCellStyle(HSSFWorkbook workbook, BorderInformation border, AlignmentInformation alignment, IFont font)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.BorderLeft = (BorderStyle)border.LeftBorderStyle;
            cellStyle.BorderTop = (BorderStyle)border.TopBorderStyle;
            cellStyle.BorderRight = (BorderStyle)border.RightBorderStyle;
            cellStyle.BorderBottom = (BorderStyle)border.BottomBorderStyle;
            cellStyle.BottomBorderColor = border.BottomBoderColor;
            cellStyle.LeftBorderColor = border.LeftBoderColor;
            cellStyle.RightBorderColor = border.RightBoderColor;
            cellStyle.TopBorderColor = border.TopBoderColor;
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = (VerticalAlignment)alignment.VerticalAlignment;
            cellStyle.Alignment = (HorizontalAlignment)alignment.HorizontalAlignment;
            cellStyle.SetFont(font);
            return cellStyle;
        }
    }
}