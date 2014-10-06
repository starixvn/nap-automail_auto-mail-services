// -----------------------------------------------------------------------
// <copyright file="SMAReport.cs" company="Microsoft">
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
            ReportByMonths = new List<SMAReportDetail>();
            Report = new List<SMAReport>();
        }

        public string UserName { get; set; }

        public decimal VirtualRate { get; set; }

        public string Currency { get; set; }

        public List<SMAReportDetail> ReportByMonths { get; set; }

        public List<SMAReport> Report { get; set; }
    }
}
