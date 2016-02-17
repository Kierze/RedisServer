using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class PongHandler : PacketHandlerBase<PongPacket>
    {
        public override PacketID ID { get { return PacketID.PONG; } }

        protected override void HandlePacket(Client client, PongPacket packet)
        {
            client.Player.Pong(packet.Time);
        }
    }
}
