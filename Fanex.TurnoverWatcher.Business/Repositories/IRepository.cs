namespace Fanex.NAPReport.TurnoverWatcher.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.Service.Core.Enums;

    public interface IRepository
    {
        List<TurnoverInfo> GetTurnOverInfo();

        Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel);
    }
}