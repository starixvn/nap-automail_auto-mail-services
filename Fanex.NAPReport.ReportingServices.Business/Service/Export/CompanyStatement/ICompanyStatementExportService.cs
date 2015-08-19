// -----------------------------------------------------------------------
// <copyright file="ICompanyStatementExportService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyStatement
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyStatementExportService
    {
        string FileName { get; set; }

        void ExportReport(CompanyStatement companyStatement, DateTime winlossDate);        
    }
}