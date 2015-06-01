using System;

namespace Fanex.Service.Core.MailService
{
	public class IntervalConverter
	{
		public static int ToMiliSeconds(int interval, TimeUnit timeUnit)
		{
			switch (timeUnit)
			{
				case TimeUnit.Second:
					return interval * 1000;

				case TimeUnit.Minute:
					return interval * 1000 * 60;

				case TimeUnit.Hour:
					return interval * 1000 * 60 * 60;

				default:
					throw new Exception("TimeUnit not supported!");
			}
		}
	}
}
