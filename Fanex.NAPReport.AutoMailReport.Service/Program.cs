// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

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

			AutoMailService mailService = new AutoMailService();

			mailService.StartAndStop();
        }
    }
}