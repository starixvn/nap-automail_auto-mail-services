using Fanex.Data.Repository;
using Fanex.Service.Core.Service;
using System;

namespace Fanex.NAPReport.TraderPerformance.Business.Criteria
{
    public class GetTraderPerformanceCriteria : CriteriaBase
    {
        public DateTime Fromdate { get; set; }

        public DateTime Todate
        {
            get
            {
                return this.Fromdate;
            }
        }

        public bool IsSoccer
        {
            get
            {
                return true;
            }
        }

        public override string GetSettingKey()
        {
            return "Trader_Performance_GetMainReport" + BaseService.GetStoreMappingSuffix(this.Fromdate);
        }
    }
}
