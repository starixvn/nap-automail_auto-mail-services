using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Entities;
using Fanex.NegativeBalanceByMatch.Business.Helper.Implement;
using Fanex.Service.Core.Extensions;
using Fanex.Service.Core.Model;
using Fanex.Service.Core.Service.Implement.Npoi;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Format;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
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

			foreach (MatchSettleLog matchSettleLog in _matchSettleLogs)
			{
				ExcelDocument.IncreaseRowIndex();
				ExcelDocument.ResetColumnIndex();

				RenderMatchSettleLogsRow(matchSettleLog);
			}
		}

		public void RenderMatchSettleLogsHeader()
		{
			XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, Color.FromArgb(60, 94, 165), Color.FromArgb(0, 0, 0));

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

		public void RenderMatchSettleLogsRow(MatchSettleLog matchSettleLog)
		{
			XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.None, Color.FromArgb(255, 255, 255), Color.FromArgb(0, 0, 0));

			ExcelDocument.RenderCurrentCell(matchSettleLog.LogID, style);

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
