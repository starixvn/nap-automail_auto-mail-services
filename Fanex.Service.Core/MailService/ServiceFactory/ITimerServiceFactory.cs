namespace Fanex.Service.Core.MailService.ServiceFactory
{
    public interface ITimerServiceFactory<out T> where T : ITimerFunctionMailService
    {
        T GetService();
    }
}