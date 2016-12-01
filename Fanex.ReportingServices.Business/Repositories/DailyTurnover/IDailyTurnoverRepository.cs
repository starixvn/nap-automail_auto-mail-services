// -----------------------------------------------------------------------
// <copyright file="IDailyTurnoverRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.DailyTurnover
{
    using Fanex.NAPReport.ReportingServices.Business.Criteria.DailyTurnover;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDailyTurnoverRepository
    {
        DailyTurnover GetDailyTurnoverByCurrency(GetDailyTurnoverCriteria criteria);
    }
}