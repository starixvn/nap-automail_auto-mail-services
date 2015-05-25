// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Service
{
    using System.ServiceProcess;
    using System;
    using Fanex.Data.Configuration;

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            //ServiceBase[] servicesToRun;
            //servicesToRun = new ServiceBase[]
            //{
            //    new AutoMailService()
            //};
            //ServiceBase.Run(servicesToRun);
            DbSettingProviderManager.Current.Start();
            var service = new SMAReport.Business.Service.SMAReportService();
            var email = service.SMAReport();
            Console.WriteLine("Test Done");
        }
    }
}