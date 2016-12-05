using System;
using System.Configuration;
using System.IO;
using System.Timers;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client;
using Fanex.NAPReport.TurnoverWatcher.Business.Service;
using Fanex.Service.Core.Extensions;
using ONEbook.UM.Services;

namespace Fanex.TurnoverWatcher.Service
{
    public class TurnoverWatcherNotificationService
    {
        private readonly IUMControlAdapter umcontrolAdapter;

        private Timer timer;
        private bool isOnProcessing = false;

        public TurnoverWatcherNotificationService(IUMControlAdapter umcontrolAdapter)
        {
            //this.umcontrolAdapter = umcontrolAdapter;            
            //Register();
        }

        public bool Start()
        {
            //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //DbSettingProviderManager.Current.Start();
            //timer.Start();
            //MailClientHandler.Start();

            return true;
        }

        public bool Stop()
        {
            //timer.Stop();
            //MailClientHandler.Stop();

            //timer.Dispose();

            return true;
        }

        private void Register()
        {
            timer = new Timer();
            timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecond"]) * 1000;
            timer.Elapsed += (sender, e) =>
            {
                if (!isOnProcessing)
                {
                    Execute();
                }
            };
        }

        private void Execute()
        {
            try
            {
                if (umcontrolAdapter.GetUnderMaintenanceTime().IsUnderMaintenanceTime)
                {
                    return;
                }

                isOnProcessing = true;

                var service = new MailingService();
                service.SendMailToVIPGroup();

                isOnProcessing = false;
            }
            catch (Exception ex)
            {
                isOnProcessing = false;
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}