namespace Fanex.OandaExChangeRate.Business.Services.BootStrappers
{
    using Fanex.OandaExChangeRate.Business.DataAccess;
    using Fanex.OandaExChangeRate.Business.Infrastructure.IoC;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Summary description for UnityBootStrapper
    /// </summary>
    public class UnityBootStrapper : CommonBootStrapper
    {
        protected override IServiceLocator CreateServiceLocator()
        {
            IUnityContainer container = new UnityContainer();
            Register(container);
            return new UnityServiceLocator(container);
        }

        /// <summary>
        /// Register all type to resolve 
        /// <see cref="http://msdn.microsoft.com/en-us/library/ff660872%28PandP.20%29.aspx"/>
        /// </summary>
        /// <param name="container"></param>
        private static void Register(IUnityContainer container)
        {
            /*
             * Life time manager url: http://msdn.microsoft.com/en-us/library/ff660872%28PandP.20%29.aspx
             * 
             */

            container
                .RegisterType<IExchangeRateDAO, ExchangeRateDAO>();
            //    .RegisterType<ICurrencyConvertor, WebServicesApiConvertor>();
            //    .RegisterType<ICountryService, CountryService>()
            //    .RegisterType<IUserService, UserService>()
            //    .RegisterType<IUserRepository, UserRepository>()
            //    .RegisterType<ISettingRepository, SettingRepository>()
            //    .RegisterType<ISettingService, SettingService>()
            //    .RegisterType<IFeedbackRepository, FeedbackRepository>()
            //    .RegisterType<IFeedbackService, FeedbackService>()
            //    .RegisterType<ICustomerRepository, CustomerRepository>()
            //    .RegisterType<ICustomerService, CustomerService>()
            //    .RegisterType<IUnitOfWork, EntlibUnitOfWork>();
                
        }
    }
}