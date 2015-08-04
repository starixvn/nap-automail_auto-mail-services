using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAPReport.WinLossByProducts.Business.Model;
using Fanex.Data.Repository;

namespace Fanex.NAPReport.WinLossByProducts.Business.Repositories
{
    public interface IRepository
    {
        List<Product> SimulateWinLossByProducts(Random rd);

        List<Product> SimulateWinLossBySportbook3rdParty(Random rd);

        List<Product> GetOverallWinLossByProducts(CriteriaBase criteria);

        List<Product> GetOverallWinLossBySportbook3rdParty(CriteriaBase criteria);

        DateTime GetLastDateDataAvaiable();
    }
}
