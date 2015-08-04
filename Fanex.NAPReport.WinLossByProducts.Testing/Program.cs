namespace Fanex.NAPReport.WinLossByProducts.Testing
{
    using System;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;
    using System.Collections.Generic;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.Service.Core.Configuration;
    using Fanex.NAPReport.WinLossByProducts.Business;
    using Fanex.NAPReport.WinLossByProducts.Business.Resources;

    public class Program
    {
        public static void Main(string[] args)
        {            
            DbSettingProviderManager.Current.Start();
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\OverallWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            Console.WriteLine("Running Win Loss By Products Report");

            //IMailingServiceBase service = new MailingService();
            //var mail = service.SendMailToVIPGroup();                       
            
            IExportExcelService excel = new ExportExcelService();
            
            OverallWinLossProductsCriteria criteria = new OverallWinLossProductsCriteria();
            criteria.rptdate = AppSettings.WinLossDate;

            excel.ExportExcel(criteria);

            //OverallWinLossProductsMailingService mailService = new OverallWinLossProductsMailingService();
            //var mail = mailService.SendMailToVIPGroup();

            Console.WriteLine("Done at " + DateTime.Now.ToString());            
        }      
    }
}