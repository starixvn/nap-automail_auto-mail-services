// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business
{
    using System;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectory
        {
            get;
            set;
        }

        public static string DataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(DataDirectory); }
        }

        public static string EmailTemplate
        {
            get;
            set;
        }

        public static string MailSubject
        {
            get;
            set;
        }

        public static string MailTitle
        {
            get;
            set;
        }

        public static string ExcelPasswordProtected
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

        public static bool IsTest
        {
            get;
            set;
        }
    }
}