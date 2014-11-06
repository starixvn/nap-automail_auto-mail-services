// -----------------------------------------------------------------------
// <copyright file="MonthlyWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLoss
    {
        public MonthlyWinLoss()
        {
            GroupLeagueWinLoss = new List<GroupLeagueWinLoss>();
        }

        public int Period { get; set; }

        public string LiveStatus { get; set; }

        public List<BettypeWinLoss> Total { get; set; }

        public List<GroupLeagueWinLoss> GroupLeagueWinLoss { get; set; }
    }
}
