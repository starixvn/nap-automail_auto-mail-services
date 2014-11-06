// -----------------------------------------------------------------------
// <copyright file="TurnoverNotifiedSelectCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.TurnoverWatcher.Business.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverNotifiedSelectCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "TurnoverNotifiedSelect";
        }
    }
}