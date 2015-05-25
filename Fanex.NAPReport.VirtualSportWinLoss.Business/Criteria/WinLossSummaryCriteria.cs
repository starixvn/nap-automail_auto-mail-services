// WinLossSummary
// <copyright file="WinLossSummaryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get win loss summary from DBWH criteria.
    /// </summary>
    public class WinLossSummaryCriteria : CriteriaBase
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_WLAutomail_VS_Summary";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}