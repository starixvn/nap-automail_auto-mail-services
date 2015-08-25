// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerBalanceInfo
    {
        public int LogID { get; set; }

        public DateTime LogDate { get; set; }

        public string MatchID { get; set; }

        public int GroupID { get; set; }

        public string CustID { get; set; }

        public string Username { get; set; }

        public double TotalBalance { get; set; }

        public string CurrencyName { get; set; }

        public string Site { get; set; }

        public string CustomInfo1 { get; set; }
    }
}
