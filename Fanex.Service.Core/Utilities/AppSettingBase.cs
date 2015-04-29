// -----------------------------------------------------------------------
// <copyright file="AppSettingBase.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

        protected static string GetSettingValueByKey(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings[key]);
        }

        public static string ExcelPassword
        {
            get 
            {
                return GetSettingValueByKey("ExcelPassword");
            }
        }

        protected static string ExportDataDirectory(string key)
        {
            string path = AppSettingBase.GetSettingValueByKey(key); 
            string month = DateTime.Today.Month < 10
                ? "0" + DateTime.Today.Month
                : DateTime.Today.Month + string.Empty;

            path += @"\" + DateTime.Today.Year + @"\" + month;

            return path;
        }

        public static string ServiceName
        {
            get 
            {
                return GetSettingValueByKey("ServiceName");
            }
        }
    }
}
