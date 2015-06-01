using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Service.Core.MailService
{
	public interface ITimerFunctionMailService : IFunctionMailService
	{
		int Interval { get; set; }
	}
}
