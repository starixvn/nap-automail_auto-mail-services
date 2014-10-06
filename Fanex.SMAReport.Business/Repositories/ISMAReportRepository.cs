// -----------------------------------------------------------------------
// <copyright file="ISMAReportRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Repositories
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.SMAReport.Business.Enums;
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