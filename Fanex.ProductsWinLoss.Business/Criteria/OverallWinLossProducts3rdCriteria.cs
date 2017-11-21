// -----------------------------------------------------------------------
// <copyright file="OverallWinLossProductsCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OverallWinLossProducts3RDCriteria : CriteriaBase
    {
        public DateTime RptDate
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_Automail_Winloss_3rd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}