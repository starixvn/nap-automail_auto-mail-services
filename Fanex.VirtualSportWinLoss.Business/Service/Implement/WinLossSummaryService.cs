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
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Criteria;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Extensions;
    using Fanex.VirtualSportWinLoss.Business.Model.Summary;
    using Fanex.VirtualSportWinLoss.Business.Resources;

    public class WinLossSummaryService : IWinLossSummaryService
    {
        private DateTime _firstDayOfMonth = new DateTime(DateTime.Today.AddDays(-1).Year, DateTime.Today.AddDays(-1).Month, 1);
        private DateTime _yesterday = DateTime.Today.AddDays(-1);

        private IRepository _repository;

        public WinLossSummaryService()
        {
            _repository = new Repository();
            if (AppSettings.IsTest)
            {
                _firstDayOfMonth = AppSettings.DateStart;
                _yesterday = AppSettings.DateEnd;
            }
        }

        public Tuple<List<WinLossSummaryByVendor>, WinLossSummaryBySportType> GetWinLossSummary()
        {
            var vendors = new List<WinLossSummaryByVendor>();
            var grandTotal = new WinLossSummaryBySportType();
            try
            {
                var winlossGroupByBettype = this.GetWinLossSummaryBySportTypeTotalList();
                winlossGroupByBettype.GroupBy(s => new { s.ProductId, s.ProductName })
                    .ToList()
                    .ForEach(vendor =>
                    {
                        WinLossSummaryGrandTotal model = new WinLossSummaryGrandTotal();
                        var winlossTotalList = vendor.GroupBy(x => x.SportID).Select(y =>
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

                            grandTotal.SportName = WinLossVirtualSportResoure.GrandTotal;
                            grandTotal.BetCount += winlossTotal.Total.BetCount;
                            grandTotal.TurnOverIBC += winlossTotal.Total.TurnOverIBC;
                            grandTotal.WinLostIBC += winlossTotal.Total.WinLostIBC;

                            model.GrandTotal.SportName = vendor.Key.ProductName + " Total";
                            model.GrandTotal.BetCount += winlossTotal.Total.BetCount;
                            model.GrandTotal.TurnOverIBC += winlossTotal.Total.TurnOverIBC;
                            model.GrandTotal.WinLostIBC += winlossTotal.Total.WinLostIBC;

                            return winlossTotal;
                        }).ToList();

                        model.WinlostTotalList = winlossTotalList;
                        vendors.Add(new WinLossSummaryByVendor
                        {
                            ProductId = vendor.Key.ProductId,
                            ProductName = vendor.Key.ProductName,
                            Data = model,
                            RowCount = vendor.Count() + model.WinlostTotalList.Count
                        });
                    });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new Tuple<List<WinLossSummaryByVendor>, WinLossSummaryBySportType>(vendors, grandTotal);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<WinLossSummaryBySportType> GetWinLossSummaryBySportTypeTotalList()
        {
            var winlossListFromWH = _repository.GetWinLossSummaryListBySportType(new WinLossSummaryOldCriteria
            {
                DateStart = this._firstDayOfMonth,
                DateEnd = this._yesterday
            });

            var winlossGroupByBettype = winlossListFromWH.GroupBy(x => new { x.BettypeID, x.SportID })
                .Select(y => new WinLossSummaryBySportType
                {
                    ProductId = y.Select(x => x.ProductId).FirstOrDefault(),
                    ProductName = y.Select(x => x.ProductName).FirstOrDefault(),
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