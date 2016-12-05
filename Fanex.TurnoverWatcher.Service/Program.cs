using SimpleInjector;
using Topshelf;
using Topshelf.SimpleInjector;

namespace Fanex.TurnoverWatcher.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new Container();
            TurnoverWatcherInjector.Register(container);
            container.Verify();

            HostFactory.Run(x =>
            {
                x.UseSimpleInjector(container);

                x.Service<TurnoverWatcherNotificationService>(s =>
                {
                    s.ConstructUsingSimpleInjector();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.SetDescription("Fanex.TurnoverWatcher.Service Host");
                x.SetDisplayName("Fanex.TurnoverWatcher.Service");
                x.SetServiceName("Fanex.TurnoverWatcher.Service");
            });
        }
    }
}