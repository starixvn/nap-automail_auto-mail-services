namespace Fanex.Service.Core.MailService
{
    using System;

    public abstract class TimerFunctionMailServiceBase : ITimerFunctionMailService
    {
        public virtual int Interval { get; set; }

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
    }
}