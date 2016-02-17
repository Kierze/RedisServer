using log4net;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using wServer.networking;
using wServer.realm;

namespace wServer.networking
{
    internal class Server
    {
        private static ILog log = LogManager.GetLogger(nameof(Server));

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
            Socket skt = null;
            try
            {
                skt = Socket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
            }
            try
            {
                Socket.BeginAccept(Listen, null);
            }
            catch (ObjectDisposedException)
            {
            }
            if (skt != null)
            {
                var client = new Client(Manager, skt);
                client.BeginProcess();
            }
        }

        public void Stop()
        {
            log.Info("Stopping server...");
            foreach (var i in Manager.Clients.Values.ToArray())
                i.Disconnect();                
            Socket.Close();
        }
    }
}
