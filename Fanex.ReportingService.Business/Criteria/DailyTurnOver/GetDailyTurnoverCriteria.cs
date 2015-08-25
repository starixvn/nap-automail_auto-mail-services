// -----------------------------------------------------------------------
// <copyright file="GetDailyTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.ReportingService.Business.Criteria.DailyTurnOver
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get daily turn over criteria.
    /// </summary>
    public class GetDailyTurnoverCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public bool IsActual { get; set; }

        [SpParam(SqlParamName = "errcode")]
        public bool ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns></returns>
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