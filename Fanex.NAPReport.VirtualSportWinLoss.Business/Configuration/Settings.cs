namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Configuration
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    public class Settings : AppSettingBase, ISettings
    {
        public static string Subject { get; set; }

        public static string Title { get; set; }

        public static string ExportDataDirectory { get; set; }
    }
}