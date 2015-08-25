namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;

    public interface IExchangeService
    {
        List<Exchange> GetExchange(GetExchangeCriteria criteria);
    }
}
