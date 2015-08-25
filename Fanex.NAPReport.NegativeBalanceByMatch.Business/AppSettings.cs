namespace Fanex.NAPReport.NegativeBalanceByMatch.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    public class AppSettings : AppSettingBase, ISettings
    {
        public static bool IsActive { get; set; }

        public static bool IsTesting { get; set; }

        public static bool IsUpdateStatusSendingEmail { get; set; }

        public static string ExcelPasswordProtected { get; set; }

        public static new string ExportDataDirectory { get; set; }

        public static string LicenseesDualLanguage { get; set; }

        public static new string ExportDataDirectoryWithYearMonthDay
        {
            get { return ExportDataDirectoryWithYearMonthDay(ExportDataDirectory); }
        }
    }
}