// -----------------------------------------------------------------------
// <copyright file="IDailyTurnoverService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.ReportingService.Business.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.ReportingService.Business.Criteria.DailyTurnOver;
    using Fanex.ReportingService.Business.Model.DailyTurnOver;

    public interface IDailyTurnoverService
    {
        List<DailyTurnover> GetReport(GetDailyTurnoverCriteria criteria);

        void ExportReport(GetDailyTurnoverCriteria crtieria);
    }
}