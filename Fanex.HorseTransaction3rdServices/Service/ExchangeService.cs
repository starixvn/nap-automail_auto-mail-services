namespace Fanex.HorseTransaction3rdServices.Service
{
    using System.Collections.Generic;
    using Fanex.HorseTransaction3rdServices.Criteria;
    using Fanex.HorseTransaction3rdServices.Model;
    using Fanex.HorseTransaction3rdServices.Repository;

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
