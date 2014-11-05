// -----------------------------------------------------------------------
// <copyright file="MonthlyWinLossByLeague.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLossByLeague
    {
        public int Period { get; set; }

        public string LiveStatus { get; set; }

        public string GroupName { get; set; }

        public string League { get; set; }

        public BettypeWinLoss BetTypeWinLoss { get; set; }
    }
}
