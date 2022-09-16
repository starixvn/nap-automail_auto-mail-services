using Fanex.NAPReport.VirtualSportWinLoss.Business;
using Fanex.Service.Core.Configuration;
using Fanex.VirtualSportWinLoss.Business.Service;
using Newtonsoft.Json;
using System;

namespace Fanex.VirtualSportWinLoss.Executing
{
    public static class Program
    {
        public static void Main()
        {
            RegisterService.RegisterServices();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\VirtualSportWinLoss\Settings\VirtualSportWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new VirtualSportNotificationImp();
            var jobResult = service.Execute();

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}