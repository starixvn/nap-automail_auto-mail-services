// -----------------------------------------------------------------------
// <copyright file="WinLossSummaryGrandTotal.cs" company="Nexcel Solutions Vietnam">
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
    public class WinLossSummaryGrandTotal
    {
        public WinLossSummaryGrandTotal()
        {
            this.WinlostTotalList = new List<WinLossSummaryBySportTypeTotal>();         
        }

        public WinLossSummaryBySportType GrandTotal { get; set; } = new WinLossSummaryBySportType();

        public List<WinLossSummaryBySportTypeTotal> WinlostTotalList { get; set; }
    }
}