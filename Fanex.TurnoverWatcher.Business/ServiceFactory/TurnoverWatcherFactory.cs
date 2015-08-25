// -----------------------------------------------------------------------
// <copyright file="TurnoverWacherFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business.ServiceFactory
{
    using Fanex.Service.Core.MailService;
    using Fanex.Service.Core.MailService.ServiceFactory;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverWatcherFactory : ITimerServiceFactory<IStandaloneTimerFunctionMailService>
    {
        public IStandaloneTimerFunctionMailService GetService()
        {
            return new MailingServiceFactory();
        }
    }
}
