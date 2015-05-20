using System.Collections.Generic;
using System.Linq;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee
{
	public class LicenseeService : ILicenseeService
	{
		private readonly LicenseeFunctionParser _licenseeFunctionParser;
		private readonly IList<LicenseeFunction> _licenseeFunctions;

		public LicenseeService(LicenseeFunctionParser licenseeFunctionParser)
		{
			_licenseeFunctionParser = licenseeFunctionParser;
			_licenseeFunctions = _licenseeFunctionParser.LicenseeFunctions;
		}

		public IList<string> GetLicensees()
		{
			return _licenseeFunctions.Select(lf => lf.LicenseeName).ToList();
		}

		public IList<LicenseeFunction>  GetLicenseeFunctions()
		{
			return _licenseeFunctions;
		} 
	}
}
