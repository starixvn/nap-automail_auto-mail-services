namespace Fanex.NAP.HorseTransaction3rd.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAP.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAP.HorseTransaction3rd.Business.Model;
    using Fanex.NAP.HorseTransaction3rd.Business.Repository;

    public class ExchangeService : IExchangeService
    {
        private IExchangeRepository _repository;

        public ExchangeService()
        {
            _repository = new ExchangeRepository();
        }

        public ExchangeService(IExchangeRepository repository)
        {
            _repository = repository;
        }

        public List<Exchange> GetExchange(GetExchangeCriteria criteria)
        {
            return _repository.GetExchange(criteria);
        }
    }
}
