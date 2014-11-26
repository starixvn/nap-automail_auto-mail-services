// -----------------------------------------------------------------------
// <copyright file="IHorseRacingReportService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Service
{
    using System;
    using Fanex.HorseRacingReport.Business.Criteria;
    using Fanex.HorseRacingReport.Business.Model;

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