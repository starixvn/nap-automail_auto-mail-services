using Fanex.NAPReport.ReportingServices.Business;
using Fanex.ReportingServices.Business.Service;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;
using System;

namespace Fanex.ReportingServices.Executing
{
    public static class Program
    {
        public static void Main()
        {
            RegisterService.RegisterServices();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\ReportingServices\Settings\ReportingService.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new ReportNotificationImp();
            var jobResult = service.Execute();

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}