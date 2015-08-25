// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business
{
    using System;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string ExportDataDirectoryDaily
        {
            get;
            set;
        }

        public static string DirectoryForDaily
        {
            get 
            {
                return ExportDataDirectoryWithYearMonth(ExportDataDirectoryDaily);
            }
        }

        public static string ExportDataDirectoryWeekly
        {
            get;
            set;
        }

        public static string DirectoryForWeekly
        {
            get
            {
                return ExportDataDirectoryWithYearMonth(ExportDataDirectoryWeekly);
            }
        }

        public static bool IsManually
        {
            get;
            set;
        }

        public static bool IsSingleSport
        {
            get;
            set;
        }

        public static bool IsCheckEnoughData
        {
            get;
            set;
        }

        public static DateTime DateStart
        {
            get;
            set;
        }

        public static DateTime DateEnd
        {
            get;
            set;
        }

        public static int FunctionIdDaily
        {
            get;
            set;
        }

        public static int FunctionIdWeekly
        {
            get;
            set;
        }  
    }
}
