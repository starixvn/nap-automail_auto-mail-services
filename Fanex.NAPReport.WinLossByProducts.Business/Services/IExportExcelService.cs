// -----------------------------------------------------------------------
// <copyright file="IExportExcelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IExportExcelService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }

        void ExportExcel(OverallWinLossProductsCriteria criteria);
    }
}
