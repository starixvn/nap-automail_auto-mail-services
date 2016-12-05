using Topshelf;

namespace Fanex.TurnoverWatcher.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TurnoverWatcherNotificationService>(s =>
                {
                    s.ConstructUsing(name => new TurnoverWatcherNotificationService());
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