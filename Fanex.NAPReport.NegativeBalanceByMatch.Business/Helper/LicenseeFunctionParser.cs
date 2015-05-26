using System.Collections.Generic;
using System.Xml.Linq;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper
{
	public class LicenseeFunctionParser
	{
		public LicenseeFunctionParser(string fileNamePath)
		{
			_fileNamePath = fileNamePath;
			LicenseeFunctions = new List<LicenseeFunction>();
			Parse();
		}

		public IList<LicenseeFunction> LicenseeFunctions;

		private readonly string _fileNamePath;

		private void Parse()
		{
			XElement licenseeFunctions = XElement.Load(_fileNamePath);

			foreach (XElement licenseeFunction in licenseeFunctions.Elements())
			{
				LicenseeFunctions.Add(
					new LicenseeFunction()
					{
						LicenseeName = licenseeFunction.Attribute("licenseeName").Value,
						Function	 = new Function
						{
							Id = int.Parse(licenseeFunction.Attribute("functionId").Value),
							Name = licenseeFunction.Attribute("functionName").Value
						}
					}
				);
			}
		}
	}
}
