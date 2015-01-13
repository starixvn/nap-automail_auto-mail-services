// -----------------------------------------------------------------------
// <copyright file="SMAReport.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReport
    {
        public SMAReport()
        {
            ReportRows = new List<SMAReport>();
        }

        public int NumberUserEnabledVirtualRate { get; set; }

        public SMAWinLoss SMAWinLoss { get; set; }

        public SMAVirtualRate SMAVirtualRate { get; set; }

        public List<SMAReport> ReportRows { get; set; }
    }
}
