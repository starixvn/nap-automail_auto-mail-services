// -----------------------------------------------------------------------
// <copyright file="MatchInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchInfo
    {
        public string MatchName { get; set; }

        public int LeagueID { get; set; }

        public string LeagueName { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime EventDate { get; set; }

        public string MatchCode { get; set; }

        public string MatchID { get; set; }

        public int GroupId { get; set; }
    }
}
