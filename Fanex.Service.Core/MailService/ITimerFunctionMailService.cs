using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Service.Core.MailService
{
	public interface ITimerFunctionMailService
	{
		void NotifyMail();

		int Interval { get; set; }
	}
}
