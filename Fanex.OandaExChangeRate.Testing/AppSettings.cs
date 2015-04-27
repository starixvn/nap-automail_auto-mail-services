// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.OandaExChangeRate.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings: AppSettingBase
    {
        public static string ExportDataDirectory
        {
            get
            {
                return ExportDataDirectory("Fanex.OandaExchangeRate.ExportDataDirectory");
            }
        }

        public static string Subject
        {
            get
            {
                return GetSettingValueByKey("OandaExchangeRate.Subject");
            }
        }

        public static string Title
        {
            get
            {
                return GetSettingValueByKey("OandaExchangeRate.Title");
            }
        }

        public static string[] BaseCurrencies
        {
            get
            {
                return GetSettingValueByKey("OandaExchangeRate.BaseCurrencies").Split(',');
            }
        }
    }
}
