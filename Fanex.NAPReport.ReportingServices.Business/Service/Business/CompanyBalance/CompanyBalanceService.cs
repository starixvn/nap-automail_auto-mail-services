// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyBalance
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyBalance;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalanceService : ICompanyBalanceService
    {
        private ICompanyBalanceRepository _repository;

        public CompanyBalanceService()
            : this(null)
        {
        }

        public CompanyBalanceService(ICompanyBalanceRepository repository)
        {
            this._repository = repository ?? new CompanyBalanceRepository();
        }

        public CompanyBalance GetCompanyBalanceReport(GetCompanyBalanceCriteria criteria)
        {
            var balance = new CompanyBalance()
            {
                GrandTotal = new CompanyBalance()
            };

            try
            {
                var companyBalance = _repository.GetCompanyBalanceReport(criteria);
                foreach (var companyBalanceItem in companyBalance.ReportRows)
                {
                    balance.ReportRows.Add(companyBalanceItem);
                    balance.AddToGrandTotal(companyBalanceItem);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return balance;
        }
    }
}