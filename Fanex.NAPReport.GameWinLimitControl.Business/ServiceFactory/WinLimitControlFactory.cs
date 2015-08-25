namespace Fanex.NAPReport.GameWinLimitControl.Business.ServiceFactory
{
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.MailService.ServiceFactory;

    public class WinLimitControlFactory : ITimerServiceFactory<IStandaloneTimerFunctionMailService>
    {
        public IStandaloneTimerFunctionMailService GetService()
        {
            return new MailingServiceFactory();
        }
    }
}
