// -----------------------------------------------------------------------
// <copyright file="MatchSettleLog.cs" company="Microsoft">
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
    public class MatchSettleLog
    {
        public int LogID { get; set; }

        public DateTime SettlementTime { get; set; }

        public string Action { get; set; }

        public int FinalHomeScore { get; set; }

        public int FinalAwayScore { get; set; }

        public int HTHomeScore { get; set; }

        public int HTAwayScore { get; set; }

        public string Result { get; set; }

        public string Remark { get; set; }
    }
}
