// -----------------------------------------------------------------------
// <copyright file="GroupLeagueWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupLeagueWinLoss
    {
        public GroupLeagueWinLoss()
        {
            LeagueWinLoss = new List<LeagueWinLoss>();
        }

        public string GroupName { get; set; }

        public List<LeagueWinLoss> LeagueWinLoss { get; set; }

        public List<BettypeWinLoss> Total { get; set; }
    }
}