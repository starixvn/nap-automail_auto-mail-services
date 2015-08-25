// -----------------------------------------------------------------------
// <copyright file="ISuperYesterdayBalanceExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.SuperYesterdayBalance
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISuperYesterdayBalanceExportService
    {
        string FileName { get; set; }

        void ExportReport(SuperYesterdayBalance balance, DateTime currentTime);        
    }
}