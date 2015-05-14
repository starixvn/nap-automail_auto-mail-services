// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Repositories
{
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsRepository
    {
        WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria);
    }
}