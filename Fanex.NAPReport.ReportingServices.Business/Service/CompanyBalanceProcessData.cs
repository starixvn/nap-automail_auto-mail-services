// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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
        private bool _isYesterdayBalance;

        public CompanyBalanceProcessData(ISubject serviceProcess, bool isYesterdayBalance)
        {
            this._serviceProcess = serviceProcess;
            _isYesterdayBalance = isYesterdayBalance;
            serviceProcess.RegisterObserver(this);
        }

        public string FileName
        {
            get;
            set;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public void ProcessData(DateTime winlossDate)
        {
            _companyBalanceService = new CompanyBalanceService();
            _exportService = new CompanyBalanceExportService(_isYesterdayBalance);
            var balanceReport = _companyBalanceService.GetCompanyBalanceReport(new GetCompanyBalanceCriteria() { ReportDate = winlossDate, IsYesterdayBalance = _isYesterdayBalance });
            _exportService.ExportReport(balanceReport, winlossDate);
            FileName = _exportService.FileName;
        }        
    }
}