// -----------------------------------------------------------------------
// <copyright file="DailyTuroverRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.DailyTurnover
{
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.DailyTurnover;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyTuroverRepository : IDailyTurnoverRepository
    {
        public DailyTurnover GetDailyTurnoverByCurrency(GetDailyTurnoverCriteria criteria)
        {
            DailyTurnover dailyTurnover = null;
            IDataReader reader;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    int ticketCount = Converter.ToInt(reader["TicketCNT"]);
                    int normalCount = Converter.ToInt(reader["NormalCNT"]);
                    int rejectCount = Converter.ToInt(reader["RejectCNT"]);
                    dailyTurnover = new DailyTurnover
                    {
                        TurnoverDetail = new TurnoverDetail
                        {
                            TotalTurnover = Converter.ToDecimal(reader["total"]),
                            SuperTurnover = Converter.ToDecimal(reader["super"]),
                            MasterTurnover = Converter.ToDecimal(reader["master"]),
                            AgentTurnover = Converter.ToDecimal(reader["agent"]),
                            IBCTurnover = Converter.ToDecimal(reader["after"])
                        },
                        Date = Converter.ToDateTime(reader["eventdate"]).ToShortFormatedDate(),
                        TicketNormalRejectCount = string.Format("{0:n0}", ticketCount)
                                                    + "/" + string.Format("{0:n0}", normalCount)
                                                    + "/" + string.Format("{0:n0}", rejectCount)
                    };
                }

                reader.Close();
            }

            return dailyTurnover;
        }
    }
}