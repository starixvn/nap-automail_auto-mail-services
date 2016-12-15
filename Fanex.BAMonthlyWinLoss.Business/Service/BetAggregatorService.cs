namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Model;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Repository;

    public class BetAggregatorService : IBetAggregatorService
    {
        private IBetAggregatorRepository _repository;

        public BetAggregatorService(IBetAggregatorRepository repository)
        {
            _repository = repository;
        }

        public BetAggregatorService()
        {
            _repository = new BetAggregatorRepository();
        }

        public List<BALicWinlossDaily> GetBALicWinlossMonthly(GetByDateCriteria criteria)
        {
            return _repository.GetBALicWinlossMonthly(criteria);
        }
    }
}
