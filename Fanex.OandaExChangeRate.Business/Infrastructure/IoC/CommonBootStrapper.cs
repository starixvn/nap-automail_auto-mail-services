namespace Fanex.OandaExChangeRate.Business.Infrastructure.IoC
{
    using Microsoft.Practices.ServiceLocation;

    public abstract class CommonBootStrapper
    {
        public static IServiceLocator Locator;

        protected CommonBootStrapper()
        {
            Locator = CreateServiceLocator();
        }

        protected abstract IServiceLocator CreateServiceLocator();
    }
}
