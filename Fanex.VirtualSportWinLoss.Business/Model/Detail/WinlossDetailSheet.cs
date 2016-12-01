// -----------------------------------------------------------------------
// <copyright file="WinlossDetailSheet.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossDetailSheet
    {
        public WinlossDetailSheet()
        {
            this.WinlossBySports = new List<WinLossDetailBySportType>();
        }

        public List<WinLossDetailBySportType> WinlossBySports { get; set; }
    }
}