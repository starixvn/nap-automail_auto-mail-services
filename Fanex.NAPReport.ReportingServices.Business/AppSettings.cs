// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business
{
    using System.Collections.Generic;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;
using System;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string ExportDataDirectoryCompanyBalance
        {
            get;
            set;
        }

        public static string ExportDataDirectoryCompanyStatement
        {
            get;
            set;
        }

        public static string ExportDataDirectoryDailyTurnover
        {
            get;
            set;
        }

        public static string ExportDataDirectoryMatchWinLoss2a
        {
            get;
            set;
        }

        public static string ExportDataDirectorySuperYesterdayBalance
        {
            get;
            set;
        }

        public static string DataDirectory(string path)
        {
            return ExportDataDirectoryWithYearMonth(path);
        }

        public static string ExcelPassword
        {
            get;
            set;
        }

        public static bool IsTest
        {
            get;
            set;
        }

        public static DateTime WinLossDate
        {
            get;
            set;
        }
    }
}