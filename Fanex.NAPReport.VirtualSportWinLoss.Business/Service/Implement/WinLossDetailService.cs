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
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Criteria;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;

    /// <summary>
    /// Win loss detail of virtual sport service's interface.
    /// </summary>
    public class WinLossDetailService : VirtualSportServiceBase, IWinLossDetailService
    {
        private DateTime _moveBetsDay = VirtualSportServiceBase.BalanceUp.MoveBetsDay;
        private DateTime _firstDayOfMonth = new DateTime(DateTime.Today.AddDays(-1).Year, DateTime.Today.AddDays(-1).Month, 1);
        private DateTime _yesterday = DateTime.Today.AddDays(-1);

        private Dictionary<int, string> _sportIDsExistOrNot = new Dictionary<int, string>();

        public WinLossDetailService()
        {
            _sportIDsExistOrNot.Add(180, "Virtual Soccer");
            _sportIDsExistOrNot.Add(181, "Virtual Horse Racing");
            _sportIDsExistOrNot.Add(182, "Virtual Greyhounds");
            _sportIDsExistOrNot.Add(183, "Virtual Speedway");
            _sportIDsExistOrNot.Add(184, "Virtual Motorsports");
            _sportIDsExistOrNot.Add(185, "Virtual Cycling");
            _sportIDsExistOrNot.Add(186, "Virtual Tennis");
        }

        /// <summary>
        /// Gets the win loss detail.
        /// </summary>
        /// <returns>Win loss detail.</returns>
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

        /// <summary>
        /// Creates the type of the bet i ds by sport.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>List bet type by sport.</returns>
        internal Dictionary<int, string> CreateBetIDsBySportType(int sportType)
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

        internal void AddRestSportNotExist(ref List<WinLossDetailBySportType> winlossBySportList)
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

        /// <summary>
        /// Creates the virtual soccer bet type list.
        /// </summary>
        /// <returns>List soccer bet type.</returns>
        internal Dictionary<int, string> CreateVirtualSoccerBetTypeList()
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

        internal Dictionary<int, string> CreateWinAndPlaceBetTypeList()
        {
            var hourseRacingBettypeList = new Dictionary<int, string>();

            hourseRacingBettypeList.Add(1231, "Win");
            hourseRacingBettypeList.Add(1232, "Place");
            hourseRacingBettypeList.Add(1233, "Win/Place");

            return hourseRacingBettypeList;
        }

        internal Dictionary<int, string> CreateVirtualTennisBetTypeList()
        {
            var tennisBettypeList = new Dictionary<int, string>();

            tennisBettypeList.Add(1220, "Moneyline");
            tennisBettypeList.Add(1235, "Correct Score");
            tennisBettypeList.Add(1236, "Total Points");

            return tennisBettypeList;
        }

        internal Dictionary<int, string> CreateSpeedWayBetTypeList()
        {
            var speedWayBettypeList = new Dictionary<int, string>();

            speedWayBettypeList.Add(1231, "Win");

            return speedWayBettypeList;
        }

        /// <summary>
        /// Adds the rest day not exist.
        /// </summary>
        /// <param name="winlossBySport">The win loss by sport.</param>
        internal void AddRestDayNotExist(ref WinLossDetailBySportType winlossBySport)
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

        /// <summary>
        /// Determines whether [is date exist] [the specified win loss by sport].
        /// </summary>
        /// <param name="winlossBySport">The win loss by sport.</param>
        /// <param name="inputDate">The input date.</param>
        /// <returns>Date is exist in report.</returns>
        internal bool IsDateExist(ref WinLossDetailBySportType winlossBySport, DateTime inputDate)
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

        /// <summary>
        /// Gets the win loss detail by date list.
        /// </summary>
        /// <returns>Win loss detail by date.</returns>
        internal List<WinLossDetailByDate> GetWinLossDetailByDateList()
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

        /// <summary>
        /// Gets the win loss detail by bet type list.
        /// </summary>
        /// <returns>Win loss by bet type.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        internal List<WinLossDetailByBettype> GetWinLossDetailByBettypeList()
        {
            var winlossListFromIBC = new List<WinLossDetailByBettype>();
            var winlossListFromWH = new List<WinLossDetailByBettype>();
            var tasks = new Task<List<WinLossDetailByBettype>>[2];

            if (this._firstDayOfMonth > this._moveBetsDay && this._yesterday > this._moveBetsDay)
            {
                winlossListFromWH = this.GetWinLossDetailListByBettype(new WinLossDetailCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else if (this._yesterday == this._moveBetsDay)
            {
                winlossListFromWH = this.GetWinLossDetailListByBettype(new WinLossDetailOldCriteria
                {
                    DateStart = this._firstDayOfMonth,
                    DateEnd = this._yesterday
                });
            }
            else
            {
                tasks[0] = Task<List<WinLossDetailByBettype>>.Factory.StartNew(() =>
                {
                    return this.GetWinLossDetailListByBettype(new WinLossDetailOldCriteria
                    {
                        DateStart = this._firstDayOfMonth,
                        DateEnd = this._moveBetsDay
                    });
                });

                tasks[1] = Task<List<WinLossDetailByBettype>>.Factory.StartNew(() =>
                {
                    return this.GetWinLossDetailListByBettype(new WinLossDetailCriteria
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

        /// <summary>
        /// Get list win loss summary by sport type.
        /// </summary>
        /// <param name="criteria">Criteria by start and end day.</param>
        /// <returns>List win loss summary by sport type.</returns>
        internal List<WinLossDetailByBettype> GetWinLossDetailListByBettype(CriteriaBase criteria)
        {
            List<WinLossDetailByBettype> winlossDetailListbySportType = new List<WinLossDetailByBettype>();
            DataSet dataSet = new DataSet();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var paramsOuput = criteria.GetSpParams();
                dataSet = objDb.ExecuteDataSet(paramsOuput);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var winlossDetailbySportType = new WinLossDetailByBettype
                    {
                        WinLossDate = Converter.To<DateTime>(row["RPTDate"]),
                        SportID = Converter.To<int>(row["SportID"]),
                        SportName = Converter.To<string>(row["SportName"]),
                        BettypeID = Converter.To<int>(row["BetTypeID"]),
                        BettypeName = Converter.To<string>(row["BetType"]),
                        BetCount = Converter.To<int>(row["BetCount"]),
                        TurnOver = Converter.To<double>(row["TurnoverIBC"]),
                        WinLoss = Converter.To<double>(row["WinLostIBC"])
                    };

                    winlossDetailListbySportType.Add(winlossDetailbySportType);
                }
            }

            return winlossDetailListbySportType;
        }
    }
}