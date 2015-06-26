// -----------------------------------------------------------------------
// <copyright file="ISMAReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Service
{
    using Fanex.NAPReport.SMAReport.Business.Model;
    using Fanex.MailDelivery.Client.Models;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAExportExcelService
    {
        void ExportReport();

        List<string> DirectoryOfFileNames { get; set; }

        List<string> FileNames { get; set; }
    }
}