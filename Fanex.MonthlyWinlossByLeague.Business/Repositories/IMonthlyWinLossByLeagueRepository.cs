// -----------------------------------------------------------------------
// <copyright file="IMonthlyWinLossByLeagueRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.MonthlyWinlossByLeague.Business.Model;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMonthlyWinLossByLeagueRepository
    {
        void UpdateTaskStatus(int id, Status status);

        Task GetCurrentTask();

        Task GetTaskById(int id);

        int InsertTask(Task task);

        Task GetTaskReadyRun();

        List<string> Bettypes { get; }

        List<MonthlyWinLossByLeague> GetMonthlyWinLossByLeague();
    }
}