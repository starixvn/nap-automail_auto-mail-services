// -----------------------------------------------------------------------
// <copyright file="IMatchWinLoss2aRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.MatchWinLoss2a
{
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchWinLoss2aRepository
    {
        List<SummaryBettypeWinLoss> GetSummaryBettypesWinLoss(
            GetSummaryBettypesWinLossCriteria criteria);

        MatchAndSportWinLoss GetMatchAndThirdPartyProductsWinLoss(
            GetSportsWinLossCriteria criteria);

        SummaryBettypeAndMatchAndSportWinLoss GetFullReport(
            GetFullReportCriteria criteria);
    }
}