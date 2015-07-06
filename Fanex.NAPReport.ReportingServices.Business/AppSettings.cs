// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string ExportDataDirectory
        {
            get;
            set;
        }

        public static string ExportDataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(ExportDataDirectory); }
        }

        public static string ExcelPassword
        {
            get;
            set;
        }
    }
}