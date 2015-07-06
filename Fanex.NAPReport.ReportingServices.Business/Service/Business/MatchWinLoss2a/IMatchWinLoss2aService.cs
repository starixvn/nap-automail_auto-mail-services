// -----------------------------------------------------------------------
// <copyright file="IMatchWinLoss2aService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchWinLoss2aService
    {
        BettypesAndSportsAndThirdPartyProductsWinLoss GetFullReport(DateTime currentTime);
    }
}