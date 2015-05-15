namespace Fanex.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using Fanex.TurnoverWatcher.Business.Model;

    public interface ITurnoverWatcherExportDataService
    {
        string ReportPath { get; }

        void ExportReportToExcel(List<TurnoverInfo> report);
    }
}