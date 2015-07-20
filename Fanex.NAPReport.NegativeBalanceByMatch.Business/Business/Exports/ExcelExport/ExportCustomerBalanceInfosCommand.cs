using System.Collections.Generic;
using System.Drawing;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Extensions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Fanex.Service.Core.Service.Interface.Npoi;
using Fanex.Service.Core.Service.Implement.Npoi;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
	public class ExportCustomerBalanceInfosCommand : ExportCommandBase
	{
		private IList<CustomerBalanceInfo> _customerBalanceInfos;
        
        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _textCenterStyle;
        private XSSFCellStyle _textRightStyle;

		public ExportCustomerBalanceInfosCommand(ExcelDocument excelDocument, IList<CustomerBalanceInfo> customerBalanceInfos)
			: base(excelDocument)
		{
			_customerBalanceInfos = customerBalanceInfos;

            _fontService = new NpoiNewFormatFontService(excelDocument.Workbook);
            _regularFont = _fontService.RegularFont;
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(excelDocument.Workbook);
            _decimalStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);
            _textCenterStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _regularFont);
            _textRightStyle = _customCellStyleService.CreateRightTextStyle(Color.White, _regularFont);
		}

		public override void Execute()
		{
			RenderLegend();
			RenderCustomerBalanceInfosHeader();

			for (int index = 0; index < _customerBalanceInfos.Count; index++)
			{
				ExcelDocument.IncreaseRowIndex();
				ExcelDocument.ResetColumnIndex();

				RenderCustomerBalanceInfosRow(index, _customerBalanceInfos[index]);
			}
		}

		public void RenderLegend()
		{
			ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow,
				ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);
			ExcelDocument.RenderCurrentCell("List of affected members");
			ExcelDocument.IncreaseRowIndex();
		}

		public void RenderCustomerBalanceInfosHeader()
		{
            XSSFCellStyle style = _customCellStyleService.CreateCenterTextStyle(Color.FromArgb(60, 94, 165), _fontService.WhiteBoldFont);

			ExcelDocument.RenderCurrentCell("No.", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Members", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Balance", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Currency", style);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell("Deposit Name", style);
		}

		public void RenderCustomerBalanceInfosRow(int index, CustomerBalanceInfo customerBalanceInfo)
		{
			ExcelDocument.RenderCurrentCell(index + 1, _textCenterStyle);

			ExcelDocument.IncreaseColumnIndex();
			ExcelDocument.RenderCurrentCell(customerBalanceInfo.Username, _textRightStyle);

			ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.TotalBalance, _decimalStyle);

			ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.CurrencyName, _textRightStyle);

			ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.CustomInfo1, _textRightStyle);
		}
	}
}
