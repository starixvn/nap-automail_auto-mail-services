// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.NAPReport.ReportingServices.Business.Constants;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Repositories.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Service;
    using MatchWinLoss2aEntities = Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aService : IMatchWinLoss2aService
    {
        private IMatchWinLoss2aRepository _repository;
        private IMatchWinLoss2aConfiguration _configuration;
        private IMatchWinLoss2aBettypeName _bettypeName;
        private IMatchWinLoss2aExcludeVirtualLineTotal _excludeVirtualLineTotal;
        private Dictionary<int, string> _cachedBettypeNames;
        private Dictionary<object, string> _teamNameCache;

        public MatchWinLoss2aService()
        {
            this._repository = new MatchWinLoss2aRepository();
            this._configuration = new MatchWinLoss2aConfiguration();
            _bettypeName = new MatchWinLoss2aBettypeName(this._configuration);
            _excludeVirtualLineTotal = new MatchWinLoss2aExcludeVirtualLineTotal(this._configuration);
        }

        public BettypesAndSportsAndThirdPartyProductsWinLoss GetFullReport(DateTime winlossDate)
        {
            BettypesAndSportsAndThirdPartyProductsWinLoss fullReport = new BettypesAndSportsAndThirdPartyProductsWinLoss();
            try
            {
                DateTime winLossDate = winlossDate;
                bool isBeforeBalanceUpDay = winLossDate < BaseService.BalanceUp.BalanceUpDay;
                
                if (isBeforeBalanceUpDay)
                {
                    var summaryBettypesReport = this.GetSummaryBettypesReportBeforeBalanceUpDay(new GetSummaryBettypesWinLossCriteria
                    {
                        Winlostdate = winLossDate
                    });

                    fullReport.SummaryBettypesWinLoss = summaryBettypesReport;

                    var sportsAndThirdPartyProductsWinLoss = this.GetSportsAndThirdPartyProductsWinLossBeforeBalanceUpDay(new GetSportsWinLossCriteria
                    {
                        Winlostdate = winLossDate
                    });

                    fullReport.SportsAndThirdPartyProductsWinLoss = sportsAndThirdPartyProductsWinLoss;
                }
                else
                {
                    fullReport = this.GetFullReportAfterBalanceUpDay(new GetFullReportCriteria
                    {
                        Winlostdate = winLossDate
                    });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return fullReport;
        }

        private BettypesAndSportsAndThirdPartyProductsWinLoss GetFullReportAfterBalanceUpDay(GetFullReportCriteria criteria)
        {
            BettypesAndSportsAndThirdPartyProductsWinLoss fullReport = new BettypesAndSportsAndThirdPartyProductsWinLoss();

            var fullReportData = _repository.GetFullReport(criteria);
            
            var summaryReportByBettypes = this.GetSummaryBettypeWinLoss(fullReportData.SummaryBettypesWinLoss);
            fullReport.SummaryBettypesWinLoss = summaryReportByBettypes;
            
            var sportsWinLoss = this.GetSportsWinLoss(fullReportData.MatchAndSportWinLoss.MatchWinLoss, fullReportData.MatchAndSportWinLoss.OutputParams);
            fullReport.SportsAndThirdPartyProductsWinLoss.SportsWinLoss = sportsWinLoss;
            
            var thirdPartyProductsWinLoss = this.GetThirdPartyProductsWinLoss(fullReportData.MatchAndSportWinLoss.ThirdPartyProducts);
            fullReport.SportsAndThirdPartyProductsWinLoss.ThirdPartyProductsWinLoss = thirdPartyProductsWinLoss;
            return fullReport;
        }

        private SummaryBettypesWinLoss GetSummaryBettypesReportBeforeBalanceUpDay(GetSummaryBettypesWinLossCriteria criteria)
        {
            SummaryBettypesWinLoss summaryBettypesWinLossReport = new SummaryBettypesWinLoss()
                {
                    GrandTotal = new SummaryBettypesWinLoss()
                };
            
            var summaryBettypeWinLoss = _repository.GetSummaryBettypesWinLoss(criteria);
            summaryBettypesWinLossReport = this.GetSummaryBettypeWinLoss(summaryBettypeWinLoss);
            return summaryBettypesWinLossReport;
        }

        private SportsAndThirdPartyProductsWinLoss GetSportsAndThirdPartyProductsWinLossBeforeBalanceUpDay(GetSportsWinLossCriteria criteria)
        {
            SportsAndThirdPartyProductsWinLoss sportsWinLossReport = new SportsAndThirdPartyProductsWinLoss();
            var sportsAndThirdPartyProducts = _repository.GetMatchAndThirdPartyProductsWinLoss(criteria);
            var sportsWinLoss = this.GetSportsWinLoss(sportsAndThirdPartyProducts.MatchWinLoss, sportsAndThirdPartyProducts.OutputParams);
            sportsWinLossReport.SportsWinLoss = sportsWinLoss;
            var thirdPartyProductsWinLoss = this.GetThirdPartyProductsWinLoss(sportsAndThirdPartyProducts.ThirdPartyProducts);
            sportsWinLossReport.ThirdPartyProductsWinLoss = thirdPartyProductsWinLoss;
            return sportsWinLossReport;
        }

        private Dictionary<int, string> CachedBettypeName(List<int> bettypeIds)
        {
            Dictionary<int, string> bettypes = new Dictionary<int, string>();
            foreach (var bettypeId in bettypeIds)
            {
                if (!bettypes.ContainsKey(bettypeId))
                {
                    bettypes.Add(bettypeId, _bettypeName.GetBetTypeName(Converter.ToInt(bettypeId)));
                }
            }

            return bettypes;
        }

        private void CachedTeamName(List<MatchWinLoss2aEntities.MatchWinLoss> matchsWinLoss)
        {
            _teamNameCache = new Dictionary<object, string>();
            var bettypeIds = matchsWinLoss.Select(x => x.BettypeId).Distinct().ToList();
            var homeTeamIds = matchsWinLoss.Select(x => x.HomeTeamId as dynamic).Distinct().ToList();
            var awayTeamIds = matchsWinLoss.Select(x => x.AwayTeamId as dynamic).Distinct().ToList();
            CacheRef.SaveCacheRefResponse(homeTeamIds, CacheRef.GetTeamName, ref _teamNameCache);
            CacheRef.SaveCacheRefResponse(awayTeamIds, CacheRef.GetTeamName, ref _teamNameCache);
        }

        private SummaryBettypesWinLoss GetSummaryBettypeWinLoss(List<MatchWinLoss2aEntities.SummaryBettypeWinLoss> summaryBettypeWinLoss)
        {
            SummaryBettypesWinLoss summaryBettypesWinLossReport = new SummaryBettypesWinLoss()
            {
                GrandTotal = new SummaryBettypesWinLoss()
            };

            var bettypeIds = summaryBettypeWinLoss.Select(x => x.BettypeId).Distinct().ToList();
            _cachedBettypeNames = CachedBettypeName(bettypeIds);
            foreach (var bettype in summaryBettypeWinLoss)
            {
                int bettypeId = bettype.BettypeId;
                if ((bettypeId == RowType.EightRM && !_configuration.EightRMConfig.IsActive) ||
                    (bettypeId == RowType.BARM && !this._configuration.BARMConfig.IsActive))
                {
                    continue;
                }

                var bettypeWinLoss = BindingSummaryBettypeWinLoss(bettype);
                bettypeWinLoss.IsExcludeVirtualLineTotal = _excludeVirtualLineTotal.IsExcludeVirtualLineTotal(bettypeId);
                summaryBettypesWinLossReport.AddToGrandTotal(bettypeWinLoss);
                summaryBettypesWinLossReport.ReportRows.Add(bettypeWinLoss);
            }

            return summaryBettypesWinLossReport;
        }

        private SummaryBettypesWinLoss BindingSummaryBettypeWinLoss(MatchWinLoss2aEntities.SummaryBettypeWinLoss bettypeWinLoss)
        {
            int bettypeId = bettypeWinLoss.BettypeId;
            return new SummaryBettypesWinLoss
            {
                BettypeName = _cachedBettypeNames[bettypeId],
                Name_Suffix = _bettypeName.GetBettypeNameSuffix(bettypeId),
                BettypeId = bettypeId,
                LineTotal_Soccer_Live = bettypeWinLoss.LineTotal_Soccer_Live,
                VirtualLineTotal_Soccer_Live = bettypeWinLoss.VirtualLineTotal_Soccer_Live,
                LineTotal_Soccer_Non_Live = bettypeWinLoss.LineTotal_Soccer_Non_Live,
                VirtualLineTotal_Soccer_Non_Live = bettypeWinLoss.VirtualLineTotal_Soccer_Non_Live,
                LineTotal_Others_Live = bettypeWinLoss.LineTotal_Others_Live,
                VirtualLineTotal_Others_Live = bettypeWinLoss.VirtualLineTotal_Others_Live,
                LineTotal_Others_Non_Live = bettypeWinLoss.LineTotal_Others_Non_Live,
                VirtualLineTotal_Others_Non_Live = bettypeWinLoss.VirtualLineTotal_Others_Non_Live,
                Hedge = bettypeWinLoss.Hedge,
                Turnover = bettypeWinLoss.Turnover
            };
        }

        private SportsWinLoss GetSportsWinLoss(List<MatchWinLoss2aEntities.MatchWinLoss> matchsWinLoss, Hashtable outputParams)
        {
            SportsWinLoss sportsWinLoss = new SportsWinLoss();
            var bettypeIds = matchsWinLoss.Select(x => x.BettypeId).Distinct().ToList();
            CachedBettypeName(bettypeIds);
            CachedTeamName(matchsWinLoss);
            var sportGroups = matchsWinLoss.GroupBy(x => x.LeagueName);
            foreach (var sportGroup in sportGroups)
            {
                WinLossBySport sportWinLoss = new WinLossBySport()
                {
                    SportName = Converter.ToString2(sportGroup.Key)
                };

                var bettypeGroups = sportGroup.GroupBy(x => new
                    {
                        x.BettypeId,
                        x.LiveIndicator
                    });

                foreach (var bettypeGroup in bettypeGroups)
                {
                    var bettypeWinLoss = new BettypeWinLoss();
                    bettypeWinLoss.BettypeName = _cachedBettypeNames[bettypeGroup.Key.BettypeId];
                    bettypeWinLoss.LiveIndicator = bettypeGroup.Key.LiveIndicator;

                    foreach (var bettype in bettypeGroup)
                    {
                        var matchWinLoss = BindingMatchWinLoss(bettype);
                        bettypeWinLoss.MatchWinLoss.Add(matchWinLoss);
                        bettypeWinLoss.BettypesWinLossTotal.Add(matchWinLoss);
                        sportWinLoss.SportWinLossTotal.Add(matchWinLoss);
                        sportsWinLoss.GrandTotal.Add(matchWinLoss);
                    }

                    sportWinLoss.BettypesWinLossBySport.Add(bettypeWinLoss);
                }

                sportsWinLoss.WinLossBySports.Add(sportWinLoss);
            }

            var eightRMTotal = GetEightRMTotal(outputParams);
            sportsWinLoss.EightRMTotal = eightRMTotal;            
            var barmTotal = GetBARMTotal(outputParams);
            sportsWinLoss.BARMTotal = barmTotal;            
            return sportsWinLoss;
        }

        private MatchWinLoss BindingMatchWinLoss(MatchWinLoss2aEntities.MatchWinLoss matchWinLoss)
        {
            int bettypeId = matchWinLoss.BettypeId;
            return new MatchWinLoss
            {
                BettypeId = bettypeId,
                BettypeName = _cachedBettypeNames[bettypeId],
                LiveIndicator = matchWinLoss.LiveIndicator,
                MatchCode = matchWinLoss.MatchCode,
                KickOffTime = matchWinLoss.KickOffTime,
                EventDate = Converter.ToDateTime(matchWinLoss.KickOffTime).ToShortFormatedDateWithSlash(),
                HomeTeamId = matchWinLoss.HomeTeamId,
                HomeTeamName = _teamNameCache[matchWinLoss.HomeTeamId],
                AwayTeamId = matchWinLoss.AwayTeamId,
                AwayTeamName = _teamNameCache[matchWinLoss.AwayTeamId],
                IsNeutral = matchWinLoss.IsNeutral,
                WinLoss = matchWinLoss.WinLoss,
                Position = matchWinLoss.Position,
                Commission = matchWinLoss.Commission,
                VirtualCommission = matchWinLoss.VirtualCommission,
                LineTotal = matchWinLoss.LineTotal,
                VirtualLineTotal = matchWinLoss.VirtualLineTotal,
                Turnover = matchWinLoss.IBCTurnover
            };
        }

        private MatchWinLoss GetEightRMTotal(Hashtable outputParams)
        {
            var eightRMTotal = new MatchWinLoss();
            if (_configuration.EightRMConfig.IsActive)
            {
                eightRMTotal.LineTotal = Converter.ToDecimal(outputParams["winlost_8rm"]);
                eightRMTotal.VirtualLineTotal = eightRMTotal.LineTotal;
                eightRMTotal.Turnover = Converter.ToDecimal(outputParams["ibcturnover_8rm"]);
            }

            return eightRMTotal;
        }

        private MatchWinLoss GetBARMTotal(Hashtable outputParams)
        {
            var barmTotal = new MatchWinLoss();
            if (_configuration.BARMConfig.IsActive)
            {
                barmTotal.LineTotal = Converter.ToDecimal(outputParams["winlost_barm"]);
                barmTotal.VirtualLineTotal = barmTotal.LineTotal;
                barmTotal.Turnover = Converter.ToDecimal(outputParams["ibcturnover_barm"]);
            }

            return barmTotal;
        }

        private SummarySportsWinLoss GetThirdPartyProductsWinLoss(List<MatchWinLoss2aEntities.SportWinLoss> thirdPartyProducts)
        {
            SummarySportsWinLoss thirdPartyProductsWinLoss = new SummarySportsWinLoss
            {
                GrandTotal = new SummarySportsWinLoss
                {
                    SportName = Labels.Grand_Total
                }
            };

            foreach (var product in thirdPartyProducts)
            {
                var productWinLoss = new SummarySportsWinLoss
                    {
                        SportName = product.SportName,
                        WinLossDate = Converter.ToDateTime(product.WinLossDate).ToShortFormatedDateWithSlash(),
                        WinLoss = product.WinLoss,
                        Position = product.Position,
                        Commission = product.Commission,
                        VirtualCommission = product.VirtualCommission,
                        LineTotal = product.LineTotal,
                        VirtualLineTotal = product.VirtualLineTotal,
                        IBCTurnover = product.IBCTurnover
                    };

                productWinLoss.IsExcludeVirtualLineTotal = _excludeVirtualLineTotal.IsExcludeVirtualLineTotalByProduct(product.SportName);
                thirdPartyProductsWinLoss.SummaryWinLossBySports.Add(productWinLoss);
                thirdPartyProductsWinLoss.AddToGrandTotal(productWinLoss);
            }

            return thirdPartyProductsWinLoss;
        }
    }
}