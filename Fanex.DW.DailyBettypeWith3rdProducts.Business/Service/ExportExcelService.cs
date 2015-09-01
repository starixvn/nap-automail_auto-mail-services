namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    public class ExportExcelService : IExportExcelService
    {
        private XSSFWorkbook _workbook;
        private string _fileName;
        private string _directoryOfFileName;

        public ExportExcelService()
        {
            _workbook = new XSSFWorkbook();
            _fileName = string.Empty;
            _directoryOfFileName = string.Empty;
            DirectoryOfFileNames = new List<string>();
            FileNames = new List<string>();
        }

        public List<string> DirectoryOfFileNames
        {
            get;
            set;
        }

        public List<string> FileNames
        {
            get;
            set;
        }

        public void ExportExcelSeparateRealAndVirtual(bool isVirtual, bool isDaily)
        {
            _workbook = new XSSFWorkbook();
            BettypeWith3rdProductsExcelService excel = new BettypeWith3rdProductsExcelService(_workbook);

            GetFileNameSeparateRealAndVirtual(isVirtual, isDaily);           

            excel.GenerateReport(isVirtual, isDaily);

            _directoryOfFileName = Path.Combine(isDaily ? AppSettings.DirectoryForDaily : AppSettings.DirectoryForWeekly, _fileName);
            SaveFile.SaveExcelFile(isDaily ? AppSettings.DirectoryForDaily : AppSettings.DirectoryForWeekly, _fileName, _workbook, AppSettings.ExcelPasswordProtected);

            DirectoryOfFileNames.Add(_directoryOfFileName);
            FileNames.Add(_fileName);
        }

        public void ExportExcelCombineRealAndVirtual(bool isDaily)
        {
            BettypeWith3rdProductsExcelService excel = new BettypeWith3rdProductsExcelService(_workbook);
            
            GetFileNameCombineRealAndVirtual();

            excel.GenerateReport(false, isDaily);
            excel.GenerateReport(true, isDaily);

            _directoryOfFileName = Path.Combine(isDaily ? AppSettings.DirectoryForDaily : AppSettings.DirectoryForWeekly, _fileName);
            SaveFile.SaveExcelFile(isDaily ? AppSettings.DirectoryForDaily : AppSettings.DirectoryForWeekly, _fileName, _workbook, AppSettings.ExcelPasswordProtected);

            DirectoryOfFileNames.Add(_directoryOfFileName);
            FileNames.Add(_fileName);
        }

        private void GetFileNameSeparateRealAndVirtual(bool isVirtual, bool isDaily)
        {
            if (isDaily)
            {
                if (isVirtual)
                {
                    _fileName = string.Format(Resource.Resource.DailyFileNameVirtual, DateTime.Today.ToString("yyyy_MM_dd"));
                }
                else
                {
                    _fileName = string.Format(Resource.Resource.DailyFileNameReal, DateTime.Today.ToString("yyyy_MM_dd"));
                }
            }
            else
            {
                if (isVirtual)
                {
                    _fileName = string.Format(Resource.Resource.WeeklyFileNameVirtual, DateTime.Today.ToString("yyyy_MM_dd"));
                }
                else
                {
                    _fileName = string.Format(Resource.Resource.WeeklyFileNameReal, DateTime.Today.ToString("yyyy_MM_dd"));
                }
            }
        }

        private void GetFileNameCombineRealAndVirtual()
        {
            if (AppSettings.IsSingleFile)
            {
                _fileName = string.Format(Resource.Resource.DailyFileName, DateTime.Today.ToString("yyyy_MM_dd"));
            }
            else
            {
                _fileName = string.Format(Resource.Resource.WeeklyFileName, DateTime.Today.ToString("yyyy_MM_dd"));
            }
        }
    }
}
