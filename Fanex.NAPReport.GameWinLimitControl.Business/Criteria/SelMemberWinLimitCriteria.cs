// -----------------------------------------------------------------------
// <copyright file="MemberWinLimitCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelMemberWinLimitCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_Notification_MemSel";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
