namespace Fanex.NAPReport.HorseTransaction3rd.Business
{
    using System;
    using System.Configuration;

    public class AppSetting
    {
        public static bool IsManual
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.IsManual"]); }
        }

        public static DateTime FromDate
        {
            get { return Convert.ToDateTime(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.FromDate"]); }
        }

        public static DateTime ToDate
        {
            get { return Convert.ToDateTime(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.ToDate"]); }
        }

        public static string TemplateOfSuccessEmailDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.SuccessEmailDirectory"];
            }
        }

        public static string TemplateOfUnsuccessEmailDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.UnsuccessEmailDirectory"];
            }
        }
    }
}
