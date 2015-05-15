using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fanex.Data.Configuration;

namespace robert_test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
        }
    }
}