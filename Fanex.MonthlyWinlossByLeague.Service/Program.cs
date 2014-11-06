// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Service
{
    using System.ServiceProcess;

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
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new AutomailService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}