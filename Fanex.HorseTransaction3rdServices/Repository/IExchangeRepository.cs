namespace Fanex.HorseTransaction3rdServices.Repository
{
    using System.Collections.Generic;
    using Fanex.HorseTransaction3rdServices.Criteria;
    using Fanex.HorseTransaction3rdServices.Model;

    public interface IExchangeRepository
    {
        List<Exchange> GetExchange(GetExchangeCriteria criteria);        
    }
}
