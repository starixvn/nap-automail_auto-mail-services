// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.MatchWinLoss2a
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a;
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aRepository : IMatchWinLoss2aRepository
    {
        public List<SummaryBettypeWinLoss> GetSummaryBettypesWinLoss(
            GetSummaryBettypesWinLossCriteria criteria)
        {
            List<SummaryBettypeWinLoss> bettypesWinLoss = new List<SummaryBettypeWinLoss>();
            IDataReader reader;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var summaryBettypesWinLoss = BindingSummaryBettypesWinLossData(reader);
                    bettypesWinLoss.Add(summaryBettypesWinLoss);
                }

                reader.Close();
            }

            return bettypesWinLoss;
        }

        public MatchAndSportWinLoss GetMatchAndThirdPartyProductsWinLoss(
            GetSportsWinLossCriteria criteria)
        {
            MatchAndSportWinLoss matchAndSportWinLoss = new MatchAndSportWinLoss();

            IDataReader reader;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var winLossDetail = BindingMatchWinLossData(reader);
                    matchAndSportWinLoss.MatchWinLoss.Add(winLossDetail);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    var thirdPartyProduct = BindingThirdPartyProductsWinLossData(reader);
                    matchAndSportWinLoss.ThirdPartyProducts.Add(thirdPartyProduct);
                }

                reader.Close();
                //AddOutPutParams(objectDb);
                matchAndSportWinLoss.OutputParams = objectDb.GetOuputParams();
            }

            return matchAndSportWinLoss;
        }

        public SummaryBettypeAndMatchAndSportWinLoss GetFullReport(
            GetFullReportCriteria criteria)
        {
            SummaryBettypeAndMatchAndSportWinLoss fullReport = new SummaryBettypeAndMatchAndSportWinLoss();

            IDataReader reader;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var winLossDetail = BindingMatchWinLossData(reader);
                    fullReport.MatchAndSportWinLoss.MatchWinLoss.Add(winLossDetail);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    var thirdPartyProduct = BindingThirdPartyProductsWinLossData(reader);
                    fullReport.MatchAndSportWinLoss.ThirdPartyProducts.Add(thirdPartyProduct);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    var bettypeWinLoss = BindingSummaryBettypesWinLossData(reader);
                    fullReport.SummaryBettypesWinLoss.Add(bettypeWinLoss);
                }

                reader.Close();
                if (criteria.Winlostdate < BaseService.BalanceUp.BalanceUpDay)
                {
                    AddOutPutParams(objectDb);
                }

                fullReport.MatchAndSportWinLoss.OutputParams = objectDb.GetOuputParams();
            }

            return fullReport;
        }

        private SummaryBettypeWinLoss BindingSummaryBettypesWinLossData(IDataReader reader)
        {
            var summaryBettypesWinLoss = new SummaryBettypeWinLoss()
                {
                    BettypeId = Converter.ToInt(reader["bettype"]),
                    Hedge = Converter.ToString2(reader["Hedge"]),
                    LineTotal_Soccer_Live = Converter.ToDecimal(reader["LineTotal_soccer_live"]),
                    VirtualLineTotal_Soccer_Live = Converter.ToDecimal(reader["VirtualLineTotal_soccer_live"]),
                    LineTotal_Soccer_Non_Live = Converter.ToDecimal(reader["LineTotal_soccer"]),
                    VirtualLineTotal_Soccer_Non_Live = Converter.ToDecimal(reader["VirtualLineTotal_soccer"]),
                    LineTotal_Others_Live = Converter.ToDecimal(reader["LineTotal_others_live"]),
                    VirtualLineTotal_Others_Live = Converter.ToDecimal(reader["VirtualLineTotal_others_live"]),
                    LineTotal_Others_Non_Live = Converter.ToDecimal(reader["LineTotal_others"]),
                    VirtualLineTotal_Others_Non_Live = Converter.ToDecimal(reader["VirtualLineTotal_others"]),
                    Turnover = Converter.ToDecimal(reader["IBCTurnover"])
                };

            return summaryBettypesWinLoss;
        }

        private MatchWinLoss BindingMatchWinLossData(IDataReader reader)
        {
            MatchWinLoss matchWinLoss = new MatchWinLoss()
            {
                MatchCode = Converter.ToString2(reader["matchcode"]),
                KickOffTime = Converter.ToString2(reader["kickofftime"]),
                LiveIndicator = Converter.ToBool(reader["liveindicator"]),
                BettypeId = Converter.ToInt(reader["bettype"]),
                HomeTeamId = Converter.ToInt(reader["homeid"]),
                AwayTeamId = Converter.ToInt(reader["awayid"]),
                WinLoss = Converter.ToDecimal(reader["WinLost"]),
                Position = Converter.ToDecimal(reader["Pos"]),
                Commission = Converter.ToDecimal(reader["Comm"]),
                VirtualCommission = Converter.ToDecimal(reader["VirtualComm"]),
                LineTotal = Converter.ToDecimal(reader["LineTotal"]),
                VirtualLineTotal = Converter.ToDecimal(reader["VirtualLineTotal"]),
                IBCTurnover = Converter.ToDecimal(reader["IBCTurnover"]),
                LeagueName = Converter.ToString2(reader["leaguenametc"]),
                IsNeutral = Converter.ToBool(reader["IsNeutral"])
            };

            return matchWinLoss;
        }

        private SportWinLoss BindingThirdPartyProductsWinLossData(IDataReader reader)
        {
            SportWinLoss productWinLoss = new SportWinLoss()
            {
                WinLossDate = Converter.ToString2(reader["WinlostDate"]),
                WinLoss = Converter.ToDecimal(reader["WinLost"]),
                Position = Converter.ToDecimal(reader["Pos"]),
                Commission = Converter.ToDecimal(reader["Comm"]),
                VirtualCommission = Converter.ToDecimal(reader["VirtualComm"]),
                LineTotal = Converter.ToDecimal(reader["LineTotal"]),
                VirtualLineTotal = Converter.ToDecimal(reader["VirtualLineTotal"]),
                IBCTurnover = Converter.ToDecimal(reader["IBCTurnover"]),
                SportName = Converter.ToString2(reader["bettype"])
            };

            return productWinLoss;
        }

        private void AddOutPutParams(ObjectDb objectDb)
        {
            objectDb.AddOutParameter("winlost_8rm", DbType.Double, 8);
            objectDb.AddOutParameter("winlost_barm", DbType.Double, 8);
            objectDb.AddOutParameter("ibcturnover_8rm", DbType.Double, 8);
            objectDb.AddOutParameter("ibcturnover_barm", DbType.Double, 8);
        }
    }
}