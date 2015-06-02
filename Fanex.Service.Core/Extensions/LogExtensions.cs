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
    }
}