// -----------------------------------------------------------------------
// <copyright file="IMatchWinLoss2aConfiguration.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a
{
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Configurations.RM;
    using Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchWinLoss2aConfiguration
    {
        Dictionary<int, string> ExcludeVirtualLineTotalByBettypes { get; }

        Dictionary<string, string> ExcludeVirtualLineTotalByProducts { get; }

        List<BettypeRangeBySport> ExcludeVirtualLineTotalByBettypeRange { get; }

        Dictionary<int, string> CustomBettypeNames { get; }

        List<BettypeRangeBySport> CustomBettypeNameByBettypeRange { get; }

        Dictionary<int, string> BettypeNameSuffix { get; }

        List<BettypeRangeBySport> BettypeNameNameSuffixByBettypeRange { get; }

        RMElement EightRMConfig { get; }

        RMElement BARMConfig { get; }
    }
}