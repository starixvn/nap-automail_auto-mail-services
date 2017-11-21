// -----------------------------------------------------------------------
// <copyright file="NpoiNewFormatBodyCellStyleService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using System.Drawing;
    using Fanex.Service.Core.Model.NpoiBase;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.BodyCellStyle;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiNewFormatBodyCellStyleService : INpoiNewFormatBodyCellStyleService
    {
        private readonly ICellTextStyle _textStyle;
        private readonly ICellNumberStyle _numberStyle;
        private readonly ICellDecimalNumberStyle _decimalNumberStyle;
        private readonly ICelPercentageStyle _percentageStyle;
        private readonly XSSFFont _font;

        public NpoiNewFormatBodyCellStyleService(XSSFWorkbook workbook, XSSFFont font)
        {
            this._font = font;
            this._textStyle = new CellTextStyle(workbook, _font);
            this._numberStyle = new CellNumberStyle(workbook, _font);
            this._decimalNumberStyle = new CellDecimalNumberStyle(workbook, _font);
            this._percentageStyle = new CelPercentageStyle(workbook, _font);
        }

        public XSSFCellStyle LeftTextStyle
        {
            get
            {
                return this._textStyle.LeftTextStyle;
            }
        }

        public XSSFCellStyle RightTextStyle
        {
            get
            {
                return this._textStyle.RightTextStyle;
            }
        }

        public XSSFCellStyle CenterTextStyle
        {
            get
            {
                return this._textStyle.CenterTextStyle;
            }
        }

        public XSSFCellStyle LeftNumberStyle
        {
            get
            {
                return this._numberStyle.LeftNumberStyle;
            }
        }

        public XSSFCellStyle RightNumberStyle
        {
            get
            {
                return this._numberStyle.RightNumberStyle;
            }
        }

        public XSSFCellStyle CenterNumberStyle
        {
            get
            {
                return this._numberStyle.CenterNumberStyle;
            }
        }

        public XSSFCellStyle DecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.DecimalNumberStyle;
            }
        }

        public XSSFCellStyle PositiveDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.PositiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle NegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.NegativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesPositiveDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesPositiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesNegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesNegativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesBlackPositiveDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesBlackPositiveDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesBlackNegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesBlackNegativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle ParenthesesUnsignedNegativeDecimalNumberStyle
        {
            get
            {
                return this._decimalNumberStyle.ParenthesesUnsignedNegativeDecimalNumberStyle;
            }
        }

        public XSSFCellStyle PercentageStyle
        {
            get
            {
                return this._percentageStyle.PercentageStyle;
            }
        }

        public XSSFCellStyle PositivePercentageStyle
        {
            get
            {
                return this._percentageStyle.PositivePercentageStyle;
            }
        }

        public XSSFCellStyle NegativePercentageStyle
        {
            get
            {
                return this._percentageStyle.NegativePercentageStyle;
            }
        }

        public XSSFCellStyle ParenthesesPositivePercentageStyle
        {
            get
            {
                return this._percentageStyle.ParenthesesPositivePercentageStyle;
            }
        }

        public XSSFCellStyle ParenthesesNegativePercentageStyle
        {
            get
            {
                return _percentageStyle.ParenthesesNegativePercentageStyle;
            }
        }

        public XSSFCellStyle CreateCellStyle(XSSFWorkbook workbook, BorderInformation border, AlignmentInformation alignment, XSSFFont font)
        {
            XSSFCellStyle cellStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            cellStyle.BorderLeft = (BorderStyle)border.LeftBorderStyle;
            cellStyle.BorderTop = (BorderStyle)border.TopBorderStyle;
            cellStyle.BorderRight = (BorderStyle)border.RightBorderStyle;
            cellStyle.BorderBottom = (BorderStyle)border.BottomBorderStyle;
            cellStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(border.BottomBoderColor));
            cellStyle.SetBorderColor(BorderSide.LEFT, new XSSFColor(border.LeftBoderColor));
            cellStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(border.RightBoderColor));
            cellStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(border.TopBoderColor));
            cellStyle.SetFillForegroundColor(new XSSFColor(Color.White));
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = (VerticalAlignment)alignment.VerticalAlignment;
            cellStyle.Alignment = (HorizontalAlignment)alignment.HorizontalAlignment;
            cellStyle.SetFont(font);
            return cellStyle;
        }
    }
}