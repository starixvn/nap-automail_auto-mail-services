using System.Collections.Generic;

namespace Fanex.NAPReport.TraderPerformance.Business.Model
{
    public class TraderDetailByUserName
    {
        public string Name { get; set; }

        public List<TraderDetailItem> TraderDetails { get; set; }

        public TraderDetailItem Total { get; set; }
    }
}
