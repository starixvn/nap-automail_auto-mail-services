// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business
{
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// Application setting.
    /// </summary>
    public class AppSettings : AppSettingBase
    {
        public static string ExportDataDirectory
        {
            get
            {
                return ExportDataDirectory("Fanex.WinLossByProductsReport.ExportDataDirectory");
            }
        }

        public static string Subject
        {
            get
            {
                return GetSettingValueByKey("WinLossByProductsReport.Subject");
            }
        }

        public static string Title
        {
            get
            {
                return GetSettingValueByKey("WinLossByProductsReport.Title");
            }
        }
    }
}
