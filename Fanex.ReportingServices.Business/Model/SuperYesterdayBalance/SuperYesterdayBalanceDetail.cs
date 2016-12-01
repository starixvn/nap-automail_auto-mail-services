// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalanceDetail.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalanceDetail
    {
        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public decimal BaseBalance { get; set; }

        public void FormatBalance()
        {
            this.Balance = Math.Round(this.Balance, 2, MidpointRounding.AwayFromZero);
            this.BaseBalance = Math.Round(this.BaseBalance, 2, MidpointRounding.AwayFromZero);
        }

        public bool BalanceIsNearZero(SuperYesterdayBalanceDetail balance)
        {
            return Math.Round(balance.Balance, 2, MidpointRounding.AwayFromZero) == 0 &&
                Math.Round(balance.BaseBalance, 2, MidpointRounding.AwayFromZero) == 0;
        }
    }
}