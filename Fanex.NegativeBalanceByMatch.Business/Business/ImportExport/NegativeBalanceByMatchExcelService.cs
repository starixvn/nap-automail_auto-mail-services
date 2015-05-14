using System;
using System.Collections.Generic;
using System.IO;
using Fanex.NegativeBalanceByMatch.Business.Entities;
using Fanex.Service.Core.Model;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.NegativeBalanceByMatch.Business.Business.ImportExport
{
    public class NegativeBalanceByMatchExcelService : INegativeBalanceByMatchExcelService
    {
        private NegativeBalanceByMatchService _negativeBalanceByMatchService;

        public NegativeBalanceByMatchExcelService(NegativeBalanceByMatchService negativeBalanceByMatchService)
        {
            _negativeBalanceByMatchService = negativeBalanceByMatchService;
        }

        public void GenerateReportForLicensees()
        {
            var licenseeToMatchs = _negativeBalanceByMatchService.GetLicenseeToMatchs();


            //IWorkbook workbook = new XSSFWorkbook();
            //ISheet sheet1 = workbook.CreateSheet("Test");
            //sheet1.CreateRow(0).CreateCell(0).SetCellValue("This is a Sample");
            //int x = 1;
            //for (int i = 1; i <= 15; i++)
            //{
            //    IRow row = sheet1.CreateRow(i);
            //    for (int j = 0; j < 15; j++)
            //    {
            //        row.CreateCell(j).SetCellValue(x++);
            //    }
            //}
            //FileStream sw = File.Create(@"C:\Users\george.nguyen\Desktop\Workspace\NegativeBalanceByMatch\test.xlsx");
            //workbook.Write(sw); sw.Close();
        }
    }
}
