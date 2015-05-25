// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        public static string EmailTemplate
        {
            get
            {
                return GetSettingValueByKey("SMAReport.EmailTemplate");
            }
        }

        public static string ExportDataDirectory
        {
            get
            {
                return GetSettingValueByKey("SMAReport.ExportDataDirectory");
            }
        }

        public static string MailSubject
        {
            get
            {
                return GetSettingValueByKey("SMAReport.Subject");
            }
        }

        public static string MailTitle
        {
            get
            {
                return GetSettingValueByKey("SMAReport.Title");
            }
        }

        public static string DownloadUrl
        {
            get
            {
                return GetSettingValueByKey("SMAReport.DownloadUrl");
            }
        }

        public static string ExcelPassword
        {
            get
            {
                return GetSettingValueByKey("SMAReport.ExcelPassword");
            }
        }
    }
}