// -----------------------------------------------------------------------
// <copyright file="CreditTerm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditTerm
    {
        public CreditTerm()
        {
            CreditTermMains = new List<CreditTermMain>();
            CreditTermDetails = new List<CreditTermDetail>();
        }

        public List<CreditTermMain> CreditTermMains { get; set; }

        public List<CreditTermDetail> CreditTermDetails { get; set; }
    }
}
