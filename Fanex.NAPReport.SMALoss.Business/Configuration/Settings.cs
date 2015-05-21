namespace Fanex.NAPReport.SMALoss.Business.Configuration
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    public class Settings : AppSettingBase, ISettings
    {
        public static bool ActiveService { get; set; }

        public static string Subject { get; set; }

        public static string Title { get; set; }

        public static string ExportDataDirectory { get; set; }

        public static string ExportDataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(ExportDataDirectory); }
        }
    }
}