using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class PongPacketHandler : PacketHandlerBase<PongPacket>
    {
        public override PacketID ID { get { return PacketID.Pong; } }

        protected override void HandlePacket(Client client, PongPacket packet)
        {
            client.Player.Pong(packet.Time);
        }
    }
}
