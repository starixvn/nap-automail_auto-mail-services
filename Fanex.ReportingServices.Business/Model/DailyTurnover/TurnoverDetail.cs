// -----------------------------------------------------------------------
// <copyright file="TurnoverDetail.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverDetail
    {
        public decimal TotalTurnover { get; set; }

        public decimal SuperTurnover { get; set; }

        public decimal MasterTurnover { get; set; }

        public decimal AgentTurnover { get; set; }

        public decimal IBCTurnover { get; set; }

        public bool IsActual { get; set; }
    }
}