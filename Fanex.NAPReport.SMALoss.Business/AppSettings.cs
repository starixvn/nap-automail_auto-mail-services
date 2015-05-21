namespace Fanex.NAPReport.SMALoss.Business
{
    using System.Configuration;
    using Fanex.Common;
    using Fanex.Service.Core.Utilities;

    public class AppSettings : AppSettingBase
    {
        private static int sendEmailHour = -1;
        private static int sendEmailMinute = -1;
        private static string title;
        private static string subject;
        private static string exportDataDirectory;

        public static int SendEmailHour
        {
            get
            {
                if (sendEmailHour == -1)
                {
                    sendEmailHour = Converter.ToInt(GetValue("SendEmailHour"));
                }

                return sendEmailHour;
            }
        }

        public static int SendEmailMinute
        {
            get
            {
                if (sendEmailMinute == -1)
                {
                    sendEmailMinute = Converter.ToInt(GetValue("SendEmailMinute"));
                }

                return sendEmailMinute;
            }
        }

        public static string Title
        {
            get
            {
                if (title == null)
                {
                    title = GetValue("SMA.Title");
                }

                return title;
            }
        }

        public static string ExportDataDirectory
        {
            get
            {
                if (exportDataDirectory == null)
                {
                    exportDataDirectory = GetValue("SMA.ExportDataDirectory");
                }

                return exportDataDirectory;
            }
        }

        public static string Subject
        {
            get
            {
                if (subject == null)
                {
                    subject = GetValue("SMA.Subject");
                }

                return subject;
            }
        }

        /// <summary>
        /// Gets the value from web configuration.
        /// </summary>
        /// <param name="key">The key which is used to identify the value.</param>
        /// <returns>Associated value of passed key.</returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}