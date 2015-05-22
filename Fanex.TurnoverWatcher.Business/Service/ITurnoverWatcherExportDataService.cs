namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;

    public interface ITurnoverWatcherExportDataService
    {
        string ReportPath { get; }

        void ExportReportToExcel(List<TurnoverInfo> report);
    }
}