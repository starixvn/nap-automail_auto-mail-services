namespace Fanex.Service.Core.MailService
{
    using System;

    public interface ITimerService<in T> : IDisposable where T : ITimerFunctionMailService
    {
        void Register(T functionMailService);

        void Start();
    }
}