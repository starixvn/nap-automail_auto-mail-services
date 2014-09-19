// -----------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogExtensions
    {
        public static void ExceptionHandler(Exception ex)
        {
            string message = ex.Message +
                              "<br>\r\nSOURCE: " + ex.Source +
                              "<br>\r\nTARGETSITE: " + ex.TargetSite +
                              "<br>\r\nSTACKTRACE: " + ex.StackTrace;
            Logger.Log.Error(message);
        }
    }
}
