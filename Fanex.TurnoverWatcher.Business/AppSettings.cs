// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TurnoverWatcher.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public static string ServiceName
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServiceName"];
                return val == null ? "_Fanex.TurnoverWatcher.Service" : val;
            }
        }

        /// <summary>
        /// Gets the interval in second.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public static int Interval
        {
            get
            {
                var val = ConfigurationManager.AppSettings["Interval.Second"];
                int interval = val == null ? 20 : Convert.ToInt32(val) * 1000;
                return interval;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [on processing].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on processing]; otherwise, <c>false</c>.
        /// </value>
        public static bool OnProcessing { get; set; }

        /// <summary>
        /// Gets connection string.
        /// </summary>
        /// <value>
        /// The Minute.
        /// </value>
        public static string ConnectionString
        {
            get
            {
                var val = ConfigurationManager.AppSettings["SQLiteDBPath"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["SQLiteDBPath"]);
            }
        }

        public static string MailTo
        {
            get
            {
                var val = ConfigurationManager.AppSettings["to"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["to"]);
            }
        }

        public static string MailCcTo
        {
            get
            {
                var val = ConfigurationManager.AppSettings["cc"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["cc"]);
            }
        }

        public static string MailBCcTo
        {
            get
            {
                var val = ConfigurationManager.AppSettings["bcc"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["bcc"]);
            }
        }

        public static string MailSubject
        {
            get
            {
                var val = ConfigurationManager.AppSettings["TurnoverWatcher.Subject"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["TurnoverWatcher.Subject"]);
            }
        }

        public static string MailTitle
        {
            get
            {
                var val = ConfigurationManager.AppSettings["TurnoverWatcher.Title"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["TurnoverWatcher.Title"]);
            }
        }

        public static string AdminEmail
        {
            get
            {
                var val = ConfigurationManager.AppSettings["AdminEmail"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["AdminEmail"]);
            }
        }

        public static bool IsEmailConfig
        {
            get
            {
                var val = ConfigurationManager.AppSettings["isMailConfig"];
                return val == null ? false : ConfigurationManager.AppSettings["isMailConfig"] == "1" ? true : false;
            }
        }

        public static int MailFunctionId
        {
            get
            {
                var val = ConfigurationManager.AppSettings["functionid"];
                return val == null ? 328 : Convert.ToInt32(ConfigurationManager.AppSettings["functionid"]);
            }
        }

        public static string ExportDataDirectory
        {
            get
            {
                return ExportDataDirectory("Fanex.TurnoverWatcherReport.ExportDataDirectory");
            }
        }
    }
}