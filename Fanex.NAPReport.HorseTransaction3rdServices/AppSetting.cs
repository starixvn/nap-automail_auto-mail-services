namespace Fanex.NAPReport.HorseTransaction3rd.Business
{
    using System;
    using System.Configuration;

    public class AppSetting
    {
        public static bool IsLocal
        {
            get { return ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.IsLocal"] == "1"; }
        }

        public static bool FtpLocal
        {
            get { return ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.ftplocal"] == "1"; }
        }

        public static int RangeDate
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.RangeDate"], 10); }
        }

        public static bool TestFlag
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.TestFlag"]); }
        }

        public static DateTime TestDate
        {
            get { return Convert.ToDateTime(ConfigurationManager.AppSettings["Fanex.NAPReport.HorseTransaction3rd.TestDate"]); }
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
