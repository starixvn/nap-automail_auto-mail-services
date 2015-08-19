// -----------------------------------------------------------------------
// <copyright file="IGameWinLimitExcelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameWinLimitExcelService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }

        void ExportReportToExcel(List<GameWinLimit> games, CriteriaBase criteria);
    }
}
