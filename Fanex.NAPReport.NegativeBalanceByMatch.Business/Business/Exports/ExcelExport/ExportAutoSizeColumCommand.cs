using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAPReport.Business.Business.Exports.ExcelExport;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NAPReport.Business.Business.Exports.ExcelExport
{
    public class ExportAutoSizeColumCommand : ExportCommandBase
    {
        public ExportAutoSizeColumCommand(ExcelDocument excelDocument)
            : base(excelDocument)
        {
        }

        public override void Execute()
        {
            XSSFSheet workSheet = ExcelDocument.Workbook.GetSheetAt(0) as XSSFSheet;

            for (int i = 0; i < ExcelDocument.ExportSettings.MaxNumOfColumns; i++)
            {
                int colIndex = i + ExcelDocument.ExportSettings.StartColIndex;
                workSheet.AutoSizeColumn(colIndex);
                workSheet.SetColumnWidth(colIndex, workSheet.GetColumnWidth(colIndex) + ExcelDocument.ExportSettings.ColumnPadding);
            }
        }
    }
}
