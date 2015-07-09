namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.Common;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using Fanex.Service.Core.Service.Implement.Npoi;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class ExportContentCommand : ExportCommandBase
    {
        private IEnumerable<FormattedSMALossGroup> _lossGroup;
        private INpoiNewFormatCustomCellStyleService _customCellStyleService;
        private INpoiNewFormatFontService _fontService;
        private XSSFFont _regularFont;
        private XSSFFont _regularBoldFont;
        private XSSFCellStyle _decimalStyle;
        private XSSFCellStyle _percentStyle;
        private XSSFCellStyle _textLeftStyle;
        private XSSFCellStyle _textCenterStyle;

        public ExportContentCommand(ExcelDocument excelDocument, IEnumerable<FormattedSMALossGroup> lossGroup)
            : base(excelDocument)
        {
            _lossGroup = lossGroup;

            _customCellStyleService = new NpoiNewFormatCustomCellStyleService(excelDocument.Workbook);
            _fontService = new NpoiNewFormatFontService(excelDocument.Workbook); 
            _regularFont = _fontService.RegularFont;
            _regularBoldFont = _fontService.BlackBoldFont;            
            _decimalStyle = _customCellStyleService.CreateDecimalNumberStyle(Color.White, _regularFont);
            _percentStyle = _customCellStyleService.CreatePercentageStyle(Color.White, _regularFont);
            _textLeftStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _regularFont);
            _textCenterStyle = _customCellStyleService.CreateCenterTextStyle(Color.White, _regularFont);
        }

        public override void Execute()
        {
            this.RenderContent();
        }

        private Color LevelColor(byte level)
        {
            switch (level)
            {
                case 1:
                    return Color.FromArgb(79, 23, 119);
                case 2:
                    return Color.FromArgb(13, 127, 0);
                case 3:
                    return Color.FromArgb(16, 88, 194);
                case 4:
                    return Color.FromArgb(255, 109, 27);
                case 5:
                    return Color.FromArgb(217, 18, 122);
                case 6:
                    return Color.FromArgb(210, 0, 01);
                default:
                    return Color.FromArgb(0, 0, 0);
            }
        }

        private void RenderContent()
        {
            var counter = 0;

            foreach (var group in _lossGroup)
            {
                ExcelDocument.IncreaseRowIndex();
                var levelColor = LevelColor(group.LossLevel);

                _regularBoldFont = _fontService.BlackBoldFont;            
                _regularBoldFont.SetColor(new XSSFColor(levelColor));
                var textLeftColorStyle = _customCellStyleService.CreateLeftTextStyle(Color.White, _regularBoldFont);
                
                ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1, textLeftColorStyle);
                ExcelDocument.RenderCurrentCell(string.Format("{0} ({1})", group.LossLevelRange, group.SMALossItems.Count), textLeftColorStyle);

                foreach (var data in group.SMALossItems)
                {
                    counter++;
                    ExcelDocument.IncreaseRowIndex();
                    this.LossData(counter, data);
                }
            }
        }

        private void LossData(int index, FormattedSMALossItem data)
        {
            ExcelDocument.RenderCurrentCell(index, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Username, _textLeftStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Level, _textCenterStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Currency, _textCenterStyle); 

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(Converter.To<decimal>(data.YesterdayBalance), _decimalStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(Converter.To<decimal>(data.TotalBalance), _decimalStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(Converter.To<decimal>(data.MaxLoss), _decimalStyle);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(Converter.To<decimal>(data.LossPercentage), _percentStyle); 

            ExcelDocument.ResetColumnIndex();
        }
    }
}
