namespace Fanex.OandaExChangeRate.Business.Services.BootStrappers
{
    using Fanex.OandaExChangeRate.Business.Infrastructure.IoC;
    using Fanex.OandaExChangeRate.Business.Infrastructure.ServiceLocatorAdapter;

    using Microsoft.Practices.ServiceLocation;

    using StructureMap;

    public class StructureMapBootStrapper : CommonBootStrapper
    {
        protected override IServiceLocator CreateServiceLocator()
        {
            IContainer container = new Container(registry =>
            {
                //registry.For<ICountryService>().Use<CountryService>();
                //registry.For<ICountryRepository>().Use<CountryRepository>();
                //registry.For<ISettingService>().Use<SettingService>();
                //registry.For<ISettingRepository>().Use<SettingRepository>();
                //registry.For<IUserRepository>().Use<UserRepository>();
                //registry.For<IUserService>().Use<UserService>();
                //registry.For<IFeedbackService>().Use<FeedbackService>();
                //registry.For<IFeedbackRepository>().Use<FeedbackRepository>();
                //registry.For<IUnitOfWork>().Use<EntlibUnitOfWork>();
            });

            return new StructureMapServiceLocator(container);
        }
    }
}
