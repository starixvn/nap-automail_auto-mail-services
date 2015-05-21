namespace Fanex.TurnoverWatcher.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Enums;
    using Fanex.TurnoverWatcher.Business.Model;

    public interface ITurnoverWatcherRepository
    {
        List<TurnoverInfo> GetTurnOverInfo();

        Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel);
    }
}