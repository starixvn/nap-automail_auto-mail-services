namespace Fanex.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using Fanex.TurnoverWatcher.Business.Model;

    public interface ITurnoverWatcherService
    {
        List<TurnoverInfo> ProcessTurnoverWatcher();
    }
}