// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        public static string ExportDataDirectoryDaily
        {
            get
            {
                return ExportDataDirectory("Fanex.DW.DailyBettypeWith3rdProducts.ExportDataDirectory.Daily");
            }
        }

        public static string ExportDataDirectoryWeekly
        {
            get
            {
                return ExportDataDirectory("Fanex.DW.DailyBettypeWith3rdProducts.ExportDataDirectory.Weekly");
            }
        }

        public static bool IsManually
        {
            get
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.IsManually") == "1";
            }
        }

        public static string DateStart
        {
            get 
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.DateStart");
            }
        }

        public static string DateEnd
        {
            get
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.DateEnd");
            }
        }

        public static bool IsSingleSport
        {
            get
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.IsSingleSport") == "1";
            }
        }

        public static string SubjectDaily
        {
            get
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.Subject.Daily");
            }
        }

        public static string SubjectWeekly
        {
            get
            {
                return GetSettingValueByKey("Fanex.DW.DailyBettypeWith3rdProducts.Subject.Weekly");
            }
        }
    }
}
