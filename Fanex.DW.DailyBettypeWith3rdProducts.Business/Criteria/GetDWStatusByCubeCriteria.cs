// -----------------------------------------------------------------------
// <copyright file="GetDWStatusByCubeCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDWStatusByCubeCriteria : CriteriaBase
    {
        public string Cube
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "BI_GetDWStatusByCube";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
