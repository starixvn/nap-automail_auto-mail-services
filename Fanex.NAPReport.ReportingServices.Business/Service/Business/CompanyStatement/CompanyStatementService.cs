// -----------------------------------------------------------------------
// <copyright file="CompanyStatementService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyStatement
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyStatement;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyStatementService : ICompanyStatementService
    {
        private ICompanyStatementRepository _repository;

        public CompanyStatementService()
            : this(null)
        {
        }

        public CompanyStatementService(ICompanyStatementRepository repository)
        {
            this._repository = repository ?? new CompanyStatementRepository();
        }

        public CompanyStatement GetCompanyBalanceReport(GetCompanyStatementCriteria criteria)
        {
            CompanyStatement result = new CompanyStatement();

            try
            {
                CompanyStatement companyStatement = _repository.GetCompanyStatement(criteria);
                decimal accumulatedTotal = 0;
                foreach (var companyStatementItem in companyStatement.ReportRows)
                {
                    accumulatedTotal += companyStatementItem.Total;
                    result.ReportRows.Add(new CompanyStatement
                        {
                            Date = companyStatementItem.Date,
                            Type = companyStatementItem.Type,
                            Total = companyStatementItem.Total,
                            AccumulatedTotal = accumulatedTotal
                        });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}