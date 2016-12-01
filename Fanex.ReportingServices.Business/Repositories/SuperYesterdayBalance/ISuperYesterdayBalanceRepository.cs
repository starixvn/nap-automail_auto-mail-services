// -----------------------------------------------------------------------
// <copyright file="ISuperYesterdayBalanceRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.SuperYesterdayBalance
{
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISuperYesterdayBalanceRepository
    {
        List<SuperYesterdayBalanceDetail> GetSuperYesterdayBalanceDetail(GetSuperYesterdayBalanceCriteria criteria);
    }
}