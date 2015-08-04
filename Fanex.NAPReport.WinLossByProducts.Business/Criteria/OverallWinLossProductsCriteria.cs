// -----------------------------------------------------------------------
// <copyright file="OverallWinLossProductsCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OverallWinLossProductsCriteria : CriteriaBase
    {
        public DateTime rptdate
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_Automail_WinlossbyProduct";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
