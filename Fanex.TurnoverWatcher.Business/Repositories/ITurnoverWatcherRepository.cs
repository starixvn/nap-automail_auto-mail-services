// -----------------------------------------------------------------------
// <copyright file="ITurnoverWatcherRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Enums;
    using Fanex.TurnoverWatcher.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITurnoverWatcherRepository
    {
        int InsertTask(TurnoverWatcherTask task);

        void UpdateTask(TurnoverWatcherTask task, Status status);

        TurnoverWatcherTask GetCurrentTask();

        EmailReceiver MailingReceiver(int functionId);

        List<TurnoverInfo> GetTurnOverInfo();

        Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel);
    }
}