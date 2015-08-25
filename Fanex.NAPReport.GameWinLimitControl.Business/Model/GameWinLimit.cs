// -----------------------------------------------------------------------
// <copyright file="ProductWinLimit.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimit
    {
        public GameWinLimit()
        {
            ProductName = string.Empty;
            Members = new List<MemberWinLimit>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public List<MemberWinLimit> Members { get; set; }
    }
}
