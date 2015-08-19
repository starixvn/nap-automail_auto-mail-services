namespace Fanex.Service.Core.MailService
{
    public interface ITimerFunctionMailService
    {
        int Interval { get; set; }

        void NotifyMail();
    }
}