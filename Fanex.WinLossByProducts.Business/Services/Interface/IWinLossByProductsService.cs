// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.WinLossByProducts.Business.Services.Interface
{
    using Fanex.WinLossByProducts.Business.Criteria;
    using Fanex.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsService
    {
        WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria);
    }
}