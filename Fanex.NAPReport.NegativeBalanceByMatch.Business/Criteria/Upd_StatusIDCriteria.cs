// -----------------------------------------------------------------------
// <copyright file="BalanceNegativeUpd_StatusIDCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Upd_StatusIDCriteria : CriteriaBase
    {
        public string LogIDs { get; set; }

        public override bool IsValid()
        {
            return true;
        }

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

        public override string GetSettingKey()
        {
            return "Acc_BalanceNegativeUpd_StatusID";
        }
    }
}
