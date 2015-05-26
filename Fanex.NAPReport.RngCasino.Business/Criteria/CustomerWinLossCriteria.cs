// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.NAPReport.RngCasino.Business.Criteria
{
	/// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerWinLossCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "amount")]
        public decimal WinLoss { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "RNGCasinoCustomerWinLoss";
        }
    }
}
