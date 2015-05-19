

namespace Fanex.NAPReport.SMALoss.Business.Services.ExcelExport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAPReport.SMALoss.Business.Entities;
    using NPOI.SS.UserModel;
    using System.Drawing;
    using NPOI.XSSF.UserModel;
    public class ExportContentCommand : ExportCommandBase
    {
        private IEnumerable<FormattedSMALossGroup> _lossGroup;

        public ExportContentCommand(ExcelDocument excelDocument, IEnumerable<FormattedSMALossGroup> lossGroup)
            : base(excelDocument)
        {
            _lossGroup = lossGroup;
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

                var style = ExcelDocument.GetCellStyle(FontBoldWeight.Bold, Color.FromArgb(255, 255, 255), Color.FromArgb(0, 0, 0), levelColor, HorizontalAlignment.Left);
                ExcelDocument.RenderMergeCell(ExcelDocument.CurrentRow, ExcelDocument.CurrentRow, ExcelDocument.CurrentColumn, ExcelDocument.CurrentColumn + ExcelDocument.ExportSettings.MaxNumOfColumns - 1, style);
                ExcelDocument.RenderCurrentCell(string.Format("{0} ({1})", group.LossLevelRange, group.SMALossItems.Count), style);

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
            var styleLeft = ExcelDocument.GetCellStyle(FontBoldWeight.Normal, default(Color), Color.FromArgb(0, 0, 0), default(Color), HorizontalAlignment.Left);
            var styleRight = ExcelDocument.GetCellStyle(FontBoldWeight.Normal, default(Color), Color.FromArgb(0, 0, 0), default(Color), HorizontalAlignment.Right);
            var styleCenter = ExcelDocument.GetCellStyle(FontBoldWeight.Normal, default(Color), Color.FromArgb(0, 0, 0), default(Color), HorizontalAlignment.Center);

            ExcelDocument.RenderCurrentCell(index, styleCenter);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Username, styleLeft);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Level, styleCenter);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.Currency, styleCenter);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.YesterdayBalance, styleRight);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.TotalBalance, styleRight);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(data.MaxLoss, styleRight);

            ExcelDocument.IncreaseColumnIndex();
            ExcelDocument.RenderCurrentCell(string.Format("{0}%", data.LossPercentage), styleRight);

            ExcelDocument.ResetColumnIndex();
        }
    }
}
