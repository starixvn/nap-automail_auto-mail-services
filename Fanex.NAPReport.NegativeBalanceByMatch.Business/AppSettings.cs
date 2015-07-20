namespace Fanex.NAPReport.NegativeBalanceByMatch.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Utilities;

	public class AppSettings : AppSettingBase, ISettings
	{
		public static bool IsActive { get; set; }

		public static bool IsTesting { get; set; }

        public static bool IsUpdateStatusSendingEmail { get; set; }

        public static string ExcelPasswordProtected { get; set; }

		public static string ExportDataDirectory { get; set; }

        public static string LicenseesDualLanguage { get; set; }

		public static string ExportDataDirectoryWithYearMonthDay
		{
			get { return ExportDataDirectoryWithYearMonthDay(ExportDataDirectory); } 
		}
	}
}
