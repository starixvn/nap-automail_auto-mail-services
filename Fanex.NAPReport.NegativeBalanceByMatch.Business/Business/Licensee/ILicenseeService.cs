using System.Collections.Generic;
using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee
{
	public interface ILicenseeService
	{
		IList<string> GetLicensees();

		IList<LicenseeFunction> GetLicenseeFunctions();
	}
}
