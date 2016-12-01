// -----------------------------------------------------------------------
// <copyright file="GetDailyTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Criteria.DailyTurnover
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDailyTurnoverCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public bool IsActual { get; set; }

        [SpParam(SqlParamName = "errcode")]
        public bool ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Special_DailyTurnover_GetReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}