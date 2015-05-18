using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.NegativeBalanceByMatch.Business.Business.Licensee
{
	public interface ILicenseeService
	{
		IList<string> GetLicensees();
	}
}
