using System;
using System.Collections.Generic;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Configuration;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;
using Fanex.Service.Core.Configuration;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Testing
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string baseDir = AppDomain.CurrentDomain.BaseDirectory;

				ConfigurationParser configurationParser = new ConfigurationParser(baseDir + @"\Configuration\NegativeBalanceByMatch.Settings.xml");

				IDictionary<string, string> keyToValue = configurationParser.Parse();
				configurationParser.Parse<Settings>();

				DbSettingProviderManager.Current.Start();

				LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(baseDir + @"\Configuration\LicenseeFunctionMap.xml");
				ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

				INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

				MailService mailService = new MailService(negativeBalanceByMatchExcelService);

				NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService, mailService, baseDir);

				MailClientHandler.Start();
				negativeBalanceByMatchService.ProcessBusiness();
				MailClientHandler.Stop();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}
		}
	}
}
