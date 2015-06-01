using System.Collections.Generic;
using System.Timers;
using Fanex.Common.Extensions;

namespace Fanex.Service.Core.MailService
{
	public class StandaloneTimerService : ITimerService<IStandaloneTimerFunctionMailService>
	{
		private IList<Timer> _standaloneTimers = new List<Timer>();
		
		public void Start()
		{
			_standaloneTimers.ForEach(n => n.Start());
		}

		public void Dispose()
		{
			_standaloneTimers.ForEach(n => n.Dispose());
		}

		public void Register(IStandaloneTimerFunctionMailService standaloneTimerFunctionMailService)
		{
			Timer timer = new Timer();
			timer.Interval = standaloneTimerFunctionMailService.Interval;
			timer.Elapsed += (sender, e) =>
			{
				standaloneTimerFunctionMailService.NotifyMail();
			};

			_standaloneTimers.Add(timer);
		}
	}
}
