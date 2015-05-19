namespace Fanex.HorseTransaction3rdServices
{
    using System;
    using System.Configuration;

    public class AppSetting
    {
        public static bool IsLocal
        {
            get { return ConfigurationManager.AppSettings["IsLocal"] == "1"; }
        }

        public static bool FtpLocal
        {
            get { return ConfigurationManager.AppSettings["ftplocal"] == "1"; }
        }

        public static int RangeDate
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["RangeDate"], 10); }
        }

        public static bool TestFlag
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["TestFlag"]); }
        }

        public static DateTime TestDate
        {
            get { return Convert.ToDateTime(ConfigurationManager.AppSettings["TestDate"]); }
        }

        public static string TemplateOfSuccessEmailDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Fanex.HorseTransaction3rd.SuccessEmailDirectory"];
            }
        }

        public static string TemplateOfUnsuccessEmailDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Fanex.HorseTransaction3rd.UnsuccessEmailDirectory"];
            }
        }
    }
}
