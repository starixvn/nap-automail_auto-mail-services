using Fanex.NAPReport.RngCasino.Business.Business;
using Fanex.NAPReport.RngCasino.Business.Business.Exports;
using Fanex.NAPReport.RngCasino.Business.Business.Exports.ExcelExport;
using Fanex.NAPReport.RngCasino.Business.Configuration;

namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Timers;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.HorseRacingReport.Business;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;
    using Fanex.NAPReport.SMALoss.Business.Business;
    using Fanex.NAPReport.TurnoverWatcher.Business.Process;
    using Fanex.Service.Core.Configuration;
    using Fanex.Service.Core.Extensions;

    public partial class AutoMailService : System.ServiceProcess.ServiceBase
    {
        private Timer _timer = null;
        private ISMALossProcess _smaLossProcess = null;
        private ITurnoverWatcherProcess _turnoverWatcherProcess = null;
        private NegativeBalanceByMatchService _negativeBalanceByMatchService = null;
	    private CustomerWinLossService _customerWinLossService;

        public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            this._smaLossProcess = new SMALossProcess();
            _turnoverWatcherProcess = new TurnoverWatcherProcess();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\NegativeBalanceByMatch\NegativeBalanceByMatch.Settings.xml");
            configurationParser.Parse<Settings>();

			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(@"SiteConfig\NegativeBalanceByMatch\LicenseeFunctionMap.xml");
            ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

            INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

            MailService mailService = new MailService(negativeBalanceByMatchExcelService);

			_negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService, mailService, 
                AppDomain.CurrentDomain.BaseDirectory + @"SiteConfig\NegativeBalanceByMatch");
		
			// RNG Casino
			ConfigurationParser rngConfigurationParser = new ConfigurationParser(@"SiteConfig\RngCasino\RngCasino.Settings.xml");
			rngConfigurationParser.Parse<RngCasinoSettings>();

			IRngCasinoExcelService rngCasinoExcelService = new RngCasinoExcelService();
			ExportSettings rngExportSettings = new ExportSettings();

            _customerWinLossService = new CustomerWinLossService(rngCasinoExcelService, rngExportSettings, 
                AppDomain.CurrentDomain.BaseDirectory + @"SiteConfig\RngCasino");
		}

        protected override void OnStart(string[] args)
        {
            ResetTimer();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
            this._timer.Stop();
            this._timer = null;
            this._smaLossProcess = null;
            AppSettings.OnProcessing = false;
            GC.Collect();
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        private void ResetTimer()
        {
            this._timer = new Timer();
            this._timer.Interval = AppSettings.Interval;
            this._timer.Elapsed += new ElapsedEventHandler(this.CallService);
            this._timer.AutoReset = true;
            this._timer.Enabled = true;
            AppSettings.OnProcessing = false;
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void CallService(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!AppSettings.OnProcessing)
                {
                    AppSettings.OnProcessing = true;

					this._smaLossProcess.NotifyEmail();
					_negativeBalanceByMatchService.ProcessBusiness();
					_turnoverWatcherProcess.Process();
					_customerWinLossService.SendMail();

                    AppSettings.OnProcessing = false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                AppSettings.OnProcessing = false;
                throw;
            }
        }

        /// <summary>
        /// Test
        /// </summary>
        public void StartAndStop()
        {
            OnStart(null);
        }
    }
}