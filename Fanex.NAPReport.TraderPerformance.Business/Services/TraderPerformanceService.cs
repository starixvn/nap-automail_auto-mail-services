using Fanex.NAPReport.TraderPerformance.Business.Criteria;
using Fanex.NAPReport.TraderPerformance.Business.Model;
using Fanex.NAPReport.TraderPerformance.Business.Repositories;
using System;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public class TraderPerformanceService : ITraderPerformanceService
    {
        private ITraderPerformanceRepository repository;
        private ICalculationService calculationService;

        public TraderPerformanceService() : this(null, null)
        {
        }

        public TraderPerformanceService(ITraderPerformanceRepository repository, ICalculationService calculationService)
        {
            this.repository = repository ?? new TraderPerformanceRepository();
            this.calculationService = calculationService ?? new CalculationService();
        }

        public TraderPerformanceModel GetReport(DateTime reportDate)
        {
            var criteria = new GetTraderPerformanceCriteria
            {
                Fromdate = reportDate
            };

            var traders = repository.GetRawData(criteria);
            var grandTotal = calculationService.CalculateGrandTotal(traders);

            return new TraderPerformanceModel
            {
                Traders = traders,
                GrandTotal = grandTotal
            };
        }

        public TraderPerformanceDetailModel GetDetailReport(DateTime reportDate)
        {
            var criteria = new GetTraderPerformanceDetailCriteria
            {
                Fromdate = reportDate
            };

            var rawTraderDetails = repository.GetTraderDetailRawData(criteria);
            var traderDetailByUserNames = calculationService.TraderDetailByUserNames(rawTraderDetails);
            var grandTotal = calculationService.CalculateDetailTotal(rawTraderDetails);

            return new TraderPerformanceDetailModel
            {
                TraderDetailByUserNames = traderDetailByUserNames,
                GrandTotal = grandTotal
            };
        }
    }
}
