// -----------------------------------------------------------------------
// <copyright file="IDailyTurnoverService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.DailyTurnover
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDailyTurnoverService
    {
        List<DailyTurnover> GetDailyTurnover(DateTime winlossDate);
    }
}