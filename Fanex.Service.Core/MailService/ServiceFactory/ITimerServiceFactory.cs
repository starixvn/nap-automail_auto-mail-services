using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Service.Core.MailService.ServiceFactory
{
	public interface ITimerServiceFactory<out T> where T : ITimerFunctionMailService
	{
		T GetService();
	}
}
