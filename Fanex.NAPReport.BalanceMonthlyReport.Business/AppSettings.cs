// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.BalanceReport
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
                return ExportDataDirectory("Balance.ExportDataDirectory");
            }
        }

        public static string EmailTemplageFolder
        {
            get
            {
                return GetSettingValueByKey("Balance.EmailTemplage");
            }
        }
    }
}
