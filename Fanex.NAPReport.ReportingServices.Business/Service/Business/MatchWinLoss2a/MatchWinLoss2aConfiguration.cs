// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aConfiguration.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Configurations.CustomBettype;
    using Fanex.NAPReport.ReportingServices.Business.Configurations.ExcludeVirtualLineTotal;
    using Fanex.NAPReport.ReportingServices.Business.Configurations.RM;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aConfiguration : IMatchWinLoss2aConfiguration
    {
        private Dictionary<int, string> _excludeVirtualLineTotalByBettypes;
        private Dictionary<string, string> _excludeVirtualLineTotalByProducts;
        private List<BettypeRangeBySport> _excludeVirtualLineTotalByBettypeRange;
        private Dictionary<int, string> _customBettypeNames;
        private List<BettypeRangeBySport> _customBettypeNameByBettypeRange;
        private Dictionary<int, string> _bettypeNamesSuffix;
        private List<BettypeRangeBySport> _bettypeNameNamesSuffixByBettypeRange;
        private RMElement _8rmConfig;
        private RMElement _barmConfig;

        public MatchWinLoss2aConfiguration()
        {
            var excludeVirtualLineTotalSection = ExcludeVirtualLineTotalSection.Instance;
            var customBettypeSection = CustomBettypeSection.Instance;
            this._8rmConfig = RMSection.Instance.Report[RMSection.Reports.MatchWinLoss2a.ToString()]
                                                .ReportConfigs[RMSection.RMType.EightRM.ToString()];
            this._barmConfig = RMSection.Instance.Report[RMSection.Reports.MatchWinLoss2a.ToString()]
                                                 .ReportConfigs[RMSection.RMType.BARM.ToString()];
            InitExcludeVirtualLineTotalBettype(excludeVirtualLineTotalSection.Bettypes);
            InitExcludeVirtualLineTotalByBettypeRange(excludeVirtualLineTotalSection.BettypeRangeBySport);
            InitExcludeVirtualLineTotalSport(excludeVirtualLineTotalSection.ProductNames);
            InitCustomBettypeName(customBettypeSection.BettypeName);
            InitCustomBettypeNameByBettypeRange(customBettypeSection.BettypeNameByBettypeRange);
            InitBettypeNameSuffix(customBettypeSection.BettypeNameSuffix);
            InitBettypeNameSuffixByBettypeRange(customBettypeSection.BettypeNameSuffixByBettypeRange);
        }

        public Dictionary<int, string> ExcludeVirtualLineTotalByBettypes
        {
            get
            {
                return _excludeVirtualLineTotalByBettypes;
            }
        }

        public Dictionary<string, string> ExcludeVirtualLineTotalByProducts
        {
            get
            {
                return _excludeVirtualLineTotalByProducts;
            }
        }

        public List<BettypeRangeBySport> ExcludeVirtualLineTotalByBettypeRange
        {
            get
            {
                return _excludeVirtualLineTotalByBettypeRange;
            }
        }

        public Dictionary<int, string> CustomBettypeNames
        {
            get
            {
                return _customBettypeNames;
            }
        }

        public List<BettypeRangeBySport> CustomBettypeNameByBettypeRange
        {
            get
            {
                return _customBettypeNameByBettypeRange;
            }
        }

        public Dictionary<int, string> BettypeNameSuffix
        {
            get
            {
                return _bettypeNamesSuffix;
            }
        }

        public List<BettypeRangeBySport> BettypeNameNameSuffixByBettypeRange
        {
            get
            {
                return _bettypeNameNamesSuffixByBettypeRange;
            }
        }

        public RMElement EightRMConfig
        {
            get
            {
                return _8rmConfig;
            }
        }

        public RMElement BARMConfig
        {
            get
            {
                return _barmConfig;
            }
        }

        private void InitExcludeVirtualLineTotalBettype(ExcludeVirtualLineTotalCollection collection)
        {
            _excludeVirtualLineTotalByBettypes = new Dictionary<int, string>();
            foreach (ExcludeVirtualLineTotalElement element in collection)
            {
                _excludeVirtualLineTotalByBettypes.Add(element.Id, element.Name);
            }
        }

        private void InitExcludeVirtualLineTotalByBettypeRange(ExcludeVirtualLineTotalCollection collection)
        {
            _excludeVirtualLineTotalByBettypeRange = new List<BettypeRangeBySport>();
            foreach (ExcludeVirtualLineTotalElement element in collection)
            {
                _excludeVirtualLineTotalByBettypeRange.Add(
                    new BettypeRangeBySport
                    {
                        SportName = element.Name,
                        MinBettype = element.MinBettype,
                        MaxBettype = element.MaxBettype
                    });
            }
        }

        private void InitExcludeVirtualLineTotalSport(ExcludeVirtualLineTotalCollection collection)
        {
            _excludeVirtualLineTotalByProducts = new Dictionary<string, string>();
            foreach (ExcludeVirtualLineTotalElement element in collection)
            {
                _excludeVirtualLineTotalByProducts.Add(element.Name, element.Name);
            }
        }

        private void InitCustomBettypeName(CustomBettypeCollection collection)
        {
            _customBettypeNames = new Dictionary<int, string>();
            foreach (CustomBettypeElement element in collection)
            {
                _customBettypeNames.Add(element.Id, element.Name);
            }
        }

        private void InitCustomBettypeNameByBettypeRange(CustomBettypeCollection collection)
        {
            _customBettypeNameByBettypeRange = new List<BettypeRangeBySport>();
            foreach (CustomBettypeElement element in collection)
            {
                _customBettypeNameByBettypeRange.Add(new BettypeRangeBySport
                {
                    SportName = element.Name,
                    MinBettype = element.MinBettype,
                    MaxBettype = element.MaxBettype
                });
            }
        }

        private void InitBettypeNameSuffix(CustomBettypeCollection collection)
        {
            _bettypeNamesSuffix = new Dictionary<int, string>();
            foreach (CustomBettypeElement element in collection)
            {
                _bettypeNamesSuffix.Add(element.Id, element.Name);
            }
        }

        private void InitBettypeNameSuffixByBettypeRange(CustomBettypeCollection collection)
        {
            _bettypeNameNamesSuffixByBettypeRange = new List<BettypeRangeBySport>();
            foreach (CustomBettypeElement element in collection)
            {
                _bettypeNameNamesSuffixByBettypeRange.Add(new BettypeRangeBySport
                {
                    SportName = element.Name,
                    MinBettype = element.MinBettype,
                    MaxBettype = element.MaxBettype
                });
            }
        }
    }
}