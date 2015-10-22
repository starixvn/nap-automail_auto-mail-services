namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.Utilities;

    public partial class AutoMailService : ServiceBase
    {
        private TimerServiceFacade _timerServiceFacade;

        public AutoMailService()
        {
            try
            {
                InitializeComponent(AppSettingBase.ServiceName);
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                DbSettingProviderManager.Current.Start();
                MailClientHandler.Start();

                _timerServiceFacade = new TimerServiceFacade(AppDomain.CurrentDomain.BaseDirectory + @"TimerServiceSetting\TimerServiceSetting.xml");
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogExtensions.LogServiceChangeStatusTime(true);
                _timerServiceFacade.StartServices();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        protected override void OnStop()
        {
            LogExtensions.LogServiceChangeStatusTime(false);
            _timerServiceFacade.StopServices();
            MailClientHandler.Stop();
        }

        /// <summary>
        /// Console Test
        /// </summary>
        public void StartAndStop()
        {
            OnStart(null);
        }
    }
}