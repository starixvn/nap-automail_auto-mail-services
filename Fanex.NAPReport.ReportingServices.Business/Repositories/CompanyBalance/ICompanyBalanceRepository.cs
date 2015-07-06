// -----------------------------------------------------------------------
// <copyright file="ICompanyBalanceRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyBalance
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyBalanceRepository
    {
        CompanyBalance GetCompanyBalanceReport(GetCompanyBalanceCriteria criteria);
    }
}