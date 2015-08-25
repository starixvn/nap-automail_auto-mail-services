// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalance.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalance
    {
        public SuperYesterdayBalance()
        {
            BaseBalanceTotal = new SuperYesterdayBalanceDetail();
            ReportRows = new List<SuperYesterdayBalanceDetail>();
        }

        public List<SuperYesterdayBalanceDetail> ReportRows { get; set; }

        public SuperYesterdayBalanceDetail BaseBalanceTotal { get; set; }
    }
}