using System.Collections.Generic;
using Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.RngCasino.Business.Configuration;
using Fanex.NAPReport.RngCasino.Business.Entities;

namespace Fanex.NAPReport.RngCasino.Business.Business.Exports
{
	public class RngCasinoExcelService : IRngCasinoExcelService
	{
		public void GenerateReport(IList<UserInfo> userInfos, ExportSettings exportSettings, string saveFileNamePath)
		{
			var excelDocument = new ExcelDocument(exportSettings, RngCasinoSettings.ExcelSheetName);

			var exportCommand = new ExportCompositeCommand<IExportCommand>(excelDocument);

			var exportWinLossCommand = new ExportWinLossCommand(excelDocument, userInfos);
			var autoSizeColumnCommand = new ExportAutoSizeColumnCommand(excelDocument);
			var saveCommand = new ExportSaveCommand(excelDocument, saveFileNamePath);

			exportCommand.Add(exportWinLossCommand);
			exportCommand.Add(autoSizeColumnCommand);
			exportCommand.Add(saveCommand);

			exportCommand.Execute();
		}
	}
}