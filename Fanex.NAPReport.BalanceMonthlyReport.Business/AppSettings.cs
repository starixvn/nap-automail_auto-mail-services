// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectoryCreditTerm
        {
            get;
            set;
        }

        public static string DataDirectorySuperBalance
        {
            get;
            set;
        }

        public static string DataDirectoryDirectBalance
        {
            get;
            set;
        }

        public static string DataDirectoryWithYearCreditTerm
        {
            get { return ExportDataDirectoryWithYear(DataDirectoryCreditTerm); }
        }

        public static string DataDirectoryWithYearSuperBalance
        {
            get { return ExportDataDirectoryWithYear(DataDirectorySuperBalance); }
        }

        public static string DataDirectoryWithYearDirectBalance
        {
            get { return ExportDataDirectoryWithYear(DataDirectoryDirectBalance); }
        }

        public static string ExcelPasswordProtectedCreditTerm
        {
            get;
            set;
        }

        public static string ExcelPasswordProtectedSuperBalance
        {
            get;
            set;
        }

        public static string ExcelPasswordProtectedDirectBalance
        {
            get;
            set;
        }

        public static bool IsTest
        {
            get;
            set;
        }       

        public static int FunctionIdCreditTerm
        {
            get;
            set;
        }

        public static int FunctionIdSuperBalance
        {
            get;
            set;
        }

        public static int FunctionIdDirectBalance
        {
            get;
            set;
        }
    }
}
