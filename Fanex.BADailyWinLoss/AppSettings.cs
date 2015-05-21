// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.BADailyWinLoss.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        public static string ExportDataDirectory
        {
            get
            {
                return ExportDataDirectory("Fanex.BADailyWinLoss.ExportDataDirectory");
            }
        }

        public static string TemplateDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Fanex.BADailyWinLoss.TemplateDirectory"];
            }
        }

        public static bool IsTesting
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["Fanex.BADailyWinLoss.IsTesting"]);
            }
        }

        public static DateTime TestedDate
        {
            get
            {
                return Convert.ToDateTime(ConfigurationManager.AppSettings["Fanex.BADailyWinLoss.TestedDate"]);
            }
        }
    }
}
