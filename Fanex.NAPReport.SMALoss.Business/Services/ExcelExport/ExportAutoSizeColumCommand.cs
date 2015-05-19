
namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using NPOI.XSSF.UserModel;

    public class ExportAutoSizeColumCommand : ExportCommandBase
    {
        public ExportAutoSizeColumCommand(ExcelDocument excelDocument)
            : base(excelDocument)
        {
        }

        public override void Execute()
        {
            var workSheet = ExcelDocument.Workbook.GetSheetAt(0) as XSSFSheet;

            for (int i = 0; i < ExcelDocument.ExportSettings.MaxNumOfColumns; i++)
            {
                int colIndex = i + ExcelDocument.ExportSettings.StartColIndex;
                workSheet.AutoSizeColumn(colIndex);
                workSheet.SetColumnWidth(colIndex, workSheet.GetColumnWidth(colIndex) + ExcelDocument.ExportSettings.ColumnPadding);
            }
        }
    }
}
