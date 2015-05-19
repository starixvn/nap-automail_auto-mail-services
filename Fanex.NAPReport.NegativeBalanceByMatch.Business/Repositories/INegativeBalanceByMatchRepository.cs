// -----------------------------------------------------------------------
// <copyright file="INegativeBalanceByMatchRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Repositories
{
    using Fanex.NAPReport.Business.Entities;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INegativeBalanceByMatchRepository
    {
        int InsertTask(NegativeBalanceTaskInfo task);

        void UpdateTaskStatus(int id, Status status);

        void UpdateTaskStatus(int id, Status status, string logData);
    }
}