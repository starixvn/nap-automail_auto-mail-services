namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports.ExcelExport
{
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class ExportMatchSettleLogsCommand : ExportCommandBase
    {
        private IList<MatchSettleLog> _matchSettleLogs;

        private INpoiNewFormatFontService _fontService;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private XSSFFont _regularFont;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _textCenterStyle;
        private XSSFCellStyle _dateTimeCenterStyle;
        private XSSFDataFormat _dataFormat;

        public ExportMatchSettleLogsCommand(ExcelDocument excelDocument, IList<MatchSettleLog> matchSettleLogs)
            : base(excelDocument)
        {
            _matchSettleLogs = matchSettleLogs;

            _fontService = new NpoiNewFormatFontService(excelDocument.Workbook);
            _regularFont = _fontService.RegularFont;
            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(excelDocument.Workbook);
            _decimalStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);
            _textCenterStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _regularFont);
            _dataFormat = excelDocument.Workbook.CreateDataFormat() as XSSFDataFormat;

            _dateTimeCenterStyle = excelDocument.Workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(_dateTimeCenterStyle, Color.White);
            _dateTimeCenterStyle.Alignment = HorizontalAlignment.Center;
            _dateTimeCenterStyle.DataFormat = _dataFormat.GetFormat("m-d-yyyy hh:mm AM/PM");
            _dateTimeCenterStyle.SetFont(_regularFont);
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
            XSSFCellStyle style = _customCellStyleService.CreateCenterTextStyle(Color.FromArgb(60, 94, 165), _fontService.WhiteBoldFont);

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
            ExcelDocument.RenderCurrentCell(index + 1, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(matchSettleLog.SettlementTime, _dateTimeCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(matchSettleLog.Action, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(matchSettleLog.HTHomeScore + " - " + matchSettleLog.HTAwayScore, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(matchSettleLog.FinalHomeScore + " - " + matchSettleLog.FinalAwayScore, _textCenterStyle);
        }
    }
}