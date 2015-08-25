// -----------------------------------------------------------------------
// <copyright file="MatchAndSportWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchAndSportWinLoss
    {
        public MatchAndSportWinLoss()
        {
            MatchWinLoss = new List<MatchWinLoss>();
            ThirdPartyProducts = new List<SportWinLoss>();
            OutputParams = new Hashtable();
        }

        public List<MatchWinLoss> MatchWinLoss { get; set; }

        public List<SportWinLoss> ThirdPartyProducts { get; set; }

        public Hashtable OutputParams { get; set; }
    }
}