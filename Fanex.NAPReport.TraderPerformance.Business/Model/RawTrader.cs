namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class RawTrader
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MatchNumber { get; set; }

        public int Major { get; set; }

        public int Minor1 { get; set; }

        public int Minor2 { get; set; }

        public int Minor3 { get; set; }

        public int Others { get; set; }

        public TraderBetType WinLoss { get; set; }

        public TraderBetType Turnover { get; set; }

        public TraderBetType LiveBetCount { get; set; }

        public TraderBetType Margin { get; set; }

        public TraderBetType Commission { get; set; }
    }
}
