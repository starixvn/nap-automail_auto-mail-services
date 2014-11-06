// -----------------------------------------------------------------------
// <copyright file="ISMAReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using Fanex.SMAReport.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAReportService
    {
        void ProcessSMAReport();

        int InsertTask(SMAReportModel task);

        SMAReportModel GetTaskReadyToRun();
    }
}