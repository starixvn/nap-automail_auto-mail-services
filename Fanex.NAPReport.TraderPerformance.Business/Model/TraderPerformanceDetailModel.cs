using System.Collections.Generic;

namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class TraderPerformanceDetailModel
    {
        public List<TraderDetailByUserName> TraderDetailByUserNames { get; set; }

        public TraderDetailItem GrandTotal { get; set; }
    }
}
