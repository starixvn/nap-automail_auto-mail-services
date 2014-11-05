// -----------------------------------------------------------------------
// <copyright file="LeagueWinLoss.cs" company="">
// TODO: Update copyright text.
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
    public class LeagueWinLoss
    {
        public string LeagueName { get; set; }

        public int MatchCount { get; set; }

        public List<BettypeWinLoss> BettypeWinLoss { get; set; }
    }
}
