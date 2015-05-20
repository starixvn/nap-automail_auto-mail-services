// -----------------------------------------------------------------------
// <copyright file="INegativeBalanceByMatchRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Enums;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Repositories
{
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