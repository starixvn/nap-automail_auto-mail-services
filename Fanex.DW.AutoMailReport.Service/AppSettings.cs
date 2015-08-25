// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.AutoMailReport.Service
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AppSettings
    {
        public static string ServiceName
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServiceName"];
                return val == null ? "_Fanex.SMAReport.Service" : val;
            }
        }
    }
}