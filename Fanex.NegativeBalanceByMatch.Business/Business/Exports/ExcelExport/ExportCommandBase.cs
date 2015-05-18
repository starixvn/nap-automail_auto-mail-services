using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
	public abstract class ExportCommandBase: IExportCommand
	{
		protected ExportCommandBase(ExcelDocument excelDocument)
		{
			ExcelDocument = excelDocument;
		}

        protected ExcelDocument ExcelDocument { get; set; }

		public abstract void Execute();
	}
}
