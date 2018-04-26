// WinLossSummary
// <copyright file="IWinLossSummaryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.VirtualSportWinLoss.Business.Model.Summary;
    using Model.Summary;

    /// <summary>
    /// Win loss detail of virtual sport service's interface.
    /// </summary>
    public interface IWinLossSummaryService
    {
        Tuple<List<WinLossSummaryByVendor>, WinLossSummaryBySportType> GetWinLossSummary();
    }
}