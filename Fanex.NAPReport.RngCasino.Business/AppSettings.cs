namespace Fanex.NAPReport.RngCasino.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    public class AppSettings : AppSettingBase, ISettings
    {
        public static bool IsActive { get; set; }

        public static string BaseCurrency { get; set; }

        public static decimal WinLoss { get; set; }

        public static string ExcelPasswordProtected { get; set; }

        public static string DataDirectory { get; set; }

        public static int FunctionIdVipGroup { get; set; }

        public static int FunctionIdNormalGroup { get; set; }

        public static new string ExportDataDirectoryWithYearMonthDay
        {
            get { return ExportDataDirectoryWithYearMonthDay(DataDirectory); }
        }

        public static string NormalSubject
        {
            get;
            set;
        }

        public static string EmptySubject
        {
            get;
            set;
        }
    }
}