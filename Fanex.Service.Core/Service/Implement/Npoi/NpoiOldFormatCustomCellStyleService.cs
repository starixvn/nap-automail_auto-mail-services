// -----------------------------------------------------------------------
// <copyright file="NpoiOldFormatCustomCellStyleService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.CustomCellStyle;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiOldFormatCustomCellStyleService : INpoiOldFormatCustomCellStyleService
    {
        private ICellTextStyle _textStyle;
        private ICellNumberStyle _numberStyle;
        private ICellDecimalNumberStyle _decimalNumberStyle;
        private ICelPercentageStyle _percentageStyle;
        private HSSFWorkbook _workbook;

        public NpoiOldFormatCustomCellStyleService(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
            this._textStyle = new CellTextStyle(_workbook);
            this._numberStyle = new CellNumberStyle(_workbook);
            this._decimalNumberStyle = new CellDecimalNumberStyle(_workbook);
            this._percentageStyle = new CelPercentageStyle(_workbook);
        }

        public ICellStyle CreateCellStyle(HSSFWorkbook workbook, BorderInformation border, BackgroundInformation background, AlignmentInformation alignment, IFont font)
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
            cellStyle.FillForegroundColor = background.FillForegroundColor;
            cellStyle.FillPattern = (FillPattern)background.FillForegroundPattern;
            cellStyle.VerticalAlignment = (VerticalAlignment)alignment.VerticalAlignment;
            cellStyle.Alignment = (HorizontalAlignment)alignment.HorizontalAlignment;
            cellStyle.SetFont(font);
            return cellStyle;
        }

        public ICellStyle CreateLeftTextStyle(short backgroundColorIndex, IFont font)
        {
            return _textStyle.CreateLeftTextStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateRightTextStyle(short backgroundColorIndex, IFont font)
        {
            return _textStyle.CreateRightTextStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateCenterTextStyle(short backgroundColorIndex, IFont font)
        {
            return _textStyle.CreateCenterTextStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateLeftNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _numberStyle.CreateLeftNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateRightNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _numberStyle.CreateRightNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateCenterNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _numberStyle.CreateCenterNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _decimalNumberStyle.CreateDecimalNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreatePositiveDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _decimalNumberStyle.CreatePositiveDecimalNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _decimalNumberStyle.CreateNegativeDecimalNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateParenthesesPositiveDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _decimalNumberStyle.CreateParenthesesPositiveDecimalNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateParenthesesNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font)
        {
            return _decimalNumberStyle.CreateParenthesesNegativeDecimalNumberStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreatePercentageStyle(short backgroundColorIndex, IFont font)
        {
            return _percentageStyle.CreatePercentageStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreatePositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            return _percentageStyle.CreatePositivePercentageStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            return _percentageStyle.CreateNegativePercentageStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateParenthesesPositivePercentageStyle(short backgroundColorIndex, IFont font)
        {
            return _percentageStyle.CreateParenthesesPositivePercentageStyle(backgroundColorIndex, font);
        }

        public ICellStyle CreateParenthesesNegativePercentageStyle(short backgroundColorIndex, IFont font)
        {
            return _percentageStyle.CreateParenthesesNegativePercentageStyle(backgroundColorIndex, font);
        }
    }
}