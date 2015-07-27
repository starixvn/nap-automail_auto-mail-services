// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss2aProcessData.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using Fanex.NAPReport.ReportingServices.Business.Service.Business.MatchWinLoss2a;
    using Fanex.NAPReport.ReportingServices.Business.Service.Export.MatchWinLoss2a;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss2aProcessData : IObserver
    {
        private ISubject _serviceProcess;
        private IMatchWinLoss2aService _matchWinLoss2aService;
        private IMatchWinLoss2aExportService _exportService;

        public MatchWinLoss2aProcessData(ISubject serviceProcess)
        {
            this._serviceProcess = serviceProcess;
            serviceProcess.RegisterObserver(this);
        }

        public void ProcessData(DateTime winlossDate)
        {
            _matchWinLoss2aService = new MatchWinLoss2aService();
            _exportService = new MatchWinLoss2aExportService();
            var matchWinLoss2aReport = _matchWinLoss2aService.GetFullReport(winlossDate);
            _exportService.ExportReport(matchWinLoss2aReport, winlossDate);
            FileName = _exportService.FileName;
        }

        public string FileName
        {
            get;
            set;
        }
    }
}