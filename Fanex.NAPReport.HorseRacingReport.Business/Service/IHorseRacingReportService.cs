// -----------------------------------------------------------------------
// <copyright file="IHorseRacingReportService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using System;
    using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
    using Fanex.NAPReport.HorseRacingReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHorseRacingReportService
    {
        string GetReportDirectory { get; }

        RacingWinLoss GetMainReport(GetMainRacingReportCriteria criteria);

        RacingWinLoss GetDetailReport(GetDetailRacingReportCriteria criteria);

        void ExportMainReport(RacingWinLoss report, int reportType, DateTime winlostDate, TimeRangeCriteria criteria);

        void ExportDetailReport(RacingWinLoss report, int partnerId, DateTime winlostDate, TimeRangeCriteria criteria);
    }
}