using System;

namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class RawTraderDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public decimal MaxBet { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }

        public int HaftTimeHomeScore { get; set; }

        public int HaftTimeAwayScore { get; set; }

        public int FullTimeHomeScore { get; set; }

        public int FullTimeAwayScore { get; set; }

        public TraderBetType WinLoss { get; set; }

        public TraderBetType Turnover { get; set; }

        public TraderBetType LiveBetCount { get; set; }

        public TraderBetType Margin { get; set; }

        public TraderBetType Commission { get; set; }
    }
}
