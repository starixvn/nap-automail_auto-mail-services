// -----------------------------------------------------------------------
// <copyright file="GetCompanyBalanceCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCompanyBalanceCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public DateTime ReportDate { get; set; }

        public bool IsYesterdayBalance { get; set; }

        public override string GetSettingKey()
        {
            return "Balance_CompanyBalance_GetReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}