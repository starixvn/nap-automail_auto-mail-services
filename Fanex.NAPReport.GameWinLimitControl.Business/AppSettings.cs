// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static int FunctionId
        {
            get;
            set;
        }

        public static new string Subject { get; set; }

        public static new string Title { get; set; }

        public static string DataDirectory
        {
            get;
            set;
        }

        public static new string ExportDataDirectoryWithYearMonthDay
        {
            get { return ExportDataDirectoryWithYearMonthDay(DataDirectory); }
        }

        public static string ExcelPasswordProtected
        {
            get;
            set;
        }

        public static int IntervalInSecond
        {
            get;
            set;
        }

        public static bool IsActive
        {
            get;
            set;
        }

        public static bool IsSimulated
        {
            get;
            set;
        }

        public static bool IsCallFPSBlockMember
        {
            get;
            set;
        }
    }
}
