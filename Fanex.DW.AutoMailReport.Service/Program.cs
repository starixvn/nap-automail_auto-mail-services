namespace Fanex.DW.AutoMailReport.Service
{
    using System.ServiceProcess;

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