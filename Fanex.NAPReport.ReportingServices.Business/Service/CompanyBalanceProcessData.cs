// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalanceProcessData : IObserver
    {
        private ISubject _serviceProcess;
        private ICompanyBalanceService _companyBalanceService;
        private ICompanyBalanceExportService _exportService;

        public CompanyBalanceProcessData(ISubject serviceProcess)
        {
            this._serviceProcess = serviceProcess;
            serviceProcess.RegisterObserver(this);
        }

        public void ProcessData(DateTime currentTime)
        {
            _companyBalanceService = new CompanyBalanceService();
            _exportService = new CompanyBalanceExportService();
            var balanceReport = _companyBalanceService.GetCompanyBalanceReport(new GetCompanyBalanceCriteria() { ReportDate = currentTime.AddDays(-1) });
            _exportService.ExportReport(balanceReport, currentTime);
        }
    }
}