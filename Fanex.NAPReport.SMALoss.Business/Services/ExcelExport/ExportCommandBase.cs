
namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    public abstract class ExportCommandBase : IExportCommand
    {
        protected ExportCommandBase(ExcelDocument excelDocument)
        {
            ExcelDocument = excelDocument;
        }

        protected ExcelDocument ExcelDocument { get; set; }

        public abstract void Execute();
    }
}
