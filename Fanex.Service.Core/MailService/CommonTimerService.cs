using System.Timers;

namespace Fanex.Service.Core.MailService
{
	/// <summary>
	/// Common Timer Service Base. 
	/// </summary>
	public class CommonTimerService : ITimerService<ICommonTimerFunctionMailService>
	{
		private readonly int _commonInterval;
		private readonly Timer _commonTimer = new Timer();

		public CommonTimerService(int commonInterval)
		{
			_commonInterval = commonInterval;
		}

		public void Start()
		{
			_commonTimer.Start();
		}

		public void Dispose()
		{
			_commonTimer.Dispose();
		}

		public void Register(ICommonTimerFunctionMailService standaloneTimerTimerFunctionMailService)
		{
			_commonTimer.Interval = _commonInterval;
			_commonTimer.Elapsed += (sender, e) =>
			{
				standaloneTimerTimerFunctionMailService.NotifyMail();
			};
		}
	}
}
