// -----------------------------------------------------------------------
// <copyright file="ICompanyBalanceExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyBalance
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyBalanceExportService
    {
        void ExportReport(CompanyBalance balance, DateTime currentTime);
    }
}