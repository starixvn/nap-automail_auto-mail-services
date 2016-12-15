// -----------------------------------------------------------------------
// <copyright file="ServiceProcess.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Service
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceProcess : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        private DateTime _currentTime = DateTime.Now;

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.ProcessData(_currentTime);
            }
        }

        public void RunService(DateTime currentTime)
        {
            _currentTime = currentTime;
            NotifyObservers();
        }
    }
}