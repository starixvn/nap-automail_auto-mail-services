using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.WinLossByProducts.Business.Criteria;
using Fanex.WinLossByProducts.Business.Services.Implement;
using Fanex.WinLossByProducts.Business.Services.Interface;

namespace Fanex.NAPReport.WinLossByProducts.Testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            IWinLossByProductsService service = new WinLossByProductsService();
            IWinLossByProductsExportDataService exportDataService = new WinLossByProductsExportDataService();
            Console.WriteLine("Running Win Loss By Products Report");

            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            var report = service.GetWinLossByProductsData(criteria);
            string reportPath = exportDataService.ExportReportToExcel(report, criteria);
            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}