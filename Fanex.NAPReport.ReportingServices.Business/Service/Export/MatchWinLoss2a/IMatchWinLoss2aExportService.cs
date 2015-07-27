// -----------------------------------------------------------------------
// <copyright file="IMatchWinLoss2aExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.MatchWinLoss2a
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchWinLoss2aExportService
    {
        void ExportReport(BettypesAndSportsAndThirdPartyProductsWinLoss fullReport, DateTime winlossDate);

        string FileName { get; set; }
    }
}