// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsExportDataService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsExportDataService
    {
        string DirectoryOfFileName { get; }

        string FileName { get; set; }

        void ExportReportToExcel(WinLossByProducts report, GetWinLossByProductsCriteria criteria);
    }
}