// -----------------------------------------------------------------------
// <copyright file="DailyTurnover.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyTurnover
    {
        public string CurrencyType { get; set; }

        public string Date { get; set; }

        public TurnoverDetail TurnoverDetail { get; set; }

        public string TicketNormalRejectCount { get; set; }
    }
}