using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport;
using Fanex.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    public class ExportMatchInfoCommand : ExportCommandBase
    {
        private readonly MatchInfo _matchInfo;

        public ExportMatchInfoCommand(ExcelDocument excelDocument, MatchInfo matchInfo)
            : base(excelDocument)
        {
            _matchInfo = matchInfo;
        }

        public override void Execute()
        {
            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);

            ExcelDocument.RenderCurrentCell(string.Format("Event Date: {0}", _matchInfo.EventDate.ToShortDateString()));

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);
            ExcelDocument.RenderCurrentCell(string.Format("Match Code: {0}", _matchInfo.MatchCode));

            ExcelDocument.IncreaseRowIndex();
            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);
            ExcelDocument.RenderCurrentCell(string.Format("Match ID: {0}", _matchInfo.MatchID));
        }
    }
}
