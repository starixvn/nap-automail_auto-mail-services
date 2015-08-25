// -----------------------------------------------------------------------
// <copyright file="GetSportsWinLossCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class GetSportsWinLossCriteria : CriteriaBase
    {
        public DateTime Winlostdate { get; set; }

        public decimal WinLost_8RM { get; set; }

        public decimal WinLost_BARM { get; set; }

        public decimal IBCTurnover_8RM { get; set; }

        public decimal IBCTurnover_BARM { get; set; }

        public override string GetSettingKey()
        {
            return "Main_MatchWinLoss2a_GetAllDetailReportBySportsType" + BaseService.GetStoreMappingSuffix(this.Winlostdate);
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}