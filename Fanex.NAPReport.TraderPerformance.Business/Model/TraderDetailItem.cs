namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class TraderDetailItem
    {
        public string EventDate { get; set; }

        public string League { get; set; }

        public string Match { get; set; }

        public decimal MaxBet { get; set; }

        public string HaftTimeScore { get; set; }

        public string FullTimeScore { get; set; }

        public TraderBetType WinLoss { get; set; }

        public TraderBetType Turnover { get; set; }

        public TraderBetType LiveBetCount { get; set; }

        public TraderBetType Margin { get; set; }

        public TraderBetType Commission { get; set; }
    }
}
