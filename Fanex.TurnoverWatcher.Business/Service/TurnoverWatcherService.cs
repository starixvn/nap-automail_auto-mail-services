namespace Fanex.NAPReport.TurnoverWatcher.Business.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAPReport.TurnoverWatcher.Business.Model;
    using Fanex.NAPReport.TurnoverWatcher.Business.Repositories;

    public class TurnoverWatcherService : ITurnoverWatcherService
    {
        private IRepository _repository;

        public TurnoverWatcherService()
        {
            _repository = new Repository();
        }

        public Result RemoveNotifiedCustomer(List<TurnoverInfo> notifiedCustomer, int sendMailLevel)
        {
            Result result = new Result();
            result = _repository.RemoveNotifiedCustomer(notifiedCustomer, sendMailLevel);

            return result;
        }

        public List<TurnoverInfo> GetSMATurnover()
        {
            var turnoverInfo = _repository.GetTurnOverInfo()
                .OrderBy(x => x.CustomerInfo.PercentageUsed)
                .ToList();

            return turnoverInfo;
        }
    }
}