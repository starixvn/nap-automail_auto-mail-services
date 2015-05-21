using System.ServiceProcess;
using System.Timers;
using System.IO;
using System;
using System.Collections.Generic;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.NAPReport.Business;
using Fanex.Service.Core.Extensions;
using Fanex.NAPReport.Business.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Service
{
    internal partial class AutoMailService : ServiceBase
    {
       /// <summary>
        /// The timer.
        /// </summary>
        private static Timer _timer;
        private static NegativeBalanceByMatchService _service;

		public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        /// <summary>
        /// Implement when service start.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);


			DbSettingProviderManager.Current.Start();

			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(AppSettings.LicenseeFunctionMap);
			ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

			MailService mailService = new MailService(negativeBalanceByMatchExcelService);

			_service = new NegativeBalanceByMatchService(licenseeService, mailService);

            _timer = new Timer();
            _timer.Interval = AppSettings.Interval;
            _timer.Elapsed += new ElapsedEventHandler(OnInterval);
            _timer.AutoReset = true;
            _timer.Enabled = true;

            AppSettings.OnProcessing = false;
            AppSettings.ForceRunningOnProcessing = false;
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            _timer.Stop();
            _timer = null;
            _service = null;
            AppSettings.OnProcessing = false;
            GC.Collect();
        }

        /// <summary>
        /// Called when [interval].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private static void OnInterval(object source, ElapsedEventArgs e)
        {
            try
            {
                if (!AppSettings.OnProcessing)
                {
                    AppSettings.OnProcessing = true;
                    _service.ProcessBusiness();
                    AppSettings.OnProcessing = false;
                }
            }
            catch (Exception ex)
            {
                AppSettings.OnProcessing = false;
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

		/// <summary>
		/// Test
		/// </summary>
		public void StartAndStop()
		{
			DbSettingProviderManager.Current.Start();

			LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(AppSettings.LicenseeFunctionMap);
			ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

			INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

			MailService mailService = new MailService(negativeBalanceByMatchExcelService);
			_service = new NegativeBalanceByMatchService(licenseeService, mailService);
			_service.ProcessBusiness();
		}
    }
}