using Fanex.CreditMovement.Business;
using Fanex.CreditMovement.Business.Service;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;
using System;

namespace Fanex.CreditMovement.Executing
{
    public static class Program
    {
        public static void Main()
        {
            RegisterService.RegisterServices();
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\CreditMovement\Settings\CreditMovement.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new CreditNotificationServiceImp();
            var jobResult = service.Execute();

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}