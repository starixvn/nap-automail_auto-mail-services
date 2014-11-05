// -----------------------------------------------------------------------
// <copyright file="IMonthlyWinLossByLeagueService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.MonthlyWinlossByLeague.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMonthlyWinLossByLeagueService
    {
        Dictionary<int, string> Bettypes { get; }

        List<MonthlyWinLoss> GetReport();

        int InsertTask(Task task);

        Task GetTaskReadyToRun();

        void ProcessMonthlyWinLossByLeagueReport();
    }
}
