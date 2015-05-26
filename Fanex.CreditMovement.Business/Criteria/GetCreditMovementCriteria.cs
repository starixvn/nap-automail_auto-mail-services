// -----------------------------------------------------------------------
// <copyright file="GetCreditMovementCriteria.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get credit movement criteria.
    /// </summary>
    public class GetCreditMovementCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SiteList { get; set; }

        public int Amount { get; set; }

        public override bool IsValid()
        {
            return this.FromDate <= this.ToDate;
        }

        public override string GetSettingKey()
        {
            return "CreditMovement_GetReport";
        }
    }
}
