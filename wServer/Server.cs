using log4net;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using wServer.networking;
using wServer.realm;

namespace wServer
{
    internal class Server
    {
        private static ILog log = LogManager.GetLogger(typeof(Server));

        public Socket Socket { get; private set; }
        public RealmManager Manager { get; private set; }

        public Server(RealmManager manager, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Manager = manager;
        }

        public void Start()
        {
            log.Info("Starting server...");
            Socket.Bind(new IPEndPoint(IPAddress.Any, 2050));
            Socket.Listen(0xff);
            Socket.BeginAccept(Listen, null);
        }

        private void Listen(IAsyncResult ar)
        {
            if (!Socket.IsBound) return;
            var cliSkt = Socket.EndAccept(ar);
            Socket.BeginAccept(Listen, null);
            if (cliSkt != null)
            {
                var client = new Client(Manager, cliSkt);
                client.BeginProcess();
            }
        }

        public void Stop()
        {
            log.Info("Stoping server...");
            foreach (var i in Manager.Clients.Values.ToArray())
                i.Disconnect();
            Socket.Close();
        }
    }
}
