// -----------------------------------------------------------------------
// <copyright file="ISuperYesterdayBalanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.ReportingService.Business.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.ReportingService.Business.Criteria.SuperYesterdayBalance;
    using Fanex.ReportingService.Business.Model.SuperYesterdayBalance;

    public interface ISuperYesterdayBalanceService
    {
        SuperYesterdayBalance GetSuperYesterdayBalance(GetSuperYesterdayBalanceCriteria superYesterdayBalanceCriteria);

        void ExportSuperYesterdayBalanceReport(GetSuperYesterdayBalanceCriteria superYesterdayBalanceCriteria);
    }
}
