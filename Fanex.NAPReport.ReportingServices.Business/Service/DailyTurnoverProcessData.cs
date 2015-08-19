// -----------------------------------------------------------------------
// <copyright file="DailyTurnoverProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Service.Business.DailyTurnover;
    using Fanex.NAPReport.ReportingServices.Business.Service.Export.DailyTurnover;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyTurnoverProcessData : IObserver
    {
        private ISubject _serviceProcess;
        private IDailyTurnoverService _dailyTurnoverService;
        private IDailyTurnoverExportService _exportService;

        public DailyTurnoverProcessData(ISubject serviceProcess)
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
            _dailyTurnoverService = new DailyTurnoverService();
            _exportService = new DailyTurnoverExportService();
            var dailyTurnoverReport = _dailyTurnoverService.GetDailyTurnover(winlossDate);
            _exportService.ExportReport(dailyTurnoverReport, winlossDate);
            FileName = _exportService.FileName;
        }        
    }
}