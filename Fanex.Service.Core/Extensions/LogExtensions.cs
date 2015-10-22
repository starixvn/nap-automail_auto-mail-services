// -----------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using System;
    using Fanex.Logging;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogExtensions
    {
        public static void ExceptionHandler(Exception ex)
        {
            try
            {
                string message = ex.Message +
                                    "<br>\r\nINNEREXCEPTION: " + ex.InnerException +
                                    "<br>\r\nSOURCE: " + ex.Source +
                                    "<br>\r\nTARGETSITE: " + ex.TargetSite +
                                    "<br>\r\nSTACKTRACE: " + ex.StackTrace;                
                IMaintenanceService maintenance = new MaintenanceService();                
                var downTime = maintenance.GetDowntimeInfo();
                if (!downTime.IsDowntime)
                {
                    Logger.Log.Error(message);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error(exception.ToString());
            }
        }

        public static void LogInfo(string msg)
        {
            try
            {
                string message = msg;
                IMaintenanceService maintenance = new MaintenanceService();
                var downTime = maintenance.GetDowntimeInfo();
                if (!downTime.IsDowntime)
                {
                    Logger.Log.Info(message);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error(exception.ToString());
            }
        }

        public static void LogServiceChangeStatusTime(bool isStart, string category = "")
        {
            string message = "Service {0} {1} at {2}.";
            string status = isStart ? "start" : "stop";
            message = string.Format(message, AppSettingBase.ServiceName, status, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            if (string.IsNullOrEmpty(category))
            {
                category = AppSettingBase.WindowsServiceCategory;
            }

            Logger.Log.Info(message, category);
        }
    }
}