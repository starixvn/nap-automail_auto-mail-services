// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;
    using Fanex.Service.Core.Configuration;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectory
        {
            get;
            set;
        }

        public static new string DataDirectoryWithYear
        {
            get { return ExportDataDirectoryWithYear(DataDirectory); }
        }

        public static string ExcelPasswordProtected
        {
            get;
            set;
        }

        public static int FunctionId
        {
            get;
            set;
        }

        public static string DownloadUrl
        {
            get;
            set;
        }       
    }
}