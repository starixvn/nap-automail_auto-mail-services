// -----------------------------------------------------------------------
// <copyright file="GetDetailRacingReportCriteria.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDetailRacingReportCriteria : CriteriaBase
    {
        public DateTime WinLostDate { get; set; }

        public int PartnerId { get; set; }

        public int PoolCurrency
        {
            get
            {
                return 9;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Racing_GetDetailReport" + BaseService.GetStoreMappingSuffix(this.WinLostDate);
        }
    }
}