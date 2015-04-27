// -----------------------------------------------------------------------
// <copyright file="ProcessExChangeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.OandaExChangeRate.Business
{
    using System;
    using System.Threading;
    using Fanex.OandaExChangeRate.Business.DataAccess;
    using Fanex.OandaExChangeRate.Business.Enums;
    using Fanex.OandaExChangeRate.Business.Infrastructure.IoC;
    using Fanex.OandaExChangeRate.Business.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProcessExChangeRate
    {
        private SQLiteDataAccess _log;

        public ProcessExChangeRate()
        {
            _log = new SQLiteDataAccess();
        }

        private void UpdateExchangeRate()
        {
            var rateDate = new DateTime();
            try
            {
                BootStrapperManager.Initialize(BootStrapperFactory.CreateInstance());
                var exchageRateService = new ExRateService();
                var baseCurrencies = AppSettings.BaseCurrencies;

                if (AppSettings.IsTest)
                {
                    exchageRateService.UpdateRate(baseCurrencies, AppSettings.TestDate);
                }
                else if (AppSettings.IsInsertHistoryRate)
                {
                    rateDate = AppSettings.ToHistoryDate;
                    var fromDate = AppSettings.FromHistoryDate;

                    while (!rateDate.ToShortDateString().Equals(fromDate.ToShortDateString()))
                    {
                        exchageRateService.UpdateRate(baseCurrencies, rateDate);
                        rateDate = rateDate.AddDays(-1);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    exchageRateService.UpdateRate(baseCurrencies);
                    exchageRateService.UpdateIbcRate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OnRunningDaily()
        {
            AppSettings.Id = _log.GetTodayLogId();
            int countRunner = 0;

            if (AppSettings.Id <= 0)
            {
                AppSettings.Id = _log.Insert(TaskType.Daily);
            }

            DateTime updatedDate = _log.GetLastUpdatedDate(AppSettings.Id).AddMinutes(AppSettings.RunNextTimeInMinutes);
            DateTime executeDate = updatedDate;
            DateTime currentDate = DateTime.Now;            
            int compareResult = DateTime.Compare(currentDate, executeDate);

            if (compareResult >= 0)
            {
                countRunner = _log.GetCurrentCountRunner(AppSettings.Id);

                if (countRunner < AppSettings.CountRunner)
                {
                    UpdateExchangeRate();
                    countRunner = countRunner + 1;
                    _log.UpdateStatus(AppSettings.Id, Status.Completed);
                    _log.UpdateCountRunner(AppSettings.Id, countRunner);
                }
            }            
        }

        public void OnRunningUrgently()
        {
            UpdateExchangeRate();
        }
    }
}
