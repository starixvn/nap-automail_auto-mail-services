using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Service.Core.Model;

namespace Fanex.NAPReport.Business.Business.Exports.ExcelExport
{
    public class ExportMatchCommand : ExportCommandBase
    {
        private ExportMatchInfoCommand _exportMatchInfoCommand;
        private ExportMatchSettleLogsCommand _exportMatchSettleLogsCommand;
        private ExportCustomerBalanceInfosCommand _exportCustomerBalanceInfoCommand;

        public ExportMatchCommand(ExcelDocument excelDocument,
            ExportMatchInfoCommand exportMatchInfoCommand,
            ExportMatchSettleLogsCommand exportMatchSettleLogsCommand,
            ExportCustomerBalanceInfosCommand exportCustomerBalanceInfoCommand)
            : base(excelDocument)
        {
            _exportMatchInfoCommand = exportMatchInfoCommand;
            _exportMatchSettleLogsCommand = exportMatchSettleLogsCommand;
            _exportCustomerBalanceInfoCommand = exportCustomerBalanceInfoCommand;
        }

        public override void Execute()
        {
           _exportMatchInfoCommand.Execute();

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.ResetColumnIndex();
            _exportMatchSettleLogsCommand.Execute();

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.ResetColumnIndex();
            _exportCustomerBalanceInfoCommand.Execute();
        }
    }
}
