// -----------------------------------------------------------------------
// <copyright file="WinLossSummaryBySportTypeTotal.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossSummaryBySportTypeTotal
    {
        public WinLossSummaryBySportTypeTotal()
        {
            this.WinlostList = new List<WinLossSummaryBySportType>();
            this.Total = new WinLossSummaryBySportType();
        }

        public WinLossSummaryBySportType Total { get; set; }

        public List<WinLossSummaryBySportType> WinlostList { get; set; }
    }
}