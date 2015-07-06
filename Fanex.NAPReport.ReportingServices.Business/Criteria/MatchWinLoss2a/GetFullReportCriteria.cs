// -----------------------------------------------------------------------
// <copyright file="GetMainReportCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class GetFullReportCriteria : CriteriaBase
    {
        public DateTime Winlostdate { get; set; }

        public double WinLost_8RM { get; set; }

        public double WinLost_BARM { get; set; }

        public override string GetSettingKey()
        {
            return "Main_MatchWinLoss2a_GetMainReport" + BaseService.GetStoreMappingSuffix(this.Winlostdate);
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}