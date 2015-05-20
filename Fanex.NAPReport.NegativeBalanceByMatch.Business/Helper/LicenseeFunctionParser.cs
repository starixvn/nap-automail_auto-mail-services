using System.Collections.Generic;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper
{
	public class LicenseeFunctionParser
	{
		public LicenseeFunctionParser(string stringToParse)
		{
			_stringToParse = stringToParse;
			Parse();
		}

		public IList<LicenseeFunction> LicenseeFunctions;

		private string _stringToParse;

		private void Parse()
		{
			LicenseeFunctions = new List<LicenseeFunction>();

			string[] licenseeFunctionGroups = _stringToParse.Split(';');

			foreach (string licenseeFunctionGroup in licenseeFunctionGroups)
			{
				string[] licenseeFunctionTokens = licenseeFunctionGroup.Trim(' ', '(', ')').Split(',');

				LicenseeFunctions.Add(
					new LicenseeFunction()
					{
						LicenseeName = licenseeFunctionTokens[0],
						Function = new Function
						{
							Id = int.Parse(licenseeFunctionTokens[1]),
							Name = licenseeFunctionTokens[2]
						}
					}
				);
			}
		}
	}
}
