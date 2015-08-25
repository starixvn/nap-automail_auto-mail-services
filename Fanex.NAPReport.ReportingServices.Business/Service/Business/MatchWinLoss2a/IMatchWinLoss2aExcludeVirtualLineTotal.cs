// -----------------------------------------------------------------------
// <copyright file="IMatchWinLoss2aExcludeVirtualLineTotal.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchWinLoss2aExcludeVirtualLineTotal
    {
        bool IsExcludeVirtualLineTotal(int bettypeId);

        bool IsExcludeVirtualLineTotalByProduct(string productName);
    }
}