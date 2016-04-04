using Fanex.Service.Core.Configuration;
using Fanex.Service.Core.Utilities;
using System;

namespace Fanex.NAPReport.TraderPerformance.Business
{
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string ExportDirectory
        {
            get;
            set;
        }

        public static new string ExcelPassword
        {
            get;
            set;
        }

        public static bool IsTest
        {
            get;
            set;
        }

        public static DateTime FromDate
        {
            get;
            set;
        }

        public static DateTime ToDate
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

        public static string DataDirectory(string path)
        {
            return ExportDataDirectoryWithYearMonth(path);
        }
    }
}
