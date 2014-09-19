// -----------------------------------------------------------------------
// <copyright file="IReportingServiceRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.ReportingService.Business.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.ReportingService.Business.Enums;
    using Fanex.ReportingService.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportingServiceRepository
    {
        int InsertTaskQueue(TaskType type);

        bool IsRunning();

        bool HasTaskAlreadyRunToday();

        int InsertTaskQueueDetails(TaskQueueDetail info);

        void UpdateStatusTaskQueueDetails(int id, Status status);

        void UpdateStatusTaskQueue(int id, Status status);

        void UpdateRetryTaskQueueDetails(int id, int retry);

        List<TaskQueueDetail> GetTaskDetailsFailed(int id);
    }
}