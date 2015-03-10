// -----------------------------------------------------------------------
// <copyright file="SMAWinLoss.cs" company="Microsoft">
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
    public class SMAWinLoss
    {
        public SMAWinLoss()
        {
            ReportByMonths = new List<SMAWinLossDetail>();
            ReportRows = new List<SMAWinLoss>();
        }

        public string UserName { get; set; }

        public decimal VirtualRate { get; set; }

        public string Currency { get; set; }

        public List<SMAWinLossDetail> ReportByMonths { get; set; }

        public List<SMAWinLoss> ReportRows { get; set; }
    }
}
