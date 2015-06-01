using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Service.Core.Configuration;
using Fanex.Service.Core.Utilities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration
{
	public class NegativeBalanceByMatchSettings : AppSettingBase, ISettings
	{
		public static bool IsActive { get; set; }

		public static bool IsTesting { get; set; }

		public static string Subject { get; set; }

		public static string Title { get; set; }

		public static string SqLiteDbPath { get; set; }

		public static bool AttachReport { get; set; }

		public static string ExcelPassword { get; set; }

		public static string ExportDataDirectory { get; set; }

		public static string ExportDataDirectoryWithYearMonth
		{
			get { return ExportDataDirectoryWithYearMonth(ExportDataDirectory); } 
		}
	}
}
