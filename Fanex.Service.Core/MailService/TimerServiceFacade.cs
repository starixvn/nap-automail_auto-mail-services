using Fanex.Common.Extensions;
using Fanex.Service.Core.MailService.ServiceFactory;

namespace Fanex.Service.Core.MailService
{
	public class TimerServiceFacade
	{
		private CommonTimerService _commonTimerService;
		private StandaloneTimerService _standaloneTimerService;

		public TimerServiceFacade(string fileNamePath)
		{
			TimerServiceFactoryLoader timerServiceFactoryLoader = new TimerServiceFactoryLoader(fileNamePath);

			_commonTimerService = new CommonTimerService(timerServiceFactoryLoader.CommonInterval);
			_standaloneTimerService = new StandaloneTimerService();

			timerServiceFactoryLoader.CommonTimerFunctionMailServices.ForEach(n => _commonTimerService.Register(n));
			timerServiceFactoryLoader.StandaloneTimerFunctionMailServices.ForEach(n => _standaloneTimerService.Register(n));
		}

		public void StartServices()
		{
			_commonTimerService.Start();
			_standaloneTimerService.Start();
		}

		public void StopServices()
		{
			_commonTimerService.Dispose();
			_standaloneTimerService.Dispose();
		}
	}
}
