// -----------------------------------------------------------------------
// <copyright file="TurnoverNotifiedUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverNotifiedUpdateCriteria : CriteriaBase
    {
        public string Matchids { get; set; }

        public string Supers { get; set; }

        [SpParam(SqlParamName = "nlevel")]
        public int SendMailLevel { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "TurnoverNotifiedUpdate";
        }
    }
}