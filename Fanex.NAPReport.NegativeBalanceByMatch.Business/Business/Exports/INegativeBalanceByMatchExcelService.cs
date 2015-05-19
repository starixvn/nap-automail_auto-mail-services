using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAPReport.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.Business.Entities;

namespace Fanex.NAPReport.Business.Business.Exports
{
	public interface INegativeBalanceByMatchExcelService
	{
		void GenerateReportForLicensee(string licenseeName, IList<Match> matchs, ExportSettings exportSettings, string saveFileNamePath);
	}
}
