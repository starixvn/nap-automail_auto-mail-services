// -----------------------------------------------------------------------
// <copyright file="IGameWinLimitRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Repositories
{
    using System.Data;
    using Fanex.Data.Repository;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameWinLimitRepository
    {
        DataTable GetMemberWinLimit(CriteriaBase criteria);

        Result UpdateNotificationMemberWinLimit(CriteriaBase criteria);
    }
}
