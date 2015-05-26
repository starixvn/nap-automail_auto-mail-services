// -----------------------------------------------------------------------
// <copyright file="AppSetting.cs" company="Nexcel">
// TODO: Mapping Application setting to class
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.RngCasino.Business.Configuration
{
    /// <summary>
    /// TODO: Mapping application setting.
    /// </summary>
    public static class AppSetting
    {
        public static int TimeInterval
        {
            get { return Fanex.Common.Converter.ToInt(System.Configuration.ConfigurationManager.AppSettings["TimeInterval"]); }
        }

        public static decimal WinLoss
        {
            get { return Fanex.Common.Converter.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["WinLoss"]); }
        }

        public static string BaseCurrency
        {
            get { return Fanex.Common.Converter.ToString2(System.Configuration.ConfigurationManager.AppSettings["BaseCurrency"]); }
        }

        public static bool Testing
        {
            get
            {
                return Fanex.Common.Converter.ToBool(System.Configuration.ConfigurationManager.AppSettings["Testing"]);
            }
        }
    }
}
