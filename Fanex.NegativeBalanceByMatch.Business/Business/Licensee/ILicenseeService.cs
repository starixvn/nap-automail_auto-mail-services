using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NegativeBalanceByMatch.Business.Entities;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Licensee
{
	public interface ILicenseeService
	{
		IList<string> GetLicensees();

		IList<LicenseeFunction> GetLicenseeFunctions();
	}
}
