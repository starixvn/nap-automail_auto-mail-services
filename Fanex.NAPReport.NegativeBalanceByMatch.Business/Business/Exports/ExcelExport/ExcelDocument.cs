namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
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

        public ExcelDocument(ExportSettings exportSettings, string licenseeName)
        {
            Workbook = new XSSFWorkbook();
            ExportSettings = exportSettings;
            _wooksheet = Workbook.CreateSheet(licenseeName) as XSSFSheet;

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
            SaveFile.SaveExcelFile(Path.GetDirectoryName(saveFileNamePath), Path.GetFileName(saveFileNamePath), Workbook, AppSettings.ExcelPasswordProtected);
        }

        public void RenderCurrentCell(object cellValue, XSSFCellStyle cellStyle = null)
        {
            CurrentCell.CellValue = cellValue;
            NpoiNewFormatExcelExtension.RenderCell(_wooksheet, CurrentCell, cellStyle);
        }

        public void RenderMergeCell(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            CellRangeAddress region = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            _wooksheet.AddMergedRegion(region);
        }

        public XSSFCellStyle GetCellStyle(
            FontBoldWeight fontBoldWeight,
            Color backgroundColor = default(Color),
            Color borderColor = default(Color),
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center)
        {
            XSSFCellStyle style = Workbook.CreateCellStyle() as XSSFCellStyle;

            IFont font = Workbook.CreateFont();
            font.Boldweight = (short)fontBoldWeight;
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