// -----------------------------------------------------------------------
// <copyright file="ISMAReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Service
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAExportExcelService
    {
        List<string> DirectoryOfFileNames { get; set; }

        List<string> FileNames { get; set; }

        void ExportReport();
    }
}