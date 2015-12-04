using common;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking
{
    public enum ProtocalStage
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }

    public class Client
    {
        private static ILog log = LogManager.GetLogger(typeof(Client));

        private Socket skt;
        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }

        public int Id { get; internal set; }
        public RealmManager Manager { get; private set; }
        public Socket Socket { get { return skt; } }

        public Client(RealmManager manager, Socket skt)
        {
            this.skt = skt;
            this.Manager = manager;
            ReceiveKey = new RC4(new byte[] { 0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1b, 0x09, 0xa1, 0x3a, 0x2a, 0x6e });
            SendKey = new RC4(new byte[] { 0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcb, 0xd7, 0x4b, 0x80 });
        }

        private NetworkHandler handler;

        public void BeginProcess()
        {
            log.Info($"Received client @ {skt.RemoteEndPoint}");
            handler = new NetworkHandler(this, skt);
            handler.BeginHandling();
        }

        public void SendPacket(Packet pkt)
        {
            handler.SendPacket(pkt);
        }

        public void SendPackets(IEnumerable<Packet> pkts)
        {
            handler.SendPackets(pkts);
        }

        public bool IsReady()
        {
            if (Stage == ProtocalStage.Disconnected)
                return false;
            if (Stage == ProtocalStage.Ready &&
                (Player == null || Player != null && Player.Owner == null))
                return false;
            return true;
        }

        internal void ProcessPacket(Packet pkt)
        {
            try
            {
                log.Logger.Log(typeof(Client), Level.Verbose, $"Handling packet \"{pkt.ID}\"", null);
                if (pkt.ID == PacketID.Packet) return;
                IPacketHandler handler;
                if (!PacketHandlers.Handlers.TryGetValue(pkt.ID, out handler))
                    log.Warn($"Unhandled packet \"{pkt.ID}\"");
                else
                    handler.Handle(this, (ClientPacket)pkt);
            }
            catch (Exception e)
            {
                log.Error($"Error when handlig packet \"{pkt.ToString()}\"", e);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (Stage == ProtocalStage.Disconnected) return;
            log.Debug($"Disconnecting client @ {skt.RemoteEndPoint}");
            var original = Stage;
            Stage = ProtocalStage.Disconnected;
            if (Account != null)
                DisconnectFromRealm();
            skt.Close();
        }

        private void Save() //Only when disconnect
        {
            if (Character != null && Player != null)
            {
                log.Debug("Saving character");
                Player.SaveToCharacter();
                bool x = Manager.Database.SaveCharacter(Account, Character, true);
            }
            Manager.Database.ReleaseLock(Account);
        }

        public DbChar Character { get; internal set; }
        public DbAccount Account { get; internal set; }
        public ProtocalStage Stage { get; internal set; }
        public Player Player { get; internal set; }
        public wRandom Random { get; internal set; }

        //Temporary connection state
        internal int targetWorld = -1;

        //Following must execute, network loop will discard disconnected client, so logic loop
        private void DisconnectFromRealm()
        {
            Manager.Logic.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                Manager.Disconnect(this);
            }, PendingPriority.Destruction);
        }

        public void Reconnect(ReconnectPacket pkt)
        {
            log.Info($"Reconnecting client @ {skt.RemoteEndPoint} to {pkt.Name}");
            Manager.Logic.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                Manager.Disconnect(this);
                SendPacket(pkt);
            }, PendingPriority.Destruction);
        }
    }
}
