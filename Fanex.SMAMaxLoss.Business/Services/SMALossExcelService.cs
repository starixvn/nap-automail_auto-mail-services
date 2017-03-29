namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System.Collections.Generic;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.NAPReport.SMALoss.Business.Services.ExcelExport;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using NPOI.XSSF.UserModel;

    public class SMALossExcelService : ISMALossExcelService
    {
        /// <summary>
        /// Generate Export Excel.
        /// </summary>
        /// <param name="title">Title in the Excel file.</param>
        /// <param name="sheetName">Sheet name.</param>
        /// <param name="lossGroup">Loss group.</param>
        /// <param name="exportSettings">Export settings.</param>
        /// <param name="saveFileNamePath">Path file.</param>
        public void GenerateReport(string title, string sheetName, IEnumerable<FormattedSMALossGroup> lossGroup, ExportSettings exportSettings, string saveFileNamePath)
        {
            var workbook = new XSSFWorkbook();

            var fontService = new NpoiNewFormatFontService(workbook);
            var cellStyleService = new NpoiNewFormatCustomCellStyleService(workbook);

            var excelDocument = new ExcelDocument(exportSettings, sheetName);

            var exportCommand = new ExportCompositeCommand<IExportCommand>(excelDocument);

            var exportHeaderCommand = new ExportHeaderCommand(excelDocument, title);
            var exportContentCommand = new ExportContentCommand(excelDocument, lossGroup);
            var autoSizeColumnCommand = new ExportAutoSizeColumCommand(excelDocument);
            var saveCommand = new ExportSaveCommand(excelDocument, saveFileNamePath);

            exportCommand.Add(exportHeaderCommand);
            exportCommand.Add(exportContentCommand);
            exportCommand.Add(autoSizeColumnCommand);
            exportCommand.Add(saveCommand);

            exportCommand.Execute();
        }
    }
}
