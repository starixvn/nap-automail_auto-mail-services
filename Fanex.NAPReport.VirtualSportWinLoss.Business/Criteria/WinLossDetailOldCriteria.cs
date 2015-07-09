// WinLossSummary
// <copyright file="WinLossDetailOldCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Criteria
{
    using System;
    using Fanex.Data.Repository;
    /// <summary>
    /// Get win loss detail from DBACC criteria.
    /// </summary>
    public class WinLossDetailOldCriteria : CriteriaBase
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_WLAutomail_VS_Detail_old";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}