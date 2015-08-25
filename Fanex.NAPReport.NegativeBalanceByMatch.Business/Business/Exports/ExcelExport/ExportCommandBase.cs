namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    using Fanex.NAPReport.Business.Business.Exports.ExcelExport;

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