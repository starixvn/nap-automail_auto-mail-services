// -----------------------------------------------------------------------
// <copyright file="BalanceNegativeSel_BySiteCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Sel_BySiteCriteria : CriteriaBase
    {
        public string Site { get; set; }

        public bool IsTest { get { return NegativeBalanceByMatchSettings.IsTesting; } }

        [SpParam(Ignored = true)]
        public string SpName
        {
            get { return GetSettingKey(); }
        }

        [SpParam(Ignored = true)]
        public object Result
        {
            get;
            set;
        }
        
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Acc_BalanceNegativeSel_BySite";
        }
    }
}
