// -----------------------------------------------------------------------
// <copyright file="ISuperYesterdayBalanceService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.SuperYesterdayBalance
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISuperYesterdayBalanceService
    {
        SuperYesterdayBalance GetSuperYesterdayBalance(GetSuperYesterdayBalanceCriteria criteria);
    }
}