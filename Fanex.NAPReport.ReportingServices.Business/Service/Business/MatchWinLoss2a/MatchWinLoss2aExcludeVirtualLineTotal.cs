// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aExcludeVirtualLineTotal.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aExcludeVirtualLineTotal : IMatchWinLoss2aExcludeVirtualLineTotal
    {
        private IMatchWinLoss2aConfiguration _configuration;

        public MatchWinLoss2aExcludeVirtualLineTotal()
            : this(null)
        {
        }

        public MatchWinLoss2aExcludeVirtualLineTotal(IMatchWinLoss2aConfiguration configuration)
        {
            this._configuration = configuration ?? new MatchWinLoss2aConfiguration();
        }

        public bool IsExcludeVirtualLineTotal(int bettypeId)
        {
            return IsExcludeVirtualLineTotalByBettype(bettypeId) || IsExcludeVirtualLineTotalInBetypeRangeBySport(bettypeId);
        }

        public bool IsExcludeVirtualLineTotalByProduct(string productName)
        {
            var isExcludeVirtualLineTotal = false;
            if (_configuration.ExcludeVirtualLineTotalByProducts.ContainsValue(productName))
            {
                isExcludeVirtualLineTotal = true;
            }

            return !string.IsNullOrEmpty(productName) && isExcludeVirtualLineTotal;
        }

        private bool IsExcludeVirtualLineTotalByBettype(int bettypeId)
        {
            var isExcludeVirtualLineTotal = false;
            if (_configuration.ExcludeVirtualLineTotalByBettypes.ContainsKey(bettypeId))
            {
                isExcludeVirtualLineTotal = true;
            }

            return isExcludeVirtualLineTotal;
        }

        private bool IsExcludeVirtualLineTotalInBetypeRangeBySport(int bettypeId)
        {
            var isExcludeVirtualLineTotal = false;
            foreach (var bettype in _configuration.ExcludeVirtualLineTotalByBettypeRange)
            {
                if (bettype.MinBettype <= bettypeId && bettypeId <= bettype.MaxBettype)
                {
                    isExcludeVirtualLineTotal = true;
                    return isExcludeVirtualLineTotal;
                }
            }

            return isExcludeVirtualLineTotal;
        }
    }
}