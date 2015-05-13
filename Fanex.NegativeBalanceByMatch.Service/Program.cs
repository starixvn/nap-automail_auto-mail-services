using System.ServiceProcess;

namespace Fanex.NegativeBalanceByMatch.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                                {
                                    new AutoMailService()
                                };
            ServiceBase.Run(ServicesToRun);
        }
    }
}