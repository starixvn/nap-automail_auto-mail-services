namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;

    public interface IExportExcelService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }

        void ExportExcel(List<TurnoverInfo> report);
    }
}