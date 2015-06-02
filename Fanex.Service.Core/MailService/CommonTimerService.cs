using System.Timers;

namespace Fanex.Service.Core.MailService
{
	/// <summary>
	/// Common Timer Service Base. 
	/// </summary>
	public class CommonTimerService : ITimerService<ICommonTimerFunctionMailService>
	{
		private readonly int _commonInterval;
		private Timer _commonTimer;

		public CommonTimerService(int commonInterval)
		{
			_commonInterval = commonInterval;
		}

		public void Start()
		{
			if (_commonTimer != null)
			{
				_commonTimer.Start();
			}
		}

		public void Dispose()
		{
			if (_commonTimer != null)
			{
				_commonTimer.Dispose();
			}
		}

		public void Register(ICommonTimerFunctionMailService standaloneTimerTimerFunctionMailService)
		{
			if (_commonTimer == null)
			{
				_commonTimer = new Timer { Interval = _commonInterval };
			}

			_commonTimer.Elapsed += (sender, e) =>
			{
				standaloneTimerTimerFunctionMailService.NotifyMail();
			};
		}
	}
}
