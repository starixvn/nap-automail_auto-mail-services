namespace Fanex.NAP.HorseTransaction3rd.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAP.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAP.HorseTransaction3rd.Business.Model;

    public interface IExchangeService
    {
        List<Exchange> GetExchange(GetExchangeCriteria criteria);
    }
}
