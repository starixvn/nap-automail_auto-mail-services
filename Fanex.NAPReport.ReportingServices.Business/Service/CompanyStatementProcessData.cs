// -----------------------------------------------------------------------
// <copyright file="CompanyStatementProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Service.Business.CompanyStatement;
    using Fanex.NAPReport.ReportingServices.Business.Service.Export.CompanyStatement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyStatementProcessData : IObserver
    {
        private ISubject _serviceProcess;
        private ICompanyStatementService _companyStatementService;
        private ICompanyStatementExportService _exportService;

        public CompanyStatementProcessData(ISubject serviceProcess)
        {
            this._serviceProcess = serviceProcess;
            serviceProcess.RegisterObserver(this);
        }

        public string FileName
        {
            get;
            set;
        }

        public void ProcessData(DateTime winlossDate)
        {
            _companyStatementService = new CompanyStatementService();
            _exportService = new CompanyStatementExportService();
            var companyStatementReport = _companyStatementService.GetCompanyBalanceReport(new GetCompanyStatementCriteria() { FromDate = winlossDate });
            _exportService.ExportReport(companyStatementReport, winlossDate);
            FileName = _exportService.FileName;
        }        
    }
}