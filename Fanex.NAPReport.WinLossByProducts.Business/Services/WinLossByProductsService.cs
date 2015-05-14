// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Repositories;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsService : IWinLossByProductsService
    {
        private IWinLossByProductsRepository _winLossByProductsRepository;

        public WinLossByProductsService()
        {
            this._winLossByProductsRepository = new WinLossByProductsRepository();
        }

        public WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria)
        {
            WinLossByProducts winLossByProducts = _winLossByProductsRepository.GetWinLossByProductsData(criteria);
            return winLossByProducts;
        }
    }
}