namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System.Drawing;
    using System.IO;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;
    using Fanex.Service.Core.Utilities;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;

    public class ExcelDocument
    {
        private XSSFSheet _wooksheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelDocument" /> class.
        /// </summary>
        /// <param name="wookbook">Wookbook value.</param>
        /// <param name="exportSettings">Export Settings.</param>
        /// <param name="sheetName">Sheet name.</param>
        public ExcelDocument(XSSFWorkbook wookbook, ExportSettings exportSettings, string sheetName)
        {
            Workbook = new XSSFWorkbook();
            ExportSettings = exportSettings;
            _wooksheet = Workbook.CreateSheet(sheetName) as XSSFSheet;
            _wooksheet.CreateFreezePane(0, 4);
            CurrentCell = new Cell();

            InitSettings();
        }

        public XSSFWorkbook Workbook { get; private set; }

        public ExportSettings ExportSettings { get; private set; }

        public int CurrentRow
        {
            get { return CurrentCell.CurrentRow; }
        }

        public int CurrentColumn
        {
            get { return CurrentCell.CurrentColumn; }
        }

        private Cell CurrentCell { get; set; }

        public void Save(string saveFileNamePath)
        {
            SaveFile.SaveExcelFile(
                AppSettings.DataDirectory, 
                Path.GetDirectoryName(saveFileNamePath), 
                Path.GetFileName(saveFileNamePath), 
                Workbook, 
                AppSettings.ExcelPasswordProtected);
        }

        public void RenderCurrentCell(object cellValue, XSSFCellStyle cellStyle = null)
        {
            CurrentCell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(_wooksheet, CurrentCell, cellStyle);
        }

        public void RenderMergeCell(int firstRow, int lastRow, int firstCol, int lastCol, XSSFCellStyle cellStyle = null)
        {
            if (cellStyle != null)
            {
                for (var rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
                {
                    var currentRow = _wooksheet.GetRow(rowIndex) as XSSFRow ?? _wooksheet.CreateRow(rowIndex) as XSSFRow;

                    for (var cellIndex = firstCol; cellIndex <= lastCol; cellIndex++)
                    {
                        var currentCell = currentRow.GetCell(cellIndex) as XSSFCell ?? currentRow.CreateCell(cellIndex) as XSSFCell;

                        if (rowIndex == firstRow || rowIndex == lastRow || cellIndex == firstCol || cellIndex == lastCol)
                        {
                            currentCell.CellStyle = cellStyle;
                        }
                    }
                }
            }

            CellRangeAddress region = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            _wooksheet.AddMergedRegion(region);
        }

        public XSSFCellStyle GetCellStyle(
            FontBoldWeight fontBoldWeight,
            Color backgroundColor = default(Color),
            Color borderColor = default(Color),
            Color fontColor = default(Color),
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center)
        {
            XSSFCellStyle style = Workbook.CreateCellStyle() as XSSFCellStyle;

            var font = Workbook.CreateFont() as XSSFFont;
            font.Boldweight = (short)fontBoldWeight;

            if (fontColor != default(Color))
            {
                font.SetColor(new XSSFColor(fontColor));
            }

            style.SetFont(font);

            if (backgroundColor != default(Color))
            {
                style.SetFillForegroundColor(new XSSFColor(backgroundColor));
                style.FillPattern = FillPattern.SolidForeground;
            }

            if (borderColor != default(Color))
            {
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.SetLeftBorderColor(new XSSFColor(borderColor));
                style.SetRightBorderColor(new XSSFColor(borderColor));
                style.SetTopBorderColor(new XSSFColor(borderColor));
                style.SetBottomBorderColor(new XSSFColor(borderColor));
            }

            style.Alignment = horizontalAlignment;

            return style;
        }

        public void ResetColumnIndex()
        {
            CurrentCell.ResetColumnIndex();
            for (int i = 0; i < ExportSettings.StartColIndex; i++)
            {
                CurrentCell.IncreaseColumnIndex();
            }
        }

        public void IncreaseRowIndex()
        {
            CurrentCell.IncreaseRowIndex();
        }

        public void IncreaseColumnIndex()
        {
            CurrentCell.IncreaseColumnIndex();
        }

        private void InitSettings()
        {
            for (int i = 0; i < ExportSettings.StartRowIndex; i++)
            {
                CurrentCell.IncreaseRowIndex();
            }

            for (int i = 0; i < ExportSettings.StartColIndex; i++)
            {
                CurrentCell.IncreaseColumnIndex();
            }
        }
    }
}
