using Fanex.Common;
using Fanex.Data;
using Fanex.NAPReport.TraderPerformance.Business.Criteria;
using Fanex.NAPReport.TraderPerformance.Business.Model;
using System.Collections.Generic;
using System.Data;

namespace Fanex.NAPReport.TraderPerformance.Business.Repositories
{
    public class TraderPerformanceRepository : ITraderPerformanceRepository
    {
       public List<RawTrader> GetRawData(GetTraderPerformanceCriteria criteria)
        {
            var result = new List<RawTrader>();
            string settingKey = criteria.GetSettingKey();
            using (var objectDb = new ObjectDb(settingKey))
            {
                var reportParams = criteria.GetSpParams();
                var reader = objectDb.ExecuteReader(reportParams);

                if (reader == null)
                {
                    return result;
                }

                while (reader.Read())
                {
                    if (Converter.ToInt(reader["custid"]) == 0 && Converter.ToDecimal(reader["overall_st"]) == 0)
                    {
                        continue;
                    }

                    var winLoss = GetTraderBetType(reader, "winlost", "wl");
                    var turnOver = GetTraderBetType(reader, "stake", "st");
                    var commission = GetTraderBetType(reader, "comm", "com");
                    var margin = GetTraderBetType(reader, "margin", "margin");
                    var liveBetCount = GetTraderBetType(reader, "bet", "livebet");
                    RawTrader traderItem = new RawTrader
                    {
                        Id = Converter.ToInt(reader["custid"]),
                        Name = Converter.ToString2(reader["username"]),
                        MatchNumber = Converter.ToInt(reader["matchno"]),
                        Major = Converter.ToInt(reader["Major"]),
                        Minor1 = Converter.ToInt(reader["Minor1"]),
                        Minor2 = Converter.ToInt(reader["Minor2"]),
                        Minor3 = Converter.ToInt(reader["Minor3"]),
                        Others = Converter.ToInt(reader["Others"]),
                        WinLoss = winLoss,
                        Turnover = turnOver,
                        LiveBetCount = liveBetCount,
                        Margin = margin,
                        Commission = commission
                    };

                    result.Add(traderItem);
                }

                reader.Close();
            }

            return result;
        }

        public List<RawTraderDetail> GetTraderDetailRawData(GetTraderPerformanceDetailCriteria criteria)
        {
            var result = new List<RawTraderDetail>();
            var settingKey = criteria.GetSettingKey();
            using (var objectDb = new ObjectDb(settingKey))
            {
                var reportDetailParams = criteria.GetSpParams();
                var reader = objectDb.ExecuteReader(reportDetailParams);

                if (reader == null)
                {
                    return result;
                }

                while (reader.Read())
                {
                    var winLoss = GetTraderBetType(reader, "winlost", "wl");
                    var turnOver = GetTraderBetType(reader, "stake", "st");
                    var commission = GetTraderBetType(reader, "comm", "com");
                    var margin = GetTraderBetType(reader, "margin", "margin");
                    var liveBetCount = GetTraderBetType(reader, "bet", "livebet");

                    RawTraderDetail traderDetail = new RawTraderDetail
                    {
                        Id = Converter.ToInt(reader["custid"]),
                        Name = Converter.ToString2(reader["username"]),
                        EventDate = Converter.ToDateTime(reader["eventdate"]),
                        LeagueId = Converter.ToInt(reader["leagueid"]),
                        HomeId = Converter.ToInt(reader["homeid"]),
                        AwayId = Converter.ToInt(reader["awayid"]),
                        IsNeutral = Converter.ToBool(reader["IsNeutral"]),
                        HaftTimeHomeScore = Converter.ToInt(reader["hthomescore"]),
                        HaftTimeAwayScore = Converter.ToInt(reader["htawayscore"]),
                        FullTimeHomeScore = Converter.ToInt(reader["fthomescore"]),
                        FullTimeAwayScore = Converter.ToInt(reader["ftawayscore"]),
                        MaxBet = Converter.ToDecimal(reader["maxbet"]),
                        WinLoss = winLoss,
                        Turnover = turnOver,
                        LiveBetCount = liveBetCount,
                        Margin = margin,
                        Commission = commission
                    };

                    result.Add(traderDetail);
                }

                reader.Close();
            }

            return result;
        }

        private TraderBetType GetTraderBetType(
           IDataReader reader,
           string groupNamePrefix,
           string overallSuffix)
        {
            var traderBetType = new TraderBetType
            {
                Hdp = Converter.ToDecimal(reader[groupNamePrefix + "_hdp"]),
                FirstHalfHdp = Converter.ToDecimal(reader[groupNamePrefix + "_1sthdp"]),
                FifteenMinHdp = Converter.ToDecimal(reader[groupNamePrefix + "_15minhdp"]),
                FirstHalfOU = Converter.ToDecimal(reader[groupNamePrefix + "_1stou"]),
                OU = Converter.ToDecimal(reader[groupNamePrefix + "_ou"]),
                FirstHalfOneXTwo = Converter.ToDecimal(reader[groupNamePrefix + "_1st1x2"]),
                OneXTwo = Converter.ToDecimal(reader[groupNamePrefix + "_1x2"]),
                FifteenMinOneXTwo = Converter.ToDecimal(reader[groupNamePrefix + "_15min1x2"]),
                FirstHalfNoOfCorners = Converter.ToDecimal(reader[groupNamePrefix + "_1stNoOfCorners"]),
                NoOfCorners = Converter.ToDecimal(reader[groupNamePrefix + "_NoOfCorners"]),
                FifteenMinOU = Converter.ToDecimal(reader[groupNamePrefix + "_15minOU"]),
                FirstHalfTotalBookings = Converter.ToDecimal(reader[groupNamePrefix + "_1stTotalBookings"]),
                TotalBookings = Converter.ToDecimal(reader[groupNamePrefix + "_TotalBookings"]),
                FirstHalfOE = Converter.ToDecimal(reader[groupNamePrefix + "_1stoe"]),
                OE = Converter.ToDecimal(reader[groupNamePrefix + "_oe"]),
                NextCorner = Converter.ToDecimal(reader[groupNamePrefix + "_NextCorner"]),
                Overall = Converter.ToDecimal(reader["overall_" + overallSuffix]),
                NextBookings = Converter.ToDecimal(reader[groupNamePrefix + "_NextBooking"]),
                TotalBookingsFifteenMinOU = Converter.ToDecimal(reader[groupNamePrefix + "_15minTotalBookings"]),
                TenMinOU = Converter.ToDecimal(reader[groupNamePrefix + "_10minOU"]),
                FifteenMinNoOfCorners = Converter.ToDecimal(reader[groupNamePrefix + "_15minCorners"]),
                SingleTeamOU = Converter.ToDecimal(reader[groupNamePrefix + "_SingleTeamOU"]),
                TenMinOneXTwo = Converter.ToDecimal(reader[groupNamePrefix + "_10min1x2"]),
                TenMinHdp = Converter.ToDecimal(reader[groupNamePrefix + "_10minHdp"]),
            };

            return traderBetType;
        }
    }
}
