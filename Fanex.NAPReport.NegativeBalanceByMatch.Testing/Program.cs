using System;
using System.Collections.Generic;
using Fanex.Data.Configuration;
using Fanex.NAPReport.Business.Business.Exports;
using Fanex.NAPReport.NegativeBalanceByMatch.Business;
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
				ConfigurationParser configurationParser = new ConfigurationParser(
					@"D:\Project_Nexcel\auto-mail-services\Fanex.NAPReport.NegativeBalanceByMatch.Business\bin\Debug\Configuration\Settings.xml");

				IDictionary<string, string> keyToValue =  configurationParser.Parse();
				Settings settings =	configurationParser.Parse<Settings>();

				DbSettingProviderManager.Current.Start();

				LicenseeFunctionParser licenseeFunctionParser = new LicenseeFunctionParser(Settings.LicenseeFunctionMap);
				ILicenseeService licenseeService = new LicenseeService(licenseeFunctionParser);

				INegativeBalanceByMatchExcelService negativeBalanceByMatchExcelService = new NegativeBalanceByMatchExcelService();

				MailService mailService = new MailService(negativeBalanceByMatchExcelService);

				NegativeBalanceByMatchService negativeBalanceByMatchService = new NegativeBalanceByMatchService(licenseeService, mailService);

				negativeBalanceByMatchService.ProcessBusiness();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}
		}
	}
}
