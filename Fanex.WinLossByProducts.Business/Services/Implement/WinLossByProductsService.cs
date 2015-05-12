// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.WinLossByProducts.Business.Services.Implement
{
    using Fanex.WinLossByProducts.Business.Criteria;
    using Fanex.WinLossByProducts.Business.Model;
    using Fanex.WinLossByProducts.Business.Repositories.Implement;
    using Fanex.WinLossByProducts.Business.Repositories.Interface;
    using Fanex.WinLossByProducts.Business.Services.Interface;

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