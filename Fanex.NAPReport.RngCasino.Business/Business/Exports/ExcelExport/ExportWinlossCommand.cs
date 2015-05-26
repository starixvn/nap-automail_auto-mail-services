using System.Collections.Generic;
using System.Drawing;
using Fanex.NAPReport.RngCasino.Business.Configuration;
using Fanex.NAPReport.RngCasino.Business.Entities;
using Fanex.NAPReport.RngCasino.Business.Helper.Implement;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport
{
	public class ExportWinLossCommand : ExportCommandBase
	{
		private readonly IList<UserInfo> _userInfos;

		public ExportWinLossCommand(ExcelDocument excelDocument, IList<UserInfo> userInfos)
			: base(excelDocument)
		{
			_userInfos = userInfos;
		}

		public override void Execute()
		{
			RenderWinLossHeader();
			RenderWinLossBody();
		}

		private void RenderWinLossHeader()
		{
			var style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, Color.FromArgb(0, 0, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(0, 0, 0));

			ExcelDocument.RenderCurrentCell("Index", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Member", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Agency", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(string.Format("Winloss Amount in {0}", RngCasinoSettings.BaseCurrency), style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Winloss Amount in user currency (UserCurrency)", style);
		}

		private void RenderWinLossBody()
		{
			for (int index = 0; index < _userInfos.Count; index++)
			{
				var userInfo = _userInfos[index];
				ExcelDocument.IncreaseRowIndex();
				ExcelDocument.ResetColumnIndex();

				RenderWinLossRow(index, userInfo);
			}
		}

		private void RenderWinLossRow(int index, UserInfo userInfo)
		{
			var styleCenter = ExcelDocument.GetCellStyle(FontBoldWeight.None, borderColor: Color.FromArgb(0, 0, 0));

			var styleRight = ExcelDocument.GetCellStyle(FontBoldWeight.None, borderColor: Color.FromArgb(0, 0, 0), horizontalAlignment: HorizontalAlignment.Right);

			ExcelDocument.RenderCurrentCell(index + 1, styleCenter);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(userInfo.Member, styleCenter);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(userInfo.Agency, styleCenter);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(NumberExtension.WinLossFormat(userInfo.WinlossBaseCurrency, 2), styleRight);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(string.Format("{0} ({1})", NumberExtension.WinLossFormat(userInfo.Winloss, 2), userInfo.CurrencyName), styleRight);
		}
	}
}
