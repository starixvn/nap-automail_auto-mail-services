using Fanex.Service.Core.MailService;
using Fanex.Service.Core.MailService.ServiceFactory;

namespace Fanex.NAPReport.GameWinLimitControl.Business.ServiceFactory
{
    public class WinLimitControlFactory : ITimerServiceFactory<IStandaloneTimerFunctionMailService>
    {
        public IStandaloneTimerFunctionMailService GetService()
        {
            return new MailingServiceFactory();
        }
    }
}
