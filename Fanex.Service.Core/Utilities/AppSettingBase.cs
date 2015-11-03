// -----------------------------------------------------------------------
// <copyright file="AppSettingBase.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Utilities
{
    using System;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettingBase
    {
        public static string IP
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerFile.IP"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["ServerFile.IP"]);
            }
        }

        public static string UserName
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerFile.UserName"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["ServerFile.UserName"]);
            }
        }

        public static string Password
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerFile.Password"];
                return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["ServerFile.Password"]);
            }
        }

        public static bool IsEncryptPassword
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerFile.IsEncryptPassword"];
                return val == null ? false : Convert.ToString(ConfigurationManager.AppSettings["ServerFile.IsEncryptPassword"]) == "true";
            }
        }

        public static bool IsAuthenticate
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerFile.IsAuthenticate"];
                return val == null ? false : Convert.ToString(ConfigurationManager.AppSettings["ServerFile.IsAuthenticate"]) == "true";
            }
        }

        public static string ExcelPassword
        {
            get
            {
                return GetSettingValueByKey("ExcelPassword");
            }
        }

        public static string ServiceName
        {
            get
            {
                return GetSettingValueByKey("ServiceName");
            }
        }

        public static int Interval
        {
            get
            {
                var val = ConfigurationManager.AppSettings["Interval.Minute"];
                int interval = val == null ? 1000 * 3600 : Convert.ToInt32(val) * 60 * 1000;
                return interval;
            }
        }

        public static bool OnProcessing { get; set; }

        public static string NapReportURL
        {
            get
            {
                return GetSettingValueByKey("NapReportURL");
            }
        }

        public static string Title
        {
            get
            {
                return GetSettingValueByKey("Title");
            }
        }

        public static string Subject
        {
            get
            {
                return GetSettingValueByKey("Subject");
            }
        }

        public static int NotificationEmailFunctionId
        {
            get
            {
                var val = ConfigurationManager.AppSettings["NotificationEmailFunctionId"];
                return val == null ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["NotificationEmailFunctionId"]);
            }
        }

        public static string WindowsServiceCategory
        {
            get { return GetSettingValueByKey("WindowsService.Category"); }
        }

        protected static string ExportDataDirectory(string key)
        {
            string path = AppSettingBase.GetSettingValueByKey(key);

            return ExportDataDirectoryWithYearMonth(path);
        }

        protected static string ExportDataDirectoryWithYearMonthDay(string exportDataDirectory)
        {
            string month = DateTime.Today.ToString("MM");
            string day = DateTime.Today.ToString("dd");
            exportDataDirectory += @"\" + DateTime.Today.Year + @"\" + month + @"\" + day;

            return exportDataDirectory;
        }

        protected static string ExportDataDirectoryWithYearMonth(string exportDataDirectory)
        {
            string month = DateTime.Today.ToString("MM");
            exportDataDirectory += @"\" + DateTime.Today.Year + @"\" + month;

            return exportDataDirectory;
        }

        protected static string ExportDataDirectoryWithYear(string exportDataDirectory)
        {
            exportDataDirectory += @"\" + DateTime.Today.Year;

            return exportDataDirectory;
        }

        protected static string GetSettingValueByKey(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings[key]);
        }
    }
}