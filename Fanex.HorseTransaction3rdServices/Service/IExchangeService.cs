namespace Fanex.HorseTransaction3rdServices.Service
{
    using System.Collections.Generic;
    using Fanex.HorseTransaction3rdServices.Criteria;
    using Fanex.HorseTransaction3rdServices.Model;

    public interface IExchangeService
    {
        List<Exchange> GetExchange(GetExchangeCriteria criteria);
    }
}
