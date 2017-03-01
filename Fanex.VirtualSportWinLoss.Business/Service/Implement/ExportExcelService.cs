namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement
{
    using System;
    using System.IO;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;
    using Fanex.Service.Core.Utilities;
    using Fanex.VirtualSportWinLoss.Business.Resources;
    using NPOI.XSSF.UserModel;

    public class ExportExcelService : IExportExcelService
    {
        private XSSFWorkbook _workbook;

        public ExportExcelService()
        {
            _workbook = new XSSFWorkbook();
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
            FileName = string.Format(WinLossVirtualSportResoure.VirtualSportWinLoss_ReportName, DateTime.Today.ToString("yyyy_MM_dd"));
            DirectoryOfFileName = Path.Combine(AppSettings.DataDirectoryWithYearMonth, FileName);
            SaveFile.SaveExcelFile(AppSettings.DataDirectory, AppSettings.DataDirectoryWithYearMonth, FileName, _workbook, AppSettings.ExcelPasswordProtected);
        }
    }
}