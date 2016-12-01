// -----------------------------------------------------------------------
// <copyright file="ISubject.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);

        void RemoveObserver(IObserver observer);

        void NotifyObservers();
    }
}