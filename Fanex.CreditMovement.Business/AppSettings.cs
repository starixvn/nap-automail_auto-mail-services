// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;
    using Fanex.CreditMovement.Business.Repositories;
    using System.Collections.Generic;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        private List<string> _listSite;

        public static string ExportDataDirectory
        {
            get
            {
                return GetSettingValueByKey("CreditMovement.ExportDataDirectory");
            }
        }

        public static string EmailTemplate
        {
            get
            {
                return GetSettingValueByKey("CreditMovement.EmailTemplate");
            }
        }

        public static string MailSubject
        {
            get
            {
                return GetSettingValueByKey("CreditMovement.Subject");
            }
        }

        public static string MailTitle
        {
            get
            {
                return GetSettingValueByKey("CreditMovement.Title");
            }
        }

        public static string ExcelPassword
        {
            get
            {
                return GetSettingValueByKey("CreditMovement.ExcelPassword");
            }
        }

        public static int Amount
        {
            get
            {
                var val = ConfigurationManager.AppSettings["CreditMovement.Amount"];
                return val == null ? 300000 : Convert.ToInt32(ConfigurationManager.AppSettings["CreditMovement.Amount"]);
            }
        }

        public static DateTime FromDate
        {
            get
            {
                var val = ConfigurationManager.AppSettings["CreditMovement.FromDate"];
                return string.IsNullOrEmpty(val) ? DateTime.Now.AddDays(-1).Date : Convert.ToDateTime(ConfigurationManager.AppSettings["CreditMovement.FromDate"]);
            }
        }

        public static DateTime ToDate
        {
            get
            {
                var val = ConfigurationManager.AppSettings["CreditMovement.ToDate"];
                return string.IsNullOrEmpty(val) ? DateTime.Now.Date.AddSeconds(-1) : Convert.ToDateTime(ConfigurationManager.AppSettings["CreditMovement.ToDate"]);
            }
        }

        public static List<string> ListSite
        {
            get
            {
                var repository = new CreditMovementRepository();
                return repository.GetSiteId();
            }
        }
    }
}