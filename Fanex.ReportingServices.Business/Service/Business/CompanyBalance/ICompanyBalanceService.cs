// -----------------------------------------------------------------------
// <copyright file="ICompanyBalanceService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyBalance
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyBalanceService
    {
        CompanyBalance GetCompanyBalanceReport(GetCompanyBalanceCriteria criteria);
    }
}