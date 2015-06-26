// -----------------------------------------------------------------------
// <copyright file="IGameWinLimitExcelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;
    using Fanex.Data.Repository;

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
