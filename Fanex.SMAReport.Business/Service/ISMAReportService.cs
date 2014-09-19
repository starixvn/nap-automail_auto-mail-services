// -----------------------------------------------------------------------
// <copyright file="IResetWinningService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.SMAReport.Business.Model;
    using Fanex.SMAReport.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAReportService
    {
        void ProcessResetWinning();

        int InsertTask(SMAReportModel task);

        Status GetSatatusByTaskId(int Id);
    }
}
