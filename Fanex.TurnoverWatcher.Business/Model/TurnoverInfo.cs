// -----------------------------------------------------------------------
// <copyright file="TurnoverInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Model
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverInfo
    {
        public MatchInfo MatchInfo { get; set; }

        public CustomerInfo CustomerInfo { get; set; }

        public int SendMailLevel { get; set; }
    }
}