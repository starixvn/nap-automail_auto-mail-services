// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalanceService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.SuperYesterdayBalance
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Repositories.SuperYesterdayBalance;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalanceService : ISuperYesterdayBalanceService
    {
        private ISuperYesterdayBalanceRepository _superYesterdayBalanceRepository;

        public SuperYesterdayBalanceService()
            : this(null)
        {
        }

        public SuperYesterdayBalanceService(ISuperYesterdayBalanceRepository superYesterdayBalanceRepository)
        {
            _superYesterdayBalanceRepository = superYesterdayBalanceRepository ?? new SuperYesterdayBalanceRepository();
        }

        public SuperYesterdayBalance GetSuperYesterdayBalance(GetSuperYesterdayBalanceCriteria criteria)
        {
            SuperYesterdayBalance balance = new SuperYesterdayBalance();
            try
            {
                var superYesterdayBalanceDetails = _superYesterdayBalanceRepository.GetSuperYesterdayBalanceDetail(criteria);
                foreach (var superBalanceDetail in superYesterdayBalanceDetails)
                {
                    var balanceItemIsNearZero = superBalanceDetail.BalanceIsNearZero(superBalanceDetail);
                    if (!balanceItemIsNearZero)
                    {
                        balance.BaseBalanceTotal.BaseBalance += superBalanceDetail.BaseBalance;
                        superBalanceDetail.FormatBalance();
                        balance.ReportRows.Add(superBalanceDetail);
                    }
                }

                balance.BaseBalanceTotal.FormatBalance();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return balance;
        }
    }
}