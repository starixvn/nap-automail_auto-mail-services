namespace Fanex.MonthlyWinlossByLeague.Service
{
    using System.ServiceProcess;

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