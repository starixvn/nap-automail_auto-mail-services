// -----------------------------------------------------------------------
// <copyright file="WinLossDetailBySportType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossDetailBySportType
    {
        public WinLossDetailBySportType()
        {
            this.WinLossByDateList = new List<WinLossDetailByDate>();
            this.Total = new WinLossDetailByDate();
            this.BettypeIDs = new Dictionary<int, string>();
        }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public bool IsExist { get; set; }

        public Dictionary<int, string> BettypeIDs { get; set; }

        public List<WinLossDetailByDate> WinLossByDateList { get; set; }

        public WinLossDetailByDate Total { get; set; }
    }
}