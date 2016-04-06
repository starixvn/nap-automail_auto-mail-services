using Fanex.Common;
using Fanex.NAPReport.TraderPerformance.Business.Model;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public class CalculationService : ICalculationService
    {
        private IRemotingRefService remotingRefService;

        public CalculationService() : this(null)
        {
        }

        public CalculationService(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public RawTrader CalculateGrandTotal(List<RawTrader> rawTraders)
        {
            var winLoss = SumByBetType(rawTraders.Select(x => x.WinLoss));
            var turnOver = SumByBetType(rawTraders.Select(x => x.Turnover));
            var liveBetCount = SumByBetType(rawTraders.Select(x => x.LiveBetCount));
            var margin = CalculateMargin(winLoss, turnOver);
            var commission = SumByBetType(rawTraders.Select(x => x.Commission));

            return new RawTrader
            {
                Name = "Grand Total",
                MatchNumber = rawTraders.Sum(x => x.MatchNumber),
                Major = rawTraders.Sum(x => x.Major),
                Minor1 = rawTraders.Sum(x => x.Minor1),
                Minor2 = rawTraders.Sum(x => x.Minor2),
                Minor3 = rawTraders.Sum(x => x.Minor3),
                Others = rawTraders.Sum(x => x.Others),
                WinLoss = winLoss,
                Turnover = turnOver,
                LiveBetCount = liveBetCount,
                Margin = margin,
                Commission = commission
            };
        }

        public TraderDetailItem CalculateDetailTotal(List<RawTraderDetail> rawTraderDetails)
        {
            var winLoss = SumByBetType(rawTraderDetails.Select(x => x.WinLoss));
            var turnOver = SumByBetType(rawTraderDetails.Select(x => x.Turnover));
            var liveBetCount = SumByBetType(rawTraderDetails.Select(x => x.LiveBetCount));
            var margin = CalculateMargin(winLoss, turnOver);
            var commission = SumByBetType(rawTraderDetails.Select(x => x.Commission));

            return new TraderDetailItem
            {
                WinLoss = winLoss,
                Turnover = turnOver,
                LiveBetCount = liveBetCount,
                Margin = margin,
                Commission = commission,
            };
        }

        public TraderDetailItem FromRawTraderDetail(RawTraderDetail rawTraderDetail)
        {
            return new TraderDetailItem
            {
                EventDate = rawTraderDetail.EventDate.ToString("dd MMM"),
                League = remotingRefService.GetLeagueName(rawTraderDetail.LeagueId),
                Match = $"{remotingRefService.GetTeamName(rawTraderDetail.HomeId)} - {remotingRefService.GetTeamName(rawTraderDetail.AwayId)}",
                MaxBet = rawTraderDetail.MaxBet,
                HaftTimeScore = $"{rawTraderDetail.HaftTimeHomeScore} - {rawTraderDetail.HaftTimeAwayScore}",
                FullTimeScore = $"{rawTraderDetail.FullTimeHomeScore} - {rawTraderDetail.FullTimeAwayScore}",
                WinLoss = rawTraderDetail.WinLoss,
                Turnover = rawTraderDetail.Turnover,
                LiveBetCount = rawTraderDetail.LiveBetCount,
                Margin = rawTraderDetail.Margin,
                Commission = rawTraderDetail.Commission
           };
        }

        public TraderDetailByUserName TraderDetailByUserNameItem(string userName, IEnumerable<RawTraderDetail> rawTraderDetails)
        {
            return new TraderDetailByUserName
            {
                Name = userName,
                TraderDetails = rawTraderDetails.Select(x => FromRawTraderDetail(x)).ToList(),
                Total = CalculateDetailTotal(rawTraderDetails.ToList())
            };
        }

        public List<TraderDetailByUserName> TraderDetailByUserNames(List<RawTraderDetail> rawTraderDetails)
        {
            var groups = rawTraderDetails.GroupBy(x => new { x.Id, x.Name });
            var traderDetailByUserNames = from @group in groups
                             let userName = Converter.ToString2(@group.Key.Name)
                             select TraderDetailByUserNameItem(userName, @group);

            return traderDetailByUserNames.ToList();
        }

        private TraderBetType SumByBetType(IEnumerable<TraderBetType> traderBetType)
        {
            if (!traderBetType.Any())
            {
                return new TraderBetType();
            }

            return new TraderBetType
            {
                FirstHalfOneXTwo = traderBetType.Sum(x => x.FirstHalfOneXTwo),
                OneXTwo = traderBetType.Sum(x => x.OneXTwo),
                FifteenMinOneXTwo = traderBetType.Sum(x => x.FifteenMinOneXTwo),
                TenMinOneXTwo = traderBetType.Sum(x => x.TenMinOneXTwo),
                FirstHalfHdp = traderBetType.Sum(x => x.FirstHalfHdp),
                Hdp = traderBetType.Sum(x => x.Hdp),
                FifteenMinHdp = traderBetType.Sum(x => x.FifteenMinHdp),
                TenMinHdp = traderBetType.Sum(x => x.TenMinHdp),
                FirstHalfOU = traderBetType.Sum(x => x.FirstHalfOU),
                OU = traderBetType.Sum(x => x.OU),
                FifteenMinOU = traderBetType.Sum(x => x.FifteenMinOU),
                TenMinOU = traderBetType.Sum(x => x.TenMinOU),
                SingleTeamOU = traderBetType.Sum(x => x.SingleTeamOU),
                FirstHalfNoOfCorners = traderBetType.Sum(x => x.FirstHalfNoOfCorners),
                NoOfCorners = traderBetType.Sum(x => x.NoOfCorners),
                FifteenMinNoOfCorners = traderBetType.Sum(x => x.FifteenMinNoOfCorners),
                NextCorner = traderBetType.Sum(x => x.NextCorner),
                FirstHalfTotalBookings = traderBetType.Sum(x => x.FirstHalfTotalBookings),
                TotalBookings = traderBetType.Sum(x => x.TotalBookings),
                TotalBookingsFifteenMinOU = traderBetType.Sum(x => x.TotalBookingsFifteenMinOU),
                NextBookings = traderBetType.Sum(x => x.NextBookings),
                FirstHalfOE = traderBetType.Sum(x => x.FirstHalfOE),
                OE = traderBetType.Sum(x => x.OE),
                Overall = traderBetType.Sum(x => x.Overall)
            };
        }

        private TraderBetType CalculateMargin(TraderBetType winLoss, TraderBetType turnOver)
        {
            return new TraderBetType
            {
                FirstHalfOneXTwo = CalculateMarginValue(winLoss.FirstHalfOneXTwo, turnOver.FirstHalfOneXTwo),
                OneXTwo = CalculateMarginValue(winLoss.OneXTwo, turnOver.OneXTwo),
                FifteenMinOneXTwo = CalculateMarginValue(winLoss.FifteenMinOneXTwo, turnOver.FifteenMinOneXTwo),
                TenMinOneXTwo = CalculateMarginValue(winLoss.TenMinOneXTwo, turnOver.TenMinOneXTwo),
                FirstHalfHdp = CalculateMarginValue(winLoss.FirstHalfHdp, turnOver.FirstHalfHdp),
                Hdp = CalculateMarginValue(winLoss.Hdp, turnOver.Hdp),
                FifteenMinHdp = CalculateMarginValue(winLoss.FifteenMinHdp, turnOver.FifteenMinHdp),
                TenMinHdp = CalculateMarginValue(winLoss.TenMinHdp, turnOver.TenMinHdp),
                FirstHalfOU = CalculateMarginValue(winLoss.FirstHalfOU, turnOver.FirstHalfOU),
                OU = CalculateMarginValue(winLoss.OU, turnOver.OU),
                FifteenMinOU = CalculateMarginValue(winLoss.FifteenMinOU, turnOver.FifteenMinOU),
                TenMinOU = CalculateMarginValue(winLoss.TenMinOU, turnOver.TenMinOU),
                SingleTeamOU = CalculateMarginValue(winLoss.SingleTeamOU, turnOver.SingleTeamOU),
                FirstHalfNoOfCorners = CalculateMarginValue(winLoss.FirstHalfNoOfCorners, turnOver.FirstHalfNoOfCorners),
                NoOfCorners = CalculateMarginValue(winLoss.NoOfCorners, turnOver.NoOfCorners),
                FifteenMinNoOfCorners = CalculateMarginValue(winLoss.FifteenMinNoOfCorners, turnOver.FifteenMinNoOfCorners),
                NextCorner = CalculateMarginValue(winLoss.NextCorner, turnOver.NextCorner),
                FirstHalfTotalBookings = CalculateMarginValue(winLoss.FirstHalfTotalBookings, turnOver.FirstHalfTotalBookings),
                TotalBookings = CalculateMarginValue(winLoss.TotalBookings, turnOver.TotalBookings),
                TotalBookingsFifteenMinOU = CalculateMarginValue(winLoss.TotalBookingsFifteenMinOU, turnOver.TotalBookingsFifteenMinOU),
                NextBookings = CalculateMarginValue(winLoss.NextBookings, turnOver.NextBookings),
                FirstHalfOE = CalculateMarginValue(winLoss.FirstHalfOE, turnOver.FirstHalfOE),
                OE = CalculateMarginValue(winLoss.OE, turnOver.OE),
                Overall = CalculateMarginValue(winLoss.Overall, turnOver.Overall),
            };
        }

        private decimal CalculateMarginValue(decimal winLoss, decimal turnOver)
        {
            return turnOver == 0 ? 0 : winLoss / turnOver;
        }
    }
}
