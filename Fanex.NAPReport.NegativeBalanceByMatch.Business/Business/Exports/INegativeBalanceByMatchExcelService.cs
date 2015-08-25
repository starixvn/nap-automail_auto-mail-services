namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports
{
    using System.Collections.Generic;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

    public interface INegativeBalanceByMatchExcelService
    {
        void GenerateReportForLicensee(string licenseeName, IList<Match> matchs, ExportSettings exportSettings, string saveFileNamePath);
    }
}