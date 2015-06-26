// -----------------------------------------------------------------------
// <copyright file="SMAVirtualRate.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAVirtualRate
    {
        public SMAVirtualRate()
        {
            ReportRows = new List<SMAVirtualRate>();
        }

        public string UserName { get; set; }

        public string Sport { get; set; }

        public bool IsLive { get; set; }

        public List<VirtualRateByBettype> VirtualRateByBettype { get; set; }

        public List<SMAVirtualRate> ReportRows { get; set; }
    }
}
