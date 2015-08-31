// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business
{
    using System;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
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

        public static bool IsCheckEnoughData
        {
            get;
            set;
        }

        public static bool IsTesting
        {
            get;
            set;
        }

        public static DateTime WinLossDate
        {
            get;
            set;
        }

        public static string SubjectOverall
        {
            get;
            set;
        }

        public static string SubjectProductWL
        {
            get;
            set;
        }
    }
}
