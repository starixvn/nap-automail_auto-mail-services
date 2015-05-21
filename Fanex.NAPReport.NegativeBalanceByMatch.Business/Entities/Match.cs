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
        public MatchInfo MatchInfo { get; set; }

        public List<MatchSettleLog> MatchSettleLogList { get; set; }

        public List<CustomerBalanceInfo> CustomerInfoList { get; set; }

        public string LicenseeName { get; set; }

        public Match()
        {
            this.MatchSettleLogList = new List<MatchSettleLog>();
            this.CustomerInfoList = new List<CustomerBalanceInfo>();
            this.MatchInfo = new MatchInfo();            
        }
    }

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

    public class CustomerBalanceInfo
    {

        public int LogID { get; set; }

        public DateTime LogDate { get; set; }

        public string MatchID { get; set; }

        public int GroupID { get; set; }

        public string CustID { get; set; }

        public string Username { get; set; }

        public double TotalBalance { get; set; }

        public string CurrencyName { get; set; }

        public string Site { get; set; }

        public string CustomInfo1 { get; set; }

    }

    public class Result
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public Result()
        {
            ErrorCode = 0;
            ErrorMessage = string.Empty;
        }
    }
}
