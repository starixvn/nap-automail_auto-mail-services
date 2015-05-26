using System.Collections.Generic;
using System.Drawing;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
	public class ExportMatchSettleLogsCommand : ExportCommandBase
	{
		private IList<MatchSettleLog> _matchSettleLogs;

		public ExportMatchSettleLogsCommand(ExcelDocument excelDocument, IList<MatchSettleLog> matchSettleLogs)
			: base(excelDocument)
		{
			_matchSettleLogs = matchSettleLogs;
		}

		public override void Execute()
		{
			RenderMatchSettleLogsHeader();

			for (int index = 0; index < _matchSettleLogs.Count; index++)
			{
				ExcelDocument.IncreaseRowIndex();
				ExcelDocument.ResetColumnIndex();

				RenderMatchSettleLogsRow(index, _matchSettleLogs[index]);
			}
		}

		public void RenderMatchSettleLogsHeader()
		{
			XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, Color.FromArgb(122, 151, 209), Color.FromArgb(0, 0, 0));

			ExcelDocument.RenderCurrentCell("#", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Settlement time", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Action", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("HT Score", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("FT Score", style);
		}

		public void RenderMatchSettleLogsRow(int index, MatchSettleLog matchSettleLog)
		{
			XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.None, Color.FromArgb(255, 255, 255), Color.FromArgb(0, 0, 0));

			ExcelDocument.RenderCurrentCell(index + 1, style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(matchSettleLog.SettlementTime, style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(matchSettleLog.Action, style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(matchSettleLog.HTHomeScore + " - " + matchSettleLog.HTAwayScore, style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(matchSettleLog.FinalHomeScore + " - " + matchSettleLog.FinalAwayScore, style);
		}
	}
}
