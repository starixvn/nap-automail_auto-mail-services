namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.NAPReport.TurnoverWatcher.Business.Repositories;

    public class TurnoverWatcherService : ITurnoverWatcherService
    {
        private ITurnoverWatcherRepository _repository;

        public TurnoverWatcherService()
        {
            _repository = new TurnoverWatcherRepository();
        }

        public Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel)
        {
            return _repository.RemoveNotifiedCustomer(notifiedCustomer, sendMailLevel);
        }

        public List<TurnoverInfo> ProcessTurnoverWatcher()
        {
            var turnoverInfo = _repository.GetTurnOverInfo()
                .OrderBy(x => x.CustomerInfo.PercentageUsed)
                .ToList();

            return turnoverInfo;
        }
    }
}