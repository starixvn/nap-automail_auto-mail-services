using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fanex.Data.Configuration;
using Fanex.NAPReport.SMALoss.Business.Services;

namespace Fanex.NAPReport.SMALoss.Testing
{
    internal class Program
    {
        private const string TemplateUrl = @"/Templates/SMALossTemplate.cshtml";

        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //DbSettingProviderManager.Current.Start();

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + TemplateUrl;

            var lossProcessService = new SMALossProcessService();
            lossProcessService.NotifyEmail(new Uri(filePath));
        }
    }
}