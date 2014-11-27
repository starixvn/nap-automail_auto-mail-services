// -----------------------------------------------------------------------
// <copyright file="ITaskRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Repositories
{
    using Fanex.HorseRacingReport.Business.Model;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITaskRepository
    {
        void UpdateTaskStatus(int id, Status status);

        TaskModel GetCurrentTask();

        TaskModel GetTaskReadyRun();

        TaskModel GetTaskById(int id);

        int InsertTask(TaskModel task);
    }
}