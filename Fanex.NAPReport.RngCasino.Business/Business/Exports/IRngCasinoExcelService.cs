using System.Collections.Generic;
using Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.RngCasino.Business.Entities;

namespace Fanex.NAPReport.RngCasino.Business.Business.Exports
{
	public interface IRngCasinoExcelService
	{
		void GenerateReport(IList<UserInfo> matchs, RngCasinoExportSettings rngCasinoExportSettings, string saveFileNamePath);
	}
}
