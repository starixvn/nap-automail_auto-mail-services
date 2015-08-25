namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;

    public interface ITurnoverWatcherService
    {
        List<TurnoverInfo> GetSMATurnover();

        Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel);
    }
}