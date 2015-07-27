// -----------------------------------------------------------------------
// <copyright file="IDailyTurnoverExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.DailyTurnover
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDailyTurnoverExportService
    {
        void ExportReport(List<DailyTurnover> dailyTurnover, DateTime winlossDate);

        string FileName { get; set; }
    }
}