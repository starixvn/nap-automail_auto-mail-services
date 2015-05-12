// -----------------------------------------------------------------------
// <copyright file="NpoiNewFormatCustomCellStyleService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using System.Drawing;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.CustomCellStyle;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.CustomCellStyle;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiNewFormatCustomCellStyleService : INpoiNewFormatCustomCellStyleService
    {
        private ICellDecimalNumberStyle _decimalNumberStyle;
        private ICellTextStyle _textStyle;
        private ICellNumberStyle _numberStyle;
        private ICelPercentageStyle _percentageStyle;
        private XSSFWorkbook _workbook;

        public NpoiNewFormatCustomCellStyleService(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
            this._decimalNumberStyle = new CellDecimalNumberStyle(_workbook);
            this._textStyle = new CellTextStyle(_workbook);
            this._numberStyle = new CellNumberStyle(_workbook);
            this._percentageStyle = new CelPercentageStyle(_workbook);
        }

        public XSSFCellStyle CreateCellStyle(XSSFWorkbook workbook, BorderInformation border, BackgroundInformation background, AlignmentInformation alignment, XSSFFont font)
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
            cellStyle.SetFillForegroundColor(new XSSFColor(background.FillForegroundColor));
            cellStyle.FillPattern = (FillPattern)background.FillForegroundPattern;
            cellStyle.VerticalAlignment = (VerticalAlignment)alignment.VerticalAlignment;
            cellStyle.Alignment = (HorizontalAlignment)alignment.HorizontalAlignment;
            cellStyle.SetFont(font);
            return cellStyle;
        }

        public XSSFCellStyle CreateLeftTextStyle(Color backgroundColor, XSSFFont font)
        {
            return this._textStyle.CreateLeftTextStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateRightTextStyle(Color backgroundColor, XSSFFont font)
        {
            return this._textStyle.CreateRightTextStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateCenterTextStyle(Color backgroundColor, XSSFFont font)
        {
            return this._textStyle.CreateCenterTextStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateLeftNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._numberStyle.CreateLeftNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateRightNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._numberStyle.CreateRightNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateCenterNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._numberStyle.CreateCenterNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._decimalNumberStyle.CreateDecimalNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreatePositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._decimalNumberStyle.CreatePositiveDecimalNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._decimalNumberStyle.CreateNegativeDecimalNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateParenthesesPositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._decimalNumberStyle.CreateParenthesesPositiveDecimalNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateParenthesesNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font)
        {
            return this._decimalNumberStyle.CreateParenthesesNegativeDecimalNumberStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreatePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            return this._percentageStyle.CreatePercentageStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreatePositivePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            return this._percentageStyle.CreatePositivePercentageStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateNegativePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            return this._percentageStyle.CreateNegativePercentageStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateParenthesesPositivePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            return this._percentageStyle.CreateParenthesesPositivePercentageStyle(backgroundColor, font);
        }

        public XSSFCellStyle CreateParenthesesNegativePercentageStyle(Color backgroundColor, XSSFFont font)
        {
            return this._percentageStyle.CreateParenthesesNegativePercentageStyle(backgroundColor, font);
        }
    }
}