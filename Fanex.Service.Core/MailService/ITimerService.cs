using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Service.Core.MailService
{
	public interface ITimerService<in T> : IStartable, IDisposable where T : ITimerFunctionMailService
	{
		void Register(T functionMailService);
	}
}
