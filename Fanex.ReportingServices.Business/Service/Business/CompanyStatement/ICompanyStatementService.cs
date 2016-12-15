// -----------------------------------------------------------------------
// <copyright file="ICompanyStatementService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyStatement
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyStatementService
    {
        CompanyStatement GetCompanyBalanceReport(GetCompanyStatementCriteria criteria);
    }
}