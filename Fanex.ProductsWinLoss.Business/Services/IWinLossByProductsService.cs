// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsService
    {
        WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria);
    }
}