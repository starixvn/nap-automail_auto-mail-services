// -----------------------------------------------------------------------
// <copyright file="ICompanyStatementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyStatement
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyStatementRepository
    {
        CompanyStatement GetCompanyStatement(GetCompanyStatementCriteria criteria);
    }
}