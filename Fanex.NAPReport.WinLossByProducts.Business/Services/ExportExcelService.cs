// -----------------------------------------------------------------------
// <copyright file="ExportExcelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Repositories;
    using Fanex.NAPReport.WinLossByProducts.Business.Resources;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Utilities;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExportExcelService : IExportExcelService
    {
        private XSSFWorkbook _workbook;
        private IRepository _repository;
        private Random _rd;

        public ExportExcelService()
        {
            _workbook = new XSSFWorkbook();
            _repository = new Repository();
            _rd = new Random();
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

        public void ExportExcel(OverallWinLossProductsCriteria criteria)
        {
            OverallWinLossProducts3rdCriteria criteria3rd = new OverallWinLossProducts3rdCriteria();
            criteria3rd.rptdate = criteria.rptdate;

            List<Product> products = _repository.GetOverallWinLossByProducts(criteria);            
            List<Product> sportbook3rdParty = _repository.GetOverallWinLossBySportbook3rdParty(criteria3rd);

            OverallWinLossProductsExcel main = new OverallWinLossProductsExcel(_workbook);
            main.ExportExcel(products, criteria);

            OverallWinLossSportbook3rdPartyExcel detail = new OverallWinLossSportbook3rdPartyExcel(_workbook);
            detail.ExportExcel(sportbook3rdParty, criteria);

            FileName = CreateReportNameWithExtension(criteria);
            DirectoryOfFileName = Path.Combine(AppSettings.DataDirectoryWithYearMonth, FileName);
            SaveFile.SaveExcelFile(AppSettings.DataDirectoryWithYearMonth, FileName, _workbook, AppSettings.ExcelPasswordProtected); 
        }

        private string CreateReportNameWithExtension(OverallWinLossProductsCriteria criteria)
        {
            string reportName = string.Format(Labels.Overall_Win_Loss_Products, criteria.rptdate.ToString("yyyy_MM_dd"));

            return reportName;
        }
    }
}
