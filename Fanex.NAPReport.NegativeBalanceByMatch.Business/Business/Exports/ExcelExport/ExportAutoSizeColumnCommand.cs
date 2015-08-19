namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    using NPOI.XSSF.UserModel;

    public class ExportAutoSizeColumnCommand : ExportCommandBase
    {
        public ExportAutoSizeColumnCommand(ExcelDocument excelDocument)
            : base(excelDocument)
        {
        }

        public override void Execute()
        {
            XSSFSheet workSheet = ExcelDocument.Workbook.GetSheetAt(0) as XSSFSheet;

            for (int i = 0; i < ExcelDocument.ExportSettings.MaxNumOfColumns; i++)
            {
                int colIndex = i + ExcelDocument.ExportSettings.StartColIndex;
                workSheet.AutoSizeColumn(colIndex);
                workSheet.SetColumnWidth(colIndex, workSheet.GetColumnWidth(colIndex) + ExcelDocument.ExportSettings.ColumnPadding);
            }
        }
    }
}
