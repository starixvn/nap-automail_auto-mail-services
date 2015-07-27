// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalanceProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Service.Business.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Service.Export.SuperYesterdayBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalanceProcessData : IObserver
    {
        private ISubject _serviceProcess;
        private ISuperYesterdayBalanceService _superBalanceService;
        private ISuperYesterdayBalanceExportService _exportService;

        public SuperYesterdayBalanceProcessData(ISubject serviceProcess)
        {
            this._serviceProcess = serviceProcess;
            serviceProcess.RegisterObserver(this);
        }

        public void ProcessData(DateTime winlossDate)
        {
            _superBalanceService = new SuperYesterdayBalanceService();
            _exportService = new SuperYesterdayBalanceExportService();
            var balanceReport = _superBalanceService.GetSuperYesterdayBalance(new GetSuperYesterdayBalanceCriteria());
            _exportService.ExportReport(balanceReport, winlossDate);
            FileName = _exportService.FileName;
        }

        public string FileName
        {
            get;
            set;
        }
    }
}