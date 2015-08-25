namespace Fanex.NAPReport.WinLossByProducts.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;

    public interface IRepository
    {
        List<Product> SimulateWinLossByProducts(Random rd);

        List<Product> SimulateWinLossBySportbook3rdParty(Random rd);

        List<Product> GetOverallWinLossByProducts(CriteriaBase criteria);

        List<Product> GetOverallWinLossBySportbook3rdParty(CriteriaBase criteria);

        DateTime GetLastDateDataAvaiable();
    }
}
