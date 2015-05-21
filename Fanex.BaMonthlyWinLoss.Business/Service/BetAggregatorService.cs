namespace Fanex.BAMonthlyWinLoss.Business.Service
{
    using System.Collections.Generic;
    using Fanex.BAMonthlyWinLoss.Business.Criteria;
    using Fanex.BAMonthlyWinLoss.Business.Model;
    using Fanex.BAMonthlyWinLoss.Business.Repository;

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
