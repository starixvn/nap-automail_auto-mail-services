using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Fanex.Service.Core.MailService.ServiceFactory
{
	public class TimerServiceFactoryLoader
	{
		public TimerServiceFactoryLoader(string fileNamePath)
		{
			CommonTimerFunctionMailServices = new List<ICommonTimerFunctionMailService>();
			StandaloneTimerFunctionMailServices = new List<IStandaloneTimerFunctionMailService>();

			XElement timerServicesXElement = XElement.Load(fileNamePath);

			LoadCommonTimerServices(timerServicesXElement);
			LoadStandaloneTimerServices(timerServicesXElement);
		}

		private void LoadCommonTimerServices(XElement timerServicesXElement)
		{
			XElement commonElements = timerServicesXElement.Element(CommonTimerServices);

			CommonInterval = GetNormalizedInterval(commonElements);

			foreach (XElement commonTimerServiceXElement in commonElements.Elements())
			{
				ICommonTimerFunctionMailService commonTimerService =
					GetService<ICommonTimerFunctionMailService>(commonTimerServiceXElement, CommonInterval);

				CommonTimerFunctionMailServices.Add(commonTimerService);
			}
		}

		private void LoadStandaloneTimerServices(XElement timerServicesXElement)
		{
			XElement standaloneElements = timerServicesXElement.Element(StandaloneTimerServices);
			foreach (XElement standaloneTimerServiceXElement in standaloneElements.Elements())
			{
				int interval = GetNormalizedInterval(standaloneTimerServiceXElement);

				IStandaloneTimerFunctionMailService standaloneTimerService =
					GetService<IStandaloneTimerFunctionMailService>(standaloneTimerServiceXElement, interval);

				StandaloneTimerFunctionMailServices.Add(standaloneTimerService);
			}
		}

		private T GetService<T>(XElement xTimerServicesXElement, int interval) where T : ITimerFunctionMailService
		{
			string serviceFactoryFullTypeName = xTimerServicesXElement.Attribute(ServiceFactoryFullTypeName).Value;
			Type serviceFactoryType = Type.GetType(serviceFactoryFullTypeName);

			ITimerServiceFactory<T> timerServiceFactory = (ITimerServiceFactory<T>)Activator.CreateInstance(serviceFactoryType);

			T commonTimerFunctionMailService = timerServiceFactory.GetService();

			// Local interval has highest precedence
			if (commonTimerFunctionMailService.Interval == 0)
			{
				commonTimerFunctionMailService.Interval = interval;
			}

			return commonTimerFunctionMailService;
		}

		private int GetNormalizedInterval(XElement xXElement)
		{
			int interval = int.Parse(xXElement.Attribute(Interval).Value);

			string strTimeUnit = xXElement.Attribute(TimeUnit) == null ? MailService.TimeUnit.Second.ToString() : xXElement.Attribute(TimeUnit).Value;
			TimeUnit timeUnit = (TimeUnit)Enum.Parse(typeof(TimeUnit), strTimeUnit);

			return IntervalConverter.ToMiliSeconds(interval, timeUnit);
		}

		public int CommonInterval { get; private set; }

		public IList<ICommonTimerFunctionMailService> CommonTimerFunctionMailServices { get; private set; }

		public IList<IStandaloneTimerFunctionMailService> StandaloneTimerFunctionMailServices { get; private set; }

		private const string CommonTimerServices = "CommonTimerServices";

		private const string StandaloneTimerServices = "StandaloneTimerServices";

		private const string Interval = "interval";

		private const string TimeUnit = "timeUnit";

		private const string ServiceFactoryFullTypeName = "serviceFactoryFullTypeName";
	}
}
