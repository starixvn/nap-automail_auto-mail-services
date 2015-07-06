// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aBettypeName.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aBettypeName : IMatchWinLoss2aBettypeName
    {
        private IMatchWinLoss2aConfiguration _configuration;

        public MatchWinLoss2aBettypeName()
            : this(null)
        {
        }

        public MatchWinLoss2aBettypeName(IMatchWinLoss2aConfiguration configuration)
        {
            this._configuration = configuration ?? new MatchWinLoss2aConfiguration();
        }

        public string GetBetTypeName(int bettypeId)
        {
            string betTypeName = string.Empty;

            if (_configuration.CustomBettypeNames.ContainsKey(bettypeId))
            {
                betTypeName = _configuration.CustomBettypeNames[bettypeId];
            }
            else if (BettypeIsInBettypeRange(bettypeId))
            {
                betTypeName = GetBettypeNameByBettypeRange(bettypeId);
            }
            else
            {
                betTypeName = CacheRef.GetBettype(bettypeId);
            }
           
            return betTypeName;
        }

        public string GetBettypeNameSuffix(int bettypeId)
        {
            string bettypeNameSuffix = string.Empty;
            if (_configuration.BettypeNameSuffix.ContainsKey(bettypeId))
            {
                return _configuration.BettypeNameSuffix[bettypeId];
            }

            bettypeNameSuffix = GetBettypeNameSuffixByBettypeRange(bettypeId);
            return bettypeNameSuffix;
        }

        private string GetBettypeNameByBettypeRange(int bettypeId)
        {
            string bettypeName = string.Empty;
            foreach (var bettypeRangeBySport in _configuration.CustomBettypeNameByBettypeRange)
            {
                if (bettypeRangeBySport.MinBettype <= bettypeId &&
                    bettypeId <= bettypeRangeBySport.MaxBettype)
                {
                    return bettypeRangeBySport.SportName;
                }
            }

            return bettypeName;
        }

        private string GetBettypeNameSuffixByBettypeRange(int bettypeId)
        {
            string bettypeNameSuffix = string.Empty;
            foreach (var bettypeRangeBySport in _configuration.BettypeNameNameSuffixByBettypeRange)
            {
                if (bettypeRangeBySport.MinBettype <= bettypeId &&
                    bettypeId <= bettypeRangeBySport.MaxBettype)
                {
                    return bettypeRangeBySport.SportName;
                }
            }

            return bettypeNameSuffix;
        }

        private bool BettypeIsInBettypeRange(int bettypeId)
        {
            bool bettypeIsInBettypeRange = false;
            foreach (var bettypeRangeBySport in _configuration.CustomBettypeNameByBettypeRange)
            {
                if (bettypeRangeBySport.MinBettype <= bettypeId &&
                    bettypeId <= bettypeRangeBySport.MaxBettype)
                {
                    return true;
                }
            }

            return bettypeIsInBettypeRange;
        }
    }
}