using common;
using log4net;
using log4net.Config;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using wServer.networking;
using wServer.realm;

namespace wServer
{
    internal static class Program
    {
        internal static Settings Settings;

        private static ILog log = LogManager.GetLogger("Server");

        private static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo("resources/config/log4net_wServer.config"));

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            using (Settings = new Settings("wServer"))
            using (var db = new Database(
                        Settings.GetValue<string>("db_host", "127.0.0.1"),
                        Settings.GetValue<int>("db_port", "6379"),
                        Settings.GetValue<string>("db_auth", "")))
            {
                RealmManager manager = new RealmManager(
                    Settings.GetValue<int>("maxClient", "100"),
                    Settings.GetValue<int>("tps", "20"),
                    db);

                manager.Initialize();
                manager.Run();

                Server server = new Server(manager, 2050);
                PolicyServer policy = new PolicyServer();

                Console.CancelKeyPress += (sender, e) => e.Cancel = true;

                policy.Start();
                server.Start();
                log.Info("Server initialized.");

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                log.Info("Terminating...");
                server.Stop();
                policy.Stop();
                manager.Stop();
                //db.Dispose();
                log.Info("Server terminated.");
            }
        }
    }
}
