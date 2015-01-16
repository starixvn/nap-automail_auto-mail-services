// -----------------------------------------------------------------------
// <copyright file="BalanceNegativeUpd_StatusIDCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NegativeBalanceByMatch.Business.Criteria
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
            get { return "Acc_BalanceNegativeUpd_StatusID"; }
        }

        [SpParam(Ignored = true)]
        public object Result
        {
            get;
            set;
        }
    }
}
