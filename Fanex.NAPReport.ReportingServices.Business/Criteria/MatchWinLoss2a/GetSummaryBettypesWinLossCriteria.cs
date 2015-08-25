// -----------------------------------------------------------------------
// <copyright file="GetSummaryBettypesWinLossCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Criteria.MatchWinLoss2a
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSummaryBettypesWinLossCriteria : CriteriaBase
    {
        public DateTime Winlostdate { get; set; }

        public override string GetSettingKey()
        {
            return "Main_MatchWinloss2a_GetWinLossByBettype" + BaseService.GetStoreMappingSuffix(this.Winlostdate);
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}