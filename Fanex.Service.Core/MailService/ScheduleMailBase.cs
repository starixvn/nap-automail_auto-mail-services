using System;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using Quartz;

namespace Fanex.Service.Core.MailService
{
	public abstract class ScheduleMailBase : JobBase
	{
		protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
		{
			SendMailToNormalGroup();
			return SendMailToVipGroup(schedule, jobContext);
		}

		public abstract JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext);

		public virtual void SendMailToNormalGroup()
		{
		}
	}
}
