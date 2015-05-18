using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    public class ExportCustomerBalanceInfosCommand : ExportCommandBase
    {
        private IList<CustomerBalanceInfo> _customerBalanceInfos;

        public ExportCustomerBalanceInfosCommand(ExcelDocument excelDocument, IList<CustomerBalanceInfo> customerBalanceInfos)
            : base(excelDocument)
        {
            _customerBalanceInfos = customerBalanceInfos;
        }

        public override void Execute()
        {
            RenderLegend();
            RenderCustomerBalanceInfosHeader();

            foreach (CustomerBalanceInfo customerBalanceInfo in _customerBalanceInfos)
            {
                ExcelDocument.IncreaseRowIndex();
                ExcelDocument.ResetColumnIndex();

                RenderCustomerBalanceInfosRow(customerBalanceInfo);
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
            XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, Color.FromArgb(60, 94, 165));

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

        public void RenderCustomerBalanceInfosRow(CustomerBalanceInfo customerBalanceInfo)
        {
            XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.None, Color.FromArgb(255, 255, 255), Color.FromArgb(0, 0, 0));

            ExcelDocument.RenderCurrentCell(customerBalanceInfo.LogID, style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.Username, style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.TotalBalance, style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.CurrencyName, style);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(customerBalanceInfo.CustomInfo1, style);
        }
    }
}
