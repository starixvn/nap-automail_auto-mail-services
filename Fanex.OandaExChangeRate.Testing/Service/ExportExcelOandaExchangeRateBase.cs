// -----------------------------------------------------------------------
// <copyright file="ExportExcelOandaExchangeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.OandaExChangeRate.Testing.Service
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Drawing;
    using Fanex.Data;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExportExcelOandaExchangeRateBase
    {
        protected XSSFFont _boldFont;
        protected BorderInfo _borderInfo;
        protected CellInfo _cellInfo;
        protected AlignmentInfo _centerAlignmentInfo;
        protected XSSFCellStyle _decimalNumberStyle;
        protected XSSFFont _font;
        protected FontAndColorInfo _fontInfo;
        protected BackgroundStyleInfo _headerBackgroundInfo;
        protected Color _headerColor;
        protected XSSFCellStyle _headerTextStyle;
        protected AlignmentInfo _leftAlignmentInfo;
        protected XSSFCellStyle _leftTextStyle;
        protected BackgroundStyleInfo _normalRowBackgroundInfo;
        protected AlignmentInfo _rightAlignmentInfo;
        protected XSSFWorkbook _workbook;
        protected XSSFSheet worksheet;

        public ExportExcelOandaExchangeRateBase()
        {
            this.InitColor();
            this.InitFontInfo();
            this.InitAlignmentInfo();
            this.IntBorderInfo();
            this.InitBackgroundColorInfo();
            
            _workbook = new XSSFWorkbook();
            this.InitFont();
            _decimalNumberStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _rightAlignmentInfo,
                _font);

            _leftTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _normalRowBackgroundInfo,
                _leftAlignmentInfo,
                _font);

            _headerTextStyle = NPOIHighVersion.CreateCellStyle(
                _workbook,
                _borderInfo,
                _headerBackgroundInfo,
                _centerAlignmentInfo,
                _boldFont);

            string sheetName = "Sheet1";
            worksheet = _workbook.GetSheet(sheetName) as XSSFSheet ?? _workbook.CreateSheet(sheetName) as XSSFSheet;
            this.InitColumnWidth(worksheet);

            _cellInfo = new CellInfo(0, 0, 0, 0, string.Empty);            
            worksheet.CreateFreezePane(1,1,1,1);
        }

        

        private void InitAlignmentInfo()
        {
            _leftAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Left,
                (int)VerticalAlignment.Center);
            _centerAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Center,
                (int)VerticalAlignment.Center);
            _rightAlignmentInfo = new AlignmentInfo(
                (int)HorizontalAlignment.Right,
                (int)VerticalAlignment.Center);
        }

        private void InitBackgroundColorInfo()
        {
            _normalRowBackgroundInfo = new BackgroundStyleInfo(
                Color.White,
                (int)FillPattern.SolidForeground);

            _headerBackgroundInfo = new BackgroundStyleInfo(
                _headerColor,
                (int)FillPattern.SolidForeground);
        }

        private void InitColor()
        {
            _headerColor = Color.FromArgb(141, 180, 226);
        }

        private void InitColumnWidth(XSSFSheet worksheet)
        {
            for (int i = 0; i < 14; i++)
            {
                worksheet.SetColumnWidth(i, 130 * NPOIConstants.FontWidth);
            }
        }

        private void InitFont()
        {
            _font = NPOIHighVersion.CreateFont(_workbook, _fontInfo, false);
            _boldFont = NPOIHighVersion.CreateFont(_workbook, _fontInfo, true);
        }

        private void InitFontInfo()
        {
            _fontInfo = new FontAndColorInfo(
                    NPOIConstants.FontName,
                    NPOIConstants.FontSize,
                    Color.Black);
        }

        private void IntBorderInfo()
        {
            _borderInfo = new BorderInfo(
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                (short)BorderStyle.Thin,
                Color.Gray,
                Color.Gray,
                Color.Gray,
                Color.Gray);
        }
    }
}
