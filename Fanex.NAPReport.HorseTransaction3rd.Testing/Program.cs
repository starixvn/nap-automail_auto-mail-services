using System;
using Fanex.NAPReport.HorseTransaction3rd.Business.Service;
using Newtonsoft.Json;

namespace Fanex.HorseTransaction3rd.Executing
{
    public class Program
    {
        private static void Main(string[] args)
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