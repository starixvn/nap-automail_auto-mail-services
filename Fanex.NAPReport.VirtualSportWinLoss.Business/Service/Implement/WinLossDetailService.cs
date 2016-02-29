// <copyright file="WinLossDetailService.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Extensions;

    public class WinLossDetailService : IWinLossDetailService
    {
        private DateTime _moveBetsDay = DateTime.Today;
        private DateTime _firstDayOfMonth = new DateTime(DateTime.Today.AddDays(-1).Year, DateTime.Today.AddDays(-1).Month, 1);
        private DateTime _yesterday = DateTime.Today.AddDays(-1);
        private IRepository _repository;

        private Dictionary<int, string> _sportIDsExistOrNot = new Dictionary<int, string>();

        public WinLossDetailService()
        {
            _repository = new Repository();
            _moveBetsDay = _repository.BalanceUp.MoveBetsDay;
            _sportIDsExistOrNot.Add(180, "Virtual Soccer");
            _sportIDsExistOrNot.Add(181, "Virtual Horse Racing");
            _sportIDsExistOrNot.Add(182, "Virtual Greyhounds");
            _sportIDsExistOrNot.Add(183, "Virtual Speedway");
            _sportIDsExistOrNot.Add(184, "Virtual Motorsports");
            _sportIDsExistOrNot.Add(185, "Virtual Cycling");
            _sportIDsExistOrNot.Add(186, "Virtual Tennis");
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<WinLossDetailBySportType> GetWinLossDetail()
        {
            var winlossBySportList = new List<WinLossDetailBySportType>();
            try
            {
                var winlossGroupByDate = this.GetWinLossDetailByDateList();

                winlossBySportList = winlossGroupByDate.GroupBy(x => x.SportID).Select(y =>
                {
                    var winlossBySport = new WinLossDetailBySportType
                    {
                        SportID = y.Key,
                        IsExist = true,
                        SportName = y.Select(x => x.SportName).FirstOrDefault(),
                        WinLossByDateList = y.ToList()
                    };

                    _sportIDsExistOrNot.Remove(winlossBySport.SportID);

                    foreach (var winlossBydate in winlossBySport.WinLossByDateList)
                    {
                        winlossBySport.BettypeIDs = CreateBetIDsBySportType(winlossBydate.SportID);
                    }

                    AddRestDayNotExist(ref winlossBySport);

                    winlossBySport.WinLossByDateList = winlossBySport.WinLossByDateList.OrderBy(x => x.WinLossDate).ToList();

                    for (int i = 0; i < winlossBySport.WinLossByDateList.Count; i++)
                    {
                        var idsTemp = winlossBySport.BettypeIDs.Take(winlossBySport.BettypeIDs.Count).ToList();
                        var winlossByDateTmp = winlossBySport.WinLossByDateList[i];

                        foreach (var id in winlossByDateTmp.BettypeIDs)
                        {
                            var index = idsTemp.FindIndex(j => j.Key == id.Key); // like Where/Single
                            if (index >= 0)
                            {   // ensure item found
                                idsTemp.RemoveAt(index);
                            }
                        }

                        foreach (var idtemp in idsTemp)
                        {
                            winlossByDateTmp.BettypeList.Add(new WinLossDetailByBettype
                            {
                                BettypeID = idtemp.Key,
                                BettypeName = idtemp.Value,
                                TurnOver = 0,
                                WinLoss = 0
                            });
                        }

                        if (winlossBySport.BettypeIDs.Count > 1)
                        {
                            winlossByDateTmp.BettypeList.Add(new WinLossDetailByBettype
                            {
                                BettypeID = 111111,
                                BettypeName = WinLossVirtualSportResoure.Total,
                                BetCount = winlossByDateTmp.BettypeList.Sum(x => x.BetCount),
                                TurnOver = Math.Round(winlossByDateTmp.BettypeList.Sum(x => x.TurnOver), 2),
                                WinLoss = Math.Round(winlossByDateTmp.BettypeList.Sum(x => x.WinLoss), 2)
                            });
                        }
                        else if (winlossBySport.BettypeIDs.Count == 1)
                        {
                            winlossBySport.WinLossByDateList[i].BettypeList[0].BettypeName += "(Total)";
                        }

                        winlossByDateTmp.BettypeList = winlossByDateTmp.BettypeList.OrderBy(x => x.BettypeID).Distinct().ToList();
                    }

                    return winlossBySport;
                }).Distinct().ToList();

                AddRestSportNotExist(ref winlossBySportList);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return winlossBySportList;
        }

        private Dictionary<int, string> CreateBetIDsBySportType(int sportType)
        {
            var betTypeListBySportType = new Dictionary<int, string>();

            switch (sportType)
            {
                case 180:
                    betTypeListBySportType = CreateVirtualSoccerBetTypeList();
                    break;

                case 181:
                case 182:
                case 184:
                case 185:
                    betTypeListBySportType = CreateWinAndPlaceBetTypeList();
                    break;

                case 183:
                    betTypeListBySportType = CreateSpeedWayBetTypeList();
                    break;

                case 186:
                    betTypeListBySportType = CreateVirtualTennisBetTypeList();
                    break;

                default:
                    break;
            }

            return betTypeListBySportType;
        }

        private void AddRestSportNotExist(ref List<WinLossDetailBySportType> winlossBySportList)
        {
            foreach (var sportIdElement in _sportIDsExistOrNot)
            {
                winlossBySportList.Add(new WinLossDetailBySportType
                {
                    SportID = sportIdElement.Key,
                    SportName = sportIdElement.Value
                });
            }
        }

        private Dictionary<int, string> CreateVirtualSoccerBetTypeList()
        {
            var soccerBettypeList = new Dictionary<int, string>();

            soccerBettypeList.Add(1201, "Handicap");
            soccerBettypeList.Add(1203, "Over/Under 2.5 Goals");
            soccerBettypeList.Add(1204, "Correct Score");
            soccerBettypeList.Add(1205, "1X2");
            soccerBettypeList.Add(1206, "Total Goal");
            soccerBettypeList.Add(1224, "Double Chance");

            return soccerBettypeList;
        }

        private Dictionary<int, string> CreateWinAndPlaceBetTypeList()
        {
            var hourseRacingBettypeList = new Dictionary<int, string>();

            hourseRacingBettypeList.Add(1231, "Win");
            hourseRacingBettypeList.Add(1232, "Place");
            hourseRacingBettypeList.Add(1233, "Win/Place");

            return hourseRacingBettypeList;
        }

        private Dictionary<int, string> CreateVirtualTennisBetTypeList()
        {
            var tennisBettypeList = new Dictionary<int, string>();

            tennisBettypeList.Add(1220, "Moneyline");
            tennisBettypeList.Add(1235, "Correct Score");
            tennisBettypeList.Add(1236, "Total Points");

            return tennisBettypeList;
        }

        private Dictionary<int, string> CreateSpeedWayBetTypeList()
        {
            var speedWayBettypeList = new Dictionary<int, string>();

            speedWayBettypeList.Add(1231, "Win");

            return speedWayBettypeList;
        }
       
        private void AddRestDayNotExist(ref WinLossDetailBySportType winlossBySport)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.AddDays(-1).Year, DateTime.Today.AddDays(-1).Month, 1);

            while (firstDayOfMonth <= this._yesterday)
            {
                if (!IsDateExist(ref winlossBySport, firstDayOfMonth))
                {
                    winlossBySport.WinLossByDateList.Insert(
                        winlossBySport.WinLossByDateList.Count - 1,
                        new WinLossDetailByDate
                        {
                            WinLossDate = firstDayOfMonth,
                            SportID = winlossBySport.SportID,
                            SportName = winlossBySport.SportName,
                            BetCount = 0
                        });
                }

                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            }
        }
       
        private bool IsDateExist(ref WinLossDetailBySportType winlossBySport, DateTime inputDate)
        {
            for (int i = 0; i < winlossBySport.WinLossByDateList.Count; i++)
            {
                if (inputDate.Equals(winlossBySport.WinLossByDateList[i].WinLossDate))
                {
                    return true;
                }
            }

            return false;
        }

        private List<WinLossDetailByDate> GetWinLossDetailByDateList()
        {
            var winlossGroupByBettype = this.GetWinLossDetailByBettypeList();

            var winlossByDateList = winlossGroupByBettype.GroupBy(x => new { x.WinLossDate, x.SportID }).Select(y => new WinLossDetailByDate
            {
                WinLossDate = y.Key.WinLossDate,
                SportID = y.Key.SportID,
                SportName = y.Select(x => x.SportName).FirstOrDefault(),
                BetCount = y.Sum(x => x.BetCount),
                BettypeIDs = y.ToDictionary(p => p.BettypeID, p => p.BettypeName),
                BettypeList = y.ToList()
            }).Distinct().OrderBy(x => x.WinLossDate).ToList();

            return winlossByDateList;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<WinLossDetailByBettype> GetWinLossDetailByBettypeList()
        {
            var winlossListFromIBC = new List<WinLossDetailByBettype>();
            var winlossListFromWH = new List<WinLossDetailByBettype>();
            var tasks = new Task<List<WinLossDetailByBettype>>[2];

            if (this._firstDayOfMonth > this._moveBetsDay && this._yesterday > this._moveBetsDay)
            {
                winlossListFromWH = _repository.GetWinLossDetailListByBettype(new WinLossDetailCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else if (this._yesterday == this._moveBetsDay)
            {
                winlossListFromWH = _repository.GetWinLossDetailListByBettype(new WinLossDetailOldCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else
            {
                tasks[0] = Task<List<WinLossDetailByBettype>>.Factory.StartNew(() =>
                {
                    return _repository.GetWinLossDetailListByBettype(new WinLossDetailOldCriteria
                    {
                        DateStart = this._firstDayOfMonth,
                        DateEnd = this._moveBetsDay
                    });
                });

                tasks[1] = Task<List<WinLossDetailByBettype>>.Factory.StartNew(() =>
                {
                    return _repository.GetWinLossDetailListByBettype(new WinLossDetailCriteria
                    {
                        DateStart = this._moveBetsDay.AddDays(1),
                        DateEnd = this._yesterday
                    });
                });

                Task.WaitAll(tasks);

                winlossListFromWH = tasks[0].Result;
                winlossListFromIBC = tasks[1].Result;
            }

            var winlossListMerge = winlossListFromIBC.Concat(winlossListFromWH).ToList();

            var totalWinlossByDateList = winlossListMerge.GroupBy(x => new { x.BettypeID, x.SportID }).Select(y => new WinLossDetailByBettype
            {
                WinLossDate = DateTime.Today.AddDays(1),
                SportID = y.Key.SportID,
                SportName = y.Select(x => x.SportName).FirstOrDefault(),
                BettypeID = y.Key.BettypeID,
                BettypeName = y.Select(x => x.BettypeName).FirstOrDefault(),
                BetCount = y.Sum(x => x.BetCount),
                TurnOver = Math.Round(y.Sum(x => x.TurnOver), 2),
                WinLoss = Math.Round(y.Sum(x => x.WinLoss), 2)
            }).OrderBy(x => x.SportID).ToList();

            return winlossListMerge.Concat(totalWinlossByDateList).Distinct().ToList();
        }
    }
}