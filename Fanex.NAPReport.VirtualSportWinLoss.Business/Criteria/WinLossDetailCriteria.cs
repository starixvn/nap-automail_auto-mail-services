// WinLossSummary
// <copyright file="WinLossDetailCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get win loss detail from DBWH criteria.
    /// </summary>
    public class WinLossDetailCriteria : CriteriaBase
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_WLAutomail_VS_Detail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}