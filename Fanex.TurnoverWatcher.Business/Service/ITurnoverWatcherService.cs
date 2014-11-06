// -----------------------------------------------------------------------
// <copyright file="ITurnoverWatcherService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Service
{
    using Fanex.Service.Core.Enums;
    using Fanex.TurnoverWatcher.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITurnoverWatcherService
    {
        int InsertTask(TurnoverWatcherTask task);

        void ProcessTurnoverWatcher();
    }
}