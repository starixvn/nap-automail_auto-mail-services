namespace Fanex.Service.Core.MailService
{
	/// <summary>
	/// Timer Function Mail Service Thread-safe.
	/// </summary>
	public abstract class TimerFunctionMailServiceBase : ITimerFunctionMailService
	{
		public void NotifyMail()
		{
			lock (this)
			{
				NotifyMailSync();
			}
		}

		public virtual void NotifyMailSync()
		{
		}

		public virtual int Interval { get; set; }
	}
}
