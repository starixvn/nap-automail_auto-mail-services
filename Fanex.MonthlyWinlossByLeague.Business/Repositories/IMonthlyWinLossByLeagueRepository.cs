// -----------------------------------------------------------------------
// <copyright file="IMonthlyWinLossByLeagueRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
        List<string> Bettypes { get; }

        void UpdateTaskStatus(int id, Status status);

        Task GetCurrentTask();

        Task GetTaskById(int id);

        int InsertTask(Task task);

        Task GetTaskReadyRun();

        List<MonthlyWinLossByLeague> GetMonthlyWinLossByLeague();
    }
}