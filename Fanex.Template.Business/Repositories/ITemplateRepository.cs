// -----------------------------------------------------------------------
// <copyright file="IResetWinningRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Template.Business.Model;
    using Fanex.Template.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITemplateRepository
    {
        SMAReportModel GetCurrentTask();

        void UpdateTaskStatus(int Id, Status status);

        void MoveTaskToArchive(int Id);

        SMAReportModel GetTaskById(int Id);

        Result ResetWinning(int Id);

        int InsertTask(SMAReportModel task);

        Status GetSatatusByTaskId(int Id);
    }
}
