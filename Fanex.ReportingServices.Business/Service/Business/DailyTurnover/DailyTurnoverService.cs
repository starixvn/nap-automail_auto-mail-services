// -----------------------------------------------------------------------
// <copyright file="DailyTurnoverService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.DailyTurnover
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.DailyTurnover;
    using Fanex.NAPReport.ReportingServices.Business.Model.DailyTurnover;
    using Fanex.NAPReport.ReportingServices.Business.Repositories.DailyTurnover;
    using Fanex.ReportingServices.Business.Resources;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyTurnoverService : IDailyTurnoverService
    {
        private IDailyTurnoverRepository _repository;

        public DailyTurnoverService()
            : this(null)
        {
        }

        public DailyTurnoverService(IDailyTurnoverRepository repository)
        {
            this._repository = repository ?? new DailyTuroverRepository();
        }

        public List<DailyTurnover> GetDailyTurnover(DateTime winlossDate)
        {
            var report = new List<DailyTurnover>();

            var virtualCurrency = _repository.GetDailyTurnoverByCurrency(
                new GetDailyTurnoverCriteria
                {
                    FromDate = winlossDate,
                    IsActual = false
                });
            virtualCurrency.CurrencyType = Labels.Virtual_Currency;
            report.Add(virtualCurrency);

            var actualCurrency = _repository.GetDailyTurnoverByCurrency(
               new GetDailyTurnoverCriteria
               {
                   FromDate = winlossDate,
                   IsActual = true
               });
            actualCurrency.CurrencyType = Labels.Actual_Currency;
            report.Add(actualCurrency);

            return report;
        }
    }
}