using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.OandaExChangeRate.Business;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.OandaExChangeRate.Testing.Service;

namespace Fanex.OandaExChangeRate.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            DbSettingProviderManager.Current.Start();

            ProcessExChangeRate oandaCurrency = new ProcessExChangeRate();
            oandaCurrency.OnRunningUrgently();

            //ExportExcelOandaExchangeRate export = new ExportExcelOandaExchangeRate();
            //export.GetDataByDate();

            Console.WriteLine("Done on " + Business.AppSettings.TestDate);
            Console.ReadLine();
        }
    }
}
