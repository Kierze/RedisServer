using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;

namespace wServer.networking.handlers
{
    internal class EscapePacketHandler : PacketHandlerBase<EscapePacket>
    {
        public override PacketID ID { get { return PacketID.ESCAPE; } }

        protected override void HandlePacket(Client client, EscapePacket packet)
        {
            client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.NEXUS_ID,
                Name = "Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}
