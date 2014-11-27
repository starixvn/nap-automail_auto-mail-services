// -----------------------------------------------------------------------
// <copyright file="ICompanyBalanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.ReportingService.Business.Service.Interface
{
    using Fanex.ReportingService.Business.Criteria.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICompanyBalanceService
    {
        void ExportCompanyBalanceReport(GetCompanyBalanceCriteria criteria);
    }
}