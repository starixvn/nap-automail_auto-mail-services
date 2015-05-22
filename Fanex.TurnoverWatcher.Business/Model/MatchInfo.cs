// -----------------------------------------------------------------------
// <copyright file="MatchInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business.Model
{
    using System;
    using System.Globalization;
    using RemotingRef;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchInfo
    {
        public const string Language = "en";

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get { return RefData.GetLeagueName(LeagueId.ToString(CultureInfo.InvariantCulture), Language); }
        }

        public int HomeId { get; set; }

        public string HomeName
        {
            get { return RefData.GetTeamName(HomeId.ToString(CultureInfo.InvariantCulture), Language); }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get { return RefData.GetTeamName(AwayId.ToString(CultureInfo.InvariantCulture), Language); }
        }

        public DateTime EventDate { get; set; }

        public string MatchCode { get; set; }

        public int MatchId { get; set; }

        public int SportType { get; set; }

        public string SportName
        {
            get { return RefData.GetSportName(SportType.ToString(CultureInfo.InvariantCulture), Language); }
        }
    }
}