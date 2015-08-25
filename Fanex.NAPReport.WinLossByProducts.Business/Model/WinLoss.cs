// -----------------------------------------------------------------------
// <copyright file="WinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLoss
    {
        public string Name { get; set; }

        public decimal Today { get; set; }

        public decimal MonthToDate { get; set; }

        public decimal LastMonth { get; set; }

        public decimal YearToDate { get; set; }

        public bool IsMarginTodayNA { get; set; }

        public bool IsMarginMonthToDateNA { get; set; }

        public bool IsMarginLastMonthNA { get; set; }

        public bool IsMarginYearToDateNA { get; set; }
    }
}
