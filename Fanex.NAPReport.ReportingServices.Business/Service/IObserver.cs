// -----------------------------------------------------------------------
// <copyright file="IObserver.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IObserver
    {
        void ProcessData(DateTime currentTime);

        string FileName { get; set; }
    }
}