// -----------------------------------------------------------------------
// <copyright file="IVirtualSportRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories.Interface
{
    using System.Data;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IVirtualSportRepository
    {
        int Insert(TaskType type);

        void UpdateStatus(int id, Status status);

        DataTable GetLogNotCompleteTask();

        bool HasTaskAlreadyRunToday();
    }
}