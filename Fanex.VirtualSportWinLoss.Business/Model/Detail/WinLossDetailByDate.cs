// -----------------------------------------------------------------------
// <copyright file="WinLossDetailByDate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossDetailByDate
    {
        public WinLossDetailByDate()
        {
            this.BettypeList = new List<WinLossDetailByBettype>();

            this.BettypeIDs = new Dictionary<int, string>();
        }

        public DateTime WinLossDate { get; set; }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public int BetCount { get; set; }

        public Dictionary<int, string> BettypeIDs { get; set; }

        public List<WinLossDetailByBettype> BettypeList { get; set; }
    }
}