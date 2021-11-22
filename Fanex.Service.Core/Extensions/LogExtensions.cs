// -----------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using Fanex.Logging;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using System;

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
                IUMControlAdapter maintenance = new UMControlAdapter();
                var downTime = maintenance.GetUnderMaintenanceTime().IsUnderMaintenanceTime;
                if (!downTime)
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
                IUMControlAdapter maintenance = new UMControlAdapter();
                var downTime = maintenance.GetUnderMaintenanceTime().IsUnderMaintenanceTime;
                if (!downTime)
                {
                    Logger.Log.Info(message);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error(exception.ToString());
            }
        }
    }
}