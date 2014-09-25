namespace Fanex.OandaExChangeRate.Business.Infrastructure.IoC
{
    /// <summary>
    /// Summary description for BootStrapperManager
    /// </summary>
    public class BootStrapperManager
    {
        private static CommonBootStrapper _bootStrapper;

        public static void Initialize(CommonBootStrapper bootStrapper)
        {
            _bootStrapper = bootStrapper;
        }

        public static CommonBootStrapper BootStrapper
        {
            get { return _bootStrapper; }
        }
    }
}

