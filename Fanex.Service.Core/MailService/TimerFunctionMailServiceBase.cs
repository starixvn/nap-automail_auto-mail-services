using System.Threading.Tasks;
using System.Threading;
using System;
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
				if (IsActive() == true)
				{
					ResetState();
                    
					if (NotifyMailVipGroup() == true)
					{
						NotifyMailNormalGroup();
					}
				}                
			}

            GC.Collect();
		}

		public abstract bool IsActive();

		public virtual void ResetState()
		{
		}

		public abstract bool NotifyMailVipGroup();

		public virtual void NotifyMailNormalGroup()
		{
		}

		public virtual int Interval { get; set; }
	}
}
