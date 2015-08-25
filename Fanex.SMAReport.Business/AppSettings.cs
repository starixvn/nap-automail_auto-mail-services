// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

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

        public static string DataDirectoryWithYear
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