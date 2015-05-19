namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Drawing;
    public class ExportHeaderCommand : ExportCommandBase
    {
        private string _title = string.Empty;
        public ExportHeaderCommand(ExcelDocument excelDocument, string title)
            : base(excelDocument)
        {
            _title = title;
        }

        public override void Execute()
        {
            this.RenderHeader();
        }

        private void RenderHeader()
        {
            var styleTitle = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, fontColor: Color.Red, horizontalAlignment: HorizontalAlignment.Left);
            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);
            ExcelDocument.RenderCurrentCell(_title, styleTitle);

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.IncreaseRowIndex();

            var style = ExcelDocument.GetCellStyle(FontBoldWeight.Normal, Color.FromArgb(60, 94, 165), Color.FromArgb(0, 0, 0));

            ExcelDocument.RenderCurrentCell("#", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("SMA Name", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Level", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Currency", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Yesterday Balance", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Total Balance", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Max Loss", style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Loss Percentage", style);

            ExcelDocument.ResetColumnIndex();
        }
    }
}
