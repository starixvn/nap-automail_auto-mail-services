// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.WinLossByProducts.Business.Repositories.Interface
{
    using Fanex.WinLossByProducts.Business.Criteria;
    using Fanex.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsRepository
    {
        WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria);
    }
}