// -----------------------------------------------------------------------
// <copyright file="EmailSetting.cs" company="Nexcel">
// TODO: Steven.Nguyen
// </copyright>
// -----------------------------------------------------------------------

namespace Robot.Report.RngCasino.Configuration
{
    /// <summary>
    /// TODO: Email Setting for.
    /// </summary>
    public static class EmailSetting
    {
        public static string Subject
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Subject"]; }
        }

        public static string Title
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Title"]; }
        }

        public static string To 
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["To"]; }
        }

        public static string Cc
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Cc"]; }
        }

        public static string Bcc
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Bcc"]; }
        }

        public static string From   
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["From"]; }
        }

        public static string Password
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Password"]; }
        }

        public static string MailHost
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["MailHost"]; }
        }

        public static int MailPort
        {
            get { return Fanex.Common.Converter.ToInt(System.Configuration.ConfigurationManager.AppSettings["MailPort"]); }
        }

        public static bool UseOutsideMail
        {
            get { return Fanex.Common.Converter.ToBool(System.Configuration.ConfigurationManager.AppSettings["UseOutsideMail"]); }
        }
    }
}
