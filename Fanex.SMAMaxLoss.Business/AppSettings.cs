namespace Fanex.NAPReport.SMALoss.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectory { get; set; }

        public static new string ExportDataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(DataDirectory); }
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

        public static new string Subject
        {
            get;
            set;
        }
    }
}