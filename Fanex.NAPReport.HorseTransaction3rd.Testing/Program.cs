using System;
using Fanex.NAPReport.HorseTransaction3rd.Business.Service;
using Newtonsoft.Json;

namespace Fanex.HorseTransaction3rd.Executing
{
    public static class Program
    {
        private static void Main()
        {
            FanexDataRegisterService.ConfigureFanexData();
            var service = new TransactionServiceImp();
            var jobResult = service.Execute();

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}