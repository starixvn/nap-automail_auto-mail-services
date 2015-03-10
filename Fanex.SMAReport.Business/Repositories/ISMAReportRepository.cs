// -----------------------------------------------------------------------
// <copyright file="ISMAReportRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Enums;
    using Fanex.SMAReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAReportRepository
    {
        void UpdateTaskStatus(int id, Status status);

        SMAReportModel GetCurrentTask();

        SMAReportModel GetTaskById(int id);

        int InsertTask(SMAReportModel task);

        SMAReportModel GetTaskReadyRun();

        SMAReport GetSMAReport();
    }
}