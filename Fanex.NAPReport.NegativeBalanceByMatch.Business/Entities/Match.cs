// -----------------------------------------------------------------------
// <copyright file="NegativeBalanceMatch.cs" company="Microsoft">
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
    public class Match
    {
        public Match()
        {
            this.MatchSettleLogList = new List<MatchSettleLog>();
            this.CustomerInfoList = new List<CustomerBalanceInfo>();
            this.MatchInfo = new MatchInfo();
        }

        public MatchInfo MatchInfo { get; set; }

        public List<MatchSettleLog> MatchSettleLogList { get; set; }

        public List<CustomerBalanceInfo> CustomerInfoList { get; set; }

        public string LicenseeName { get; set; }
    }
}
