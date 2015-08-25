// -----------------------------------------------------------------------
// <copyright file="IGameWinLimitService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameWinLimitService
    {
        List<GameWinLimit> GetMemberWinLimit(CriteriaBase criteria);

        Result UdpMemberWinLimit(List<GameWinLimit> games);

        Result BlockMembers(List<GameWinLimit> games);
    }
}
