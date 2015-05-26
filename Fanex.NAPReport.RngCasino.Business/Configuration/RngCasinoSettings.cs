using Fanex.Service.Core.Configuration;
using Fanex.Service.Core.Utilities;

namespace Fanex.NAPReport.RngCasino.Business.Configuration
{
	public class RngCasinoSettings : AppSettingBase, ISettings
	{
		public static bool IsActive { get; set; }


		public static string Subject { get; set; }

		public static string Title { get; set; }


		public static int TimeInterval { get; set; }

		public static string BaseCurrency { get; set; }

		public static decimal WinLoss { get; set; }


		public static bool AttachReport { get; set; }

		public new static string ExcelPassword { get; set; }

		public static string ExcelFileName { get; set; }

		public static string ExcelSheetName { get; set; }

		public static string ExportDataDirectory { get; set; }


		public static int FunctionId { get; set; }


		public static string ExportDataDirectoryWithYearMonth
		{
			get { return ExportDataDirectoryWithYearMonth(ExportDataDirectory); }
		}
	}
}
