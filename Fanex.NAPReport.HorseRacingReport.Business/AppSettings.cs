// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business
{
    using System;
    using System.Configuration;
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
                return ExportDataDirectory("Fanex.HorseRacingReport.ExportDataDirectory");
            }
        }

        public static string Subject
        {
            get
            {
                return GetSettingValueByKey("HorseRacingReport.Subject");
            }
        }

        public static string Title
        {
            get
            {
                return GetSettingValueByKey("HorseRacingReport.Title");
            }
        }

        public static string [] BaseCurrencies
        {
            get
            {
                return GetSettingValueByKey("OandaExchangeRate.BaseCurrencies").Split(',');
            }
        }
    }
}