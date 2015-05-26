// -----------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="Nexcel">
// TODO: Steven.Nguyen
// </copyright>
namespace Robot.Report.RngCasino.Helper.Implement
{
    using System;
    using Fanex.Logging;

    /// <summary>
    /// Logging extension.
    /// </summary>
    public class LogHelper
    {
        public enum LogType : int
        {
            /// <summary>
            /// The information.
            /// </summary>
            Info,

            /// <summary>
            /// The error type.
            /// </summary>
            Error
        }

        /// <summary>
        /// Logs the specified ex.
        /// </summary>
        /// <param name="ex">The exception detail to write log.</param>
        public static void Log(Exception ex)
        {
            LogService.Error(ex.Message, ex);
            FileHelper.WriteFile(ex.Message + "\n" + ex.StackTrace + "\n");
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(message);
                    break;
                case LogType.Error:
                    LogService.Error(message);
                    break;
            }
        }
    }
}
