// <copyright file="WinLossSummaryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Criteria;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Extensions;

    public class WinLossSummaryService : IWinLossSummaryService
    {
        private DateTime _moveBetsDay = DateTime.Today;
        private DateTime _firstDayOfMonth = new DateTime(DateTime.Today.AddDays(-1).Year, DateTime.Today.AddDays(-1).Month, 1);
        private DateTime _yesterday = DateTime.Today.AddDays(-1);

        private IRepository _repository;

        public WinLossSummaryService()
        {
            _repository = new Repository();
            _moveBetsDay = _repository.BalanceUp.MoveBetsDay;
            if (AppSettings.IsTest)
            {
                _firstDayOfMonth = AppSettings.DateStart;
                _yesterday = AppSettings.DateEnd;
            }
        }
      
        public WinLossSummaryGrandTotal GetWinLossSummary()
        {
            WinLossSummaryGrandTotal model = new WinLossSummaryGrandTotal();
            try
            {
                List<WinLossSummaryBySportTypeTotal> winlossTotalList = new List<WinLossSummaryBySportTypeTotal>();
                var winlossGroupByBettype = this.GetWinLossSummaryBySportTypeTotalList();
                winlossTotalList = winlossGroupByBettype.GroupBy(x => x.SportID).Select(y =>
                {
                    var winlossTotal = new WinLossSummaryBySportTypeTotal
                    {
                        Total = new WinLossSummaryBySportType
                        {
                            SportID = y.Key,
                            SportName = y.Select(x => x.SportName).First() + " " + WinLossVirtualSportResoure.Total,
                            BettypeID = -1,
                            BettypeName = string.Empty,
                            BetCount = y.Sum(x => x.BetCount),
                            TurnOverIBC = Math.Round(y.Sum(x => x.TurnOverIBC), 2),
                            WinLostIBC = Math.Round(y.Sum(x => x.WinLostIBC), 2)
                        },
                        WinlostList = y.ToList()
                    };

                    model.GrandTotal.SportName = WinLossVirtualSportResoure.GrandTotal;
                    model.GrandTotal.BetCount += winlossTotal.Total.BetCount;
                    model.GrandTotal.TurnOverIBC += winlossTotal.Total.TurnOverIBC;
                    model.GrandTotal.WinLostIBC += winlossTotal.Total.WinLostIBC;

                    return winlossTotal;
                }).ToList();

                model.WinlostTotalList = winlossTotalList;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return model;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<WinLossSummaryBySportType> GetWinLossSummaryBySportTypeTotalList()
        {
            var winlossListFromIBC = new List<WinLossSummaryBySportType>();
            var winlossListFromWH = new List<WinLossSummaryBySportType>();
            var tasks = new Task<List<WinLossSummaryBySportType>>[2];

            if (this._firstDayOfMonth > this._moveBetsDay && this._yesterday > this._moveBetsDay)
            {
                winlossListFromWH = _repository.GetWinLossSummaryListBySportType(new WinLossSummaryCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else if (this._yesterday == this._moveBetsDay)
            {
                winlossListFromWH = _repository.GetWinLossSummaryListBySportType(new WinLossSummaryOldCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else
            {
                tasks[0] = Task<List<WinLossSummaryBySportType>>.Factory.StartNew(() =>
                {
                    return _repository.GetWinLossSummaryListBySportType(new WinLossSummaryOldCriteria
                    {
                        DateStart = this._firstDayOfMonth,
                        DateEnd = this._moveBetsDay
                    });
                });

                tasks[1] = Task<List<WinLossSummaryBySportType>>.Factory.StartNew(() =>
                {
                    return _repository.GetWinLossSummaryListBySportType(new WinLossSummaryCriteria
                    {
                        DateStart = this._moveBetsDay.AddDays(1),
                        DateEnd = this._yesterday
                    });
                });

                Task.WaitAll(tasks);

                winlossListFromWH = tasks[0].Result;
                winlossListFromIBC = tasks[1].Result;
            }

            var winlossGroupByBettype = winlossListFromIBC.Concat(winlossListFromWH).ToList().GroupBy(x => new { x.BettypeID, x.SportID })
                .Select(y => new WinLossSummaryBySportType
                {
                    SportID = y.Key.SportID,
                    SportName = y.Select(x => x.SportName).First(),
                    BettypeID = y.Key.BettypeID,
                    BettypeName = y.Select(x => x.BettypeName).First(),
                    BetCount = y.Sum(x => x.BetCount),
                    TurnOverIBC = Math.Round(y.Sum(x => x.TurnOverIBC), 2),
                    WinLostIBC = Math.Round(y.Sum(x => x.WinLostIBC), 2)
                }).ToList();

            return winlossGroupByBettype;
        }
    }
}