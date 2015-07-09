namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.IO;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Resources;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    public class ExportExcelService : IExportExcelService
    {
        private XSSFWorkbook _workbook;
        public ExportExcelService()
        {
            _workbook = new XSSFWorkbook();
        }

        public void ExportExcel()
        {
            var main = new ExportExcelWinLossSummaryService(_workbook);
            main.ExportExcelMain();

            var detail = new ExportExcelWinLossDetailService(_workbook);
            detail.ExportExcelDetail();

            SaveExcelFile();
        }

        private void SaveExcelFile()
        {
            FileName = string.Format(WinLossVirtualSportResoure.VirtualSportWinLoss_ReportName, DateTime.Today.ToShortFormatedDate().Replace('/', '_')) + ".xlsx";
            DirectoryOfFileName = Path.Combine(AppSettings.DataDirectoryWithYearMonth, FileName);
            SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYearMonth, FileName, _workbook, AppSettings.ExcelPasswordProtected);
        }

        public string DirectoryOfFileName
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }
    }
}