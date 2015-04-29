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
        public static string ExportDataDirectory
        {
            get
            {
                return ExportDataDirectory("Fanex.DW.DailyBettypeWith3rdProducts.ExportDataDirectory");
            }
        }
    }
}
