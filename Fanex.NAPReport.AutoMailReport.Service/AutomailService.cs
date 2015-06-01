using System;
using System.IO;
using System.ServiceProcess;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.Service.Core.MailService;
using Fanex.Service.Core.Utilities;

namespace Fanex.NAPReport.AutoMailReport.Service
{
	public partial class AutoMailService : ServiceBase
	{
		private TimerServiceFacade _timerServiceFacade;

		public AutoMailService()
		{
			InitializeComponent(AppSettingBase.ServiceName);
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
			DbSettingProviderManager.Current.Start();
			MailClientHandler.Start();

			_timerServiceFacade = new TimerServiceFacade(AppDomain.CurrentDomain.BaseDirectory + @"TimerServiceSetting\TimerServiceSetting.xml");
		}

		protected override void OnStart(string[] args)
		{
			_timerServiceFacade.StartServices();
		}

		protected override void OnStop()
		{
			_timerServiceFacade.StopServices();

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