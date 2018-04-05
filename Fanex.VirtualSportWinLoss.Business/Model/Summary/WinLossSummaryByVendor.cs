using System.Collections.Generic;
using Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary;

namespace Fanex.VirtualSportWinLoss.Business.Model.Summary
{
    public class WinLossSummaryByVendor
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int RowCount { get; set; }

        public WinLossSummaryGrandTotal Data { get; set; } = new WinLossSummaryGrandTotal();
    }
}