using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;
using SimpleInjector;

namespace Fanex.TurnoverWatcher.Service
{
    public static class TurnoverWatcherInjector
    {
        public static void Register(Container container)
        {
            container.Register<IUMControlAdapter, UMControlAdapter>(Lifestyle.Transient);
        }
    }
}