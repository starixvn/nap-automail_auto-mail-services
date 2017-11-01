namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class ExportHeaderCommand : ExportCommandBase
    {
        private string _title = string.Empty;
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;
        private XSSFCellStyle _headerStyle;

        public ExportHeaderCommand(ExcelDocument excelDocument, string title)
            : base(excelDocument)
        {
            _title = title;
            _fontService = new NpoiNewFormatFontService(excelDocument.Workbook);
            _regularFont = _fontService.RegularFont;            
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(excelDocument.Workbook);
            _headerStyle = _customCellStyleService.CreateCenterTextStyle(Color.FromArgb(60, 94, 165), _fontService.WhiteBoldFont);
        }

        public override void Execute()
        {
            this.RenderHeader();
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void RenderHeader()
        {
            var styleTitle = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, fontColor: Color.Red, horizontalAlignment: HorizontalAlignment.Left);
            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);
            ExcelDocument.RenderCurrentCell(_title, styleTitle);

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.IncreaseRowIndex();

            ExcelDocument.RenderCurrentCell("#", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Username", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Level", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Currency", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Yesterday Balance", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Total Balance", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Max Loss", _headerStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell("Loss Percentage", _headerStyle);

            ExcelDocument.ResetColumnIndex();
        }
    }
}
