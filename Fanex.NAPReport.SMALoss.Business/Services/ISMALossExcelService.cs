namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System.Collections.Generic;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.NAPReport.SMALoss.Business.Services.ExcelExport;

    public interface ISMALossExcelService
    {
        void GenerateReport(string title, string sheetName, IEnumerable<FormattedSMALossGroup> lossGroup, ExportSettings exportSettings, string saveFileNamePath);
    }
}
