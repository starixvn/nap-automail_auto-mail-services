// -----------------------------------------------------------------------
// <copyright file="GetMainRacingReportCriteria.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMainRacingReportCriteria : CriteriaBase
    {
        public int ReportType { get; set; }

        public DateTime WinLostDate { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Racing_GetMainReport" + BaseService.GetStoreMappingSuffix(this.WinLostDate);
        }
    }
}