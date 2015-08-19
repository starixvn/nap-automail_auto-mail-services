// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System.ServiceProcess;

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        private static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new AutoMailService()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}