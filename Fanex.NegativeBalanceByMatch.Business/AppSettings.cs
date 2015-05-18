// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Service.Core.Utilities;

namespace Fanex.NegativeBalanceByMatch.Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Configuration;

	/// <summary>
	/// Application setting.
	/// </summary>
	public class AppSettings: AppSettingBase
	{
		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		/// <value>
		/// The name of the service.
		/// </value>
		public static string ServiceName
		{
			get
			{
				var val = ConfigurationManager.AppSettings["ServiceName"];
				return val == null ? "Fanex.MonthlyWinlossByLeague.Service" : val;
			}
		}

		/// <summary>
		/// Gets the interval in second.
		/// </summary>
		/// <value>
		/// The interval.
		/// </value>
		public static int Interval
		{
			get
			{
				var val = ConfigurationManager.AppSettings["Interval.Second"];
				int interval = val == null ? 10 : Convert.ToInt32(val) * 1000;
				return interval;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [on processing].
		/// </summary>
		/// <value>
		///   <c>true</c> if [on processing]; otherwise, <c>false</c>.
		/// </value>
		public static bool OnProcessing { get; set; }

		public static bool ForceRunning
		{
			get
			{
				var val = ConfigurationManager.AppSettings["ForceRunning"];
				return val == null ? false : (Convert.ToInt32(ConfigurationManager.AppSettings["ForceRunning"]) == 1);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [force running on processing].
		/// </summary>
		/// <value>
		/// <c>true</c> if [force running on processing]; otherwise, <c>false</c>.
		/// </value>
		public static bool ForceRunningOnProcessing { get; set; }

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <value>
		/// The connection string.
		/// </value>
		public static string ConnectionString
		{
			get
			{
				var val = ConfigurationManager.AppSettings["SQLiteDBPath"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["SQLiteDBPath"]);
			}
		}

		public static string MailTo
		{
			get
			{
				var val = ConfigurationManager.AppSettings["to"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["to"]);
			}
		}

		public static string MailCC
		{
			get
			{
				var val = ConfigurationManager.AppSettings["cc"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["cc"]);
			}
		}

		public static string MailBCC
		{
			get
			{
				var val = ConfigurationManager.AppSettings["bcc"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["bcc"]);
			}
		}

		public static string MailSubject
		{
			get
			{
				var val = ConfigurationManager.AppSettings["Subject"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["Subject"]);
			}
		}

		public static string MailTitle
		{
			get
			{
				var val = ConfigurationManager.AppSettings["Title"];
				return val == null ? string.Empty : Convert.ToString(ConfigurationManager.AppSettings["Title"]);
			}
		}

		/// <summary>
		/// true: get real currency and email from db, false: get uus currency and get email config file.
		/// </summary>
		public static bool IsTesting
		{
			get
			{
				var val = ConfigurationManager.AppSettings["isTesting"];
				return val == null ? false : (Convert.ToString(ConfigurationManager.AppSettings["isTesting"]) == "1");
			}
		}

		public static string Licensees
		{
			get
			{
				var val = ConfigurationManager.AppSettings["licensee"];
				return val == null ? "HAIFA" : Convert.ToString(ConfigurationManager.AppSettings["licensee"]);
			}
		}

		public static bool IsSendEmailFromConfigFileWithRealCurrency
		{
			get
			{
				var val = ConfigurationManager.AppSettings["IsSendEmailFromConfigFileWithRealCurrency"];
				return val == null ? false : (Convert.ToString(ConfigurationManager.AppSettings["IsSendEmailFromConfigFileWithRealCurrency"]) == "1");
			}
		}

		public static string LicenseeFunctionMap
		{
			get
			{

				return ConfigurationManager.AppSettings["LicenseeFunctionMap"];
			}
		}

		public static string ExportDataDirectory
		{
			get
			{
				return ExportDataDirectory("ExportDataDirectory");
			}
		}

	}
}
