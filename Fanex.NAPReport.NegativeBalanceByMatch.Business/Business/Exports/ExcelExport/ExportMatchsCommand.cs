namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    using System.Collections.Generic;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class ExportMatchsCommand : ExportCompositeCommand<ExportMatchCommand>
    {
        private readonly ExcelDocument _excelDocument;
        private readonly IList<Match> _matchs;
        private readonly int _numOfRowBreaks;

        public ExportMatchsCommand(ExcelDocument excelDocument, IList<Match> matchs, int numOfRowBreaks = 0) :
            base(excelDocument)
        {
            _excelDocument = excelDocument;
            _matchs = matchs;
            _numOfRowBreaks = numOfRowBreaks;
        }

        public override void Execute()
        {
            for (int index = 0; index < _commands.Count; index++)
            {
                ExcelDocument.ResetColumnIndex();

                if (index >= 1)
                {
                    for (int row = 0; row < _numOfRowBreaks; row++)
                    {
                        ExcelDocument.IncreaseRowIndex();
                    }
                }

                // Render match header
                RenderMatchHeader(index, _matchs[index].MatchInfo);

                ExcelDocument.IncreaseRowIndex();
                _commands[index].Execute();
            }
        }

        private void RenderMatchHeader(int index, MatchInfo matchInfo)
        {
            XSSFCellStyle style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, horizontalAlignment: HorizontalAlignment.Left);

            ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1);

            ExcelDocument.RenderCurrentCell(string.Format("{0}-Match {1}: {2} vs {3}", index + 1, index + 1, matchInfo.HomeName, matchInfo.AwayName), style);
        }
    }
}
