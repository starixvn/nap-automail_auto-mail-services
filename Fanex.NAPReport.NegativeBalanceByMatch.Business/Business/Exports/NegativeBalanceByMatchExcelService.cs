namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports
{
    using System.Collections.Generic;
    using Fanex.NAPReport.Business.Business.Exports.ExcelExport;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

    public class NegativeBalanceByMatchExcelService : INegativeBalanceByMatchExcelService
    {
        public void GenerateReportForLicensee(string licenseeName, IList<Match> matchs, ExportSettings exportSettings, string saveFileNamePath)
        {
            ExcelDocument excelDocument = new ExcelDocument(exportSettings, licenseeName);

            ExportCompositeCommand<IExportCommand> exportCommand = new ExportCompositeCommand<IExportCommand>(excelDocument);

            ExportMatchsCommand exportMatchsCommand = new ExportMatchsCommand(excelDocument, matchs, 3);
            foreach (var match in matchs)
            {
                ExportMatchInfoCommand matchInfoCommand = new ExportMatchInfoCommand(excelDocument, match.MatchInfo);
                ExportMatchSettleLogsCommand matchSettleLogsCommand = new ExportMatchSettleLogsCommand(excelDocument, match.MatchSettleLogList);
                ExportCustomerBalanceInfosCommand customerBalanceInfoCommand = new ExportCustomerBalanceInfosCommand(excelDocument, match.CustomerInfoList);

                ExportMatchCommand matchCommand = new ExportMatchCommand(excelDocument, matchInfoCommand, matchSettleLogsCommand, customerBalanceInfoCommand);

                exportMatchsCommand.Add(matchCommand);
            }

            IExportCommand autoSizeColumnCommand = new ExportAutoSizeColumnCommand(excelDocument);

            IExportCommand saveCommand = new ExportSaveCommand(excelDocument, saveFileNamePath);

            exportCommand.Add(exportMatchsCommand);
            exportCommand.Add(autoSizeColumnCommand);
            exportCommand.Add(saveCommand);

            exportCommand.Execute();
        }
    }
}