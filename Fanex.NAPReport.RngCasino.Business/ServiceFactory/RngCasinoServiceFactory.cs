namespace Fanex.NAPReport.RngCasino.Business.ServiceFactory
{
    using System;
    using Fanex.NAPReport.RngCasino.Business.Business;
    using Fanex.NAPReport.RngCasino.Business.Business.Exports;
    using Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.MailService.ServiceFactory;

    public class RngCasinoServiceFactory : ITimerServiceFactory<IStandaloneTimerFunctionMailService>
    {
        public IStandaloneTimerFunctionMailService GetService()
        {
            var configurationParser = new ConfigurationParser(AppDomain.CurrentDomain.BaseDirectory + @"\SiteConfig\RNGCasinoWinLoss\Settings\RngCasino.Settings.xml");
            configurationParser.Parse<AppSettings>();

            IRngCasinoExcelService rngCasinoExcelService = new RngCasinoExcelService();
            RngCasinoExportSettings rngRngCasinoExportSettings = new RngCasinoExportSettings();

            return new CustomerWinLossService(rngCasinoExcelService, rngRngCasinoExportSettings, AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
