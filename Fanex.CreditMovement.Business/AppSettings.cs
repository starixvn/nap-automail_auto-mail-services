// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Fanex.CreditMovement.Business.Repositories;
    using Fanex.Service.Core.Utilities;
    using Fanex.Service.Core.Configuration;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        private List<string> _listSite;

        public static string ExportDataDirectory
        {
            get;
            set;
        }

        public static string ExportDataDirectoryWithYearMonth
        {
            get { return ExportDataDirectoryWithYearMonth(ExportDataDirectory); }
        }

        public static string EmailTemplate
        {
            get;
            set;
        }

        public static string ExcelPassword
        {
            get;
            set;
        }

        public static int Amount
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

        public static List<string> ListSite
        {
            get
            {
                var repository = new CreditMovementRepository();
                return repository.GetSiteId();
            }
        }

        public static int FunctionId
        {
            get;
            set;
        }
    }
}