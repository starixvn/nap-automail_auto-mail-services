namespace Fanex.NAPReport.HorseTransaction3rd.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;

    public interface IExchangeRepository
    {
        List<Exchange> GetExchange(GetExchangeCriteria criteria);        
    }
}
