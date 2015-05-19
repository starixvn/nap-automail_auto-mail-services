using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAPReport.Business.Entities;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee
{
	public interface ILicenseeService
	{
		IList<string> GetLicensees();

		IList<LicenseeFunction> GetLicenseeFunctions();
	}
}
