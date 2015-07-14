// -----------------------------------------------------------------------
// <copyright file="TurnoverWacherFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business.ServiceFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.MailService.ServiceFactory;
    using Fanex.Service.Core.MailService;

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
