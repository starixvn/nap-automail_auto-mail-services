namespace Fanex.OandaExChangeRate.Business.Infrastructure.IoC
{
    using Microsoft.Practices.ServiceLocation;

    public abstract class CommonBootStrapper
    {
        public static IServiceLocator Locator;

        protected CommonBootStrapper()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Locator = CreateServiceLocator();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected abstract IServiceLocator CreateServiceLocator();
    }
}
