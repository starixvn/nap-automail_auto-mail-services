using System.Collections.Generic;

namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class TraderPerformanceModel
    {
        public List<RawTrader> Traders { get; set; }

        public RawTrader GrandTotal { get; set; }
    }
}
